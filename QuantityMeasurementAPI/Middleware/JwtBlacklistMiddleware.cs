using QuantityMeasurementAppRepositoryLayer.Utilities;

namespace QuantityMeasurementAPI.Middleware
{
    /// <summary>
    /// Middleware that checks the JWT token's jti (JWT ID) against
    /// the Redis blacklist on every authenticated request.
    ///
    /// This allows immediate token invalidation on logout even before
    /// the token's natural expiry time.
    ///
    /// Pipeline position: after UseAuthentication(), before UseAuthorization()
    /// </summary>
    public class JwtBlacklistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtBlacklistMiddleware> _logger;

        public JwtBlacklistMiddleware(
            RequestDelegate next,
            ILogger<JwtBlacklistMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RedisService redis)
        {
            // Only check authenticated requests
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var jti = context.User.FindFirst("jti")?.Value
                       ?? context.User.FindFirst(
                           System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti)?.Value;

                if (!string.IsNullOrEmpty(jti))
                {
                    var blacklisted = await redis.IsTokenBlacklistedAsync(jti);
                    if (blacklisted)
                    {
                        _logger.LogWarning("Blacklisted token used: jti={Jti}", jti);
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(
                            """{"status":401,"error":"Unauthorized","message":"Token has been revoked"}""");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}