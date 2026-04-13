using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace QuantityMeasurementAPI.Config
{
    public static class SecurityConfig
    {
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration config)
        {
            var jwtSettings = config.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(
                jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key not configured"));

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = ctx =>
                        {
                            Console.WriteLine($"❌ Auth FAILED: {ctx.Exception.Message}");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = ctx =>
                        {
                            Console.WriteLine($"✅ Token VALID for: {ctx.Principal?.Identity?.Name}");
                            return Task.CompletedTask;
                        },
                        OnMessageReceived = ctx =>
                        {
                            Console.WriteLine($"📨 Token RECEIVED: {ctx.Token?[..20]}...");
                            return Task.CompletedTask;
                        },
                        OnChallenge = async ctx =>
                        {
                            Console.WriteLine($"🚫 Challenge: {ctx.Error} - {ctx.ErrorDescription}");
                            ctx.HandleResponse();
                            ctx.Response.StatusCode = 401;
                            ctx.Response.ContentType = "application/json";
                            await ctx.Response.WriteAsync(
                                "{\"status\":401,\"error\":\"Unauthorized\",\"message\":\"Authentication required. Please provide a valid Bearer token.\"}");
                        },
                        OnForbidden = async ctx =>
                        {
                            ctx.Response.StatusCode = 403;
                            ctx.Response.ContentType = "application/json";
                            await ctx.Response.WriteAsync(
                                "{\"status\":403,\"error\":\"Forbidden\",\"message\":\"You do not have permission to access this resource.\"}");
                        }
                    };
                });

            return services;
        }

        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole("Admin"));

                options.AddPolicy("AuthenticatedUser", policy =>
                    policy.RequireAuthenticatedUser());

                options.AddPolicy("AdminOrUser", policy =>
                    policy.RequireRole("Admin", "User"));
            });

            return services;
        }

        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("DevelopmentCors", policy =>
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());

                options.AddPolicy("ProductionCors", policy =>
                    policy
                        .WithOrigins("https://quantity-measurement-app-frontend-bznatuwtj.vercel.app")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            return services;
        }
    }
}