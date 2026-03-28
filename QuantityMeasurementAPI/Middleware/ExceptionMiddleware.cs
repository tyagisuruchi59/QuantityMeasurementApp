using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace QuantityMeasurementAPI.Middleware
{
    // ─────────────────────────────────────────────────────────────────────────
    // Structured error response model
    // ─────────────────────────────────────────────────────────────────────────
    public record ErrorResponse(
        DateTime Timestamp,
        int Status,
        string Error,
        string Message,
        string Path);

    // ─────────────────────────────────────────────────────────────────────────
    // Global Exception Handler Middleware
    // Equivalent to Spring's @ControllerAdvice + @ExceptionHandler
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Centralized exception handling for all REST controllers.
    /// Maps exception types to appropriate HTTP status codes and
    /// returns a consistent JSON error response format.
    ///
    /// Handles:
    /// - ArgumentException / ArgumentNullException    → 400 Bad Request
    /// - InvalidOperationException                    → 400 Bad Request
    /// - UnauthorizedAccessException                  → 401 Unauthorized
    /// - KeyNotFoundException                         → 404 Not Found
    /// - DivideByZeroException                        → 500 Internal Server Error
    /// - All other exceptions                         → 500 Internal Server Error
    /// </summary>
    public class ExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (status, error) = exception switch
            {
                ArgumentException or
                ArgumentNullException or
                InvalidOperationException or
                FormatException => (400, "Bad Request"),

                UnauthorizedAccessException => (401, "Unauthorized"),

                KeyNotFoundException => (404, "Not Found"),

                DivideByZeroException => (500, "Internal Server Error"),

                _ => (500, "Internal Server Error")
            };

            // Don't expose internal details for 500 errors
            var message = status == 500
                ? "An unexpected error occurred. Please try again later."
                : exception.Message;

            // For quantity/validation errors show actual message
            if (exception is InvalidOperationException or ArgumentException)
                message = exception.Message;

            var response = new ErrorResponse(
                Timestamp: DateTime.UtcNow,
                Status: status,
                Error: error,
                Message: message,
                Path: context.Request.Path);

            context.Response.StatusCode = status;
            context.Response.ContentType = "application/json";

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Validation Exception Filter (for ModelState errors)
    // Equivalent to Spring's @Valid handling
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Action filter that intercepts ModelState validation failures
    /// and returns a structured 400 response.
    /// </summary>
    public class ValidationExceptionFilter : IActionFilter
    {
        private readonly ILogger<ValidationExceptionFilter> _logger;

        public ValidationExceptionFilter(ILogger<ValidationExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors.Select(e => new
                    {
                        field = x.Key,
                        message = e.ErrorMessage
                    }))
                    .ToList();

                _logger.LogWarning("Validation failed: {Errors}", errors);

                var response = new
                {
                    timestamp = DateTime.UtcNow,
                    status = 400,
                    error = "Quantity Measurement Error",
                    message = "Unit must be valid for the specified measurement type",
                    path = context.HttpContext.Request.Path.Value
                };

                context.Result = new BadRequestObjectResult(response);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}