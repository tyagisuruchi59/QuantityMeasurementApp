using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using QuantityMeasurementAPI.Config;
using QuantityMeasurementAPI.Middleware;
using QuantityMeasurementAppBusinessLayer.Interface;
using QuantityMeasurementAppBusinessLayer.Service;
using QuantityMeasurementAppRepositoryLayer.Context;
using QuantityMeasurementAppRepositoryLayer.Interface;
using QuantityMeasurementAppRepositoryLayer.Service;
using QuantityMeasurementAppRepositoryLayer.Utilities;

// ─── NLog early init ──────────────────────────────────────────────────────────
var logger = LogManager.Setup()
    .LoadConfigurationFromAppSettings()
    .GetCurrentClassLogger();

try
{
    logger.Info("Starting Quantity Measurement API...");

    var builder = WebApplication.CreateBuilder(args);

    // ─── NLog as the logging provider ────────────────────────────────────────
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // ─── Controllers + Validation Filter ─────────────────────────────────────
    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ValidationExceptionFilter>();
    });

    // ─── EF Core (InMemory for dev; swap for SQL Server in prod) ─────────────
    var useInMemory = builder.Configuration.GetValue<bool>("Database:UseInMemory", true);
    if (useInMemory)
    {
        builder.Services.AddDbContext<QuantityDbContext>(opt =>
            opt.UseInMemoryDatabase("QuantityMeasurementDb"));
        logger.Info("Using In-Memory database");
    }
    else
    {
        builder.Services.AddDbContext<QuantityDbContext>(opt =>
            opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        logger.Info("Using PostgreSQL database");
    }

    // ─── Redis Distributed Cache ──────────────────────────────────────────────
    var redisConnection = builder.Configuration.GetConnectionString("Redis");
    if (!string.IsNullOrWhiteSpace(redisConnection))
    {
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnection;
            options.InstanceName = "QMA:";
        });
        logger.Info("Redis cache configured: {Connection}", redisConnection);
    }
    else
    {
        // Fallback to in-memory cache if Redis not configured
        builder.Services.AddDistributedMemoryCache();
        logger.Warn("Redis not configured — using in-memory distributed cache");
    }

    // ─── Repository Layer ─────────────────────────────────────────────────────
    builder.Services.AddScoped<IQuantityMeasurementRepository, QuantityRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();

    // ─── Utilities ────────────────────────────────────────────────────────────
    builder.Services.AddScoped<PasswordService>();
    builder.Services.AddScoped<JwtTokenService>();
    builder.Services.AddScoped<RedisService>();

    // ─── Business / Service Layer ─────────────────────────────────────────────
    builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();
    builder.Services.AddScoped<IAuthService, AuthService>();

    // ─── Middleware ───────────────────────────────────────────────────────────
    builder.Services.AddTransient<ExceptionMiddleware>();

    // ─── JWT Authentication + Authorization ───────────────────────────────────
    builder.Services.AddJwtAuthentication(builder.Configuration);
    builder.Services.AddAuthorizationPolicies();

    // ─── CORS ─────────────────────────────────────────────────────────────────
    builder.Services.AddCorsPolicy();

    // ─── Swagger / OpenAPI ────────────────────────────────────────────────────
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerWithJwt();

    // ─── Health Checks ────────────────────────────────────────────────────────
    builder.Services.AddHealthChecks();

    // ─── HTTP Context Accessor ────────────────────────────────────────────────
    builder.Services.AddHttpContextAccessor();

    // ─────────────────────────────────────────────────────────────────────────
    var app = builder.Build();

    // ─── Auto-migrate database on startup ────────────────────────────────────
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<QuantityDbContext>();
        if (!useInMemory)
        {
            db.Database.Migrate();
            logger.Info("Database migrations applied");
        }
        else
        {
            db.Database.EnsureCreated();
        }
    }

    var swaggerEnabled = builder.Configuration.GetValue<bool>("Swagger:Enabled", true);
    if (app.Environment.IsDevelopment() || swaggerEnabled)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quantity Measurement API v1");
            c.RoutePrefix = "swagger";
            c.DisplayRequestDuration();
            c.EnableDeepLinking();
        });
    }

    app.UseHttpsRedirection();

    // CORS before auth
    app.UseCors(app.Environment.IsDevelopment() ? "DevelopmentCors" : "ProductionCors");

    // Exception handling
    app.UseMiddleware<ExceptionMiddleware>();

    // Auth pipeline: authenticate → blacklist check → authorize
    app.UseAuthentication();
    app.UseMiddleware<JwtBlacklistMiddleware>();
    app.UseAuthorization();

    // Health check endpoints
    app.MapHealthChecks("/actuator/health");
    app.MapGet("/actuator/info", () => new
    {
        application = "quantity-measurement-api",
        version = "1.0.0",
        environment = app.Environment.EnvironmentName
    });

    app.MapControllers();

    logger.Info("Quantity Measurement API started successfully");

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Application failed to start");
    throw;
}
finally
{
    LogManager.Shutdown();
}

// Required for WebApplicationFactory in integration tests
public partial class Program { }