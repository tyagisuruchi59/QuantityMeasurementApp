using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using QuantityMeasurementAppModel.Entities;

namespace QuantityMeasurementAppRepositoryLayer.Utilities
{
    /// <summary>
    /// Service for JWT access token generation and validation,
    /// plus refresh token management.
    ///
    /// JWT Structure:
    ///   Header:  { alg: HS256, typ: JWT }
    ///   Payload: { sub, email, role, jti, iat, exp }
    ///   Signature: HMAC-SHA256 with secret key
    /// </summary>
    public class JwtTokenService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<JwtTokenService> _logger;

        public JwtTokenService(IConfiguration config, ILogger<JwtTokenService> logger)
        {
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// Generate a signed JWT access token for the authenticated user.
        /// Claims: sub, email, username, role, jti, iat
        /// </summary>
        public string GenerateAccessToken(UserEntity user)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]
                    ?? throw new InvalidOperationException("JWT key not configured")));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)
            };

            var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"] ?? "60");
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            _logger.LogInformation("JWT access token generated for user: {Email}", user.Email);
            return tokenString;
        }

        /// <summary>
        /// Generate a cryptographically secure refresh token (256-bit random).
        /// Stored as Base64 in database alongside expiry.
        /// </summary>
        public (string token, DateTime expiry) GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(32);
            var token = Convert.ToBase64String(randomBytes);
            var expiryDays = int.Parse(_config["Jwt:RefreshTokenExpiryDays"] ?? "7");
            var expiry = DateTime.UtcNow.AddDays(expiryDays);
            _logger.LogDebug("Refresh token generated, expires: {Expiry}", expiry);
            return (token, expiry);
        }

        /// <summary>
        /// Validate a JWT access token and return its ClaimsPrincipal.
        /// Returns null if invalid or expired.
        /// </summary>
        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var jwtSettings = _config.GetSection("Jwt");
                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

                var validationParams = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero
                };

                var handler = new JwtSecurityTokenHandler();
                var principal = handler.ValidateToken(token, validationParams, out var securityToken);

                if (securityToken is not JwtSecurityToken jwtToken ||
                    !jwtToken.Header.Alg.Equals(
                        SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    _logger.LogWarning("Token validation failed: invalid algorithm");
                    return null;
                }

                return principal;
            }
            catch (SecurityTokenExpiredException)
            {
                _logger.LogWarning("Token expired");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Token validation failed");
                return null;
            }
        }

        /// <summary>
        /// Extract claims from an EXPIRED token (for refresh flow).
        /// Does NOT validate lifetime - only signature and structure.
        /// </summary>
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            try
            {
                var jwtSettings = _config.GetSection("Jwt");
                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

                var validationParams = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = key
                };

                var handler = new JwtSecurityTokenHandler();
                return handler.ValidateToken(token, validationParams, out _);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to extract principal from expired token");
                return null;
            }
        }

        /// <summary>
        /// Get the expiry DateTime for a new access token.
        /// </summary>
        public DateTime GetTokenExpiry()
        {
            var expiryMinutes = int.Parse(_config["Jwt:ExpiryMinutes"] ?? "60");
            return DateTime.UtcNow.AddMinutes(expiryMinutes);
        }
    }
}