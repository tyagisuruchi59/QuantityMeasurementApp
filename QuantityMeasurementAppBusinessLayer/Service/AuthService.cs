using Google.Apis.Auth;
using Microsoft.Extensions.Logging;
using QuantityMeasurementAppBusinessLayer.Interface;
using QuantityMeasurementAppModel.DTOs;
using QuantityMeasurementAppModel.Entities;
using QuantityMeasurementAppRepositoryLayer.Interface;
using QuantityMeasurementAppRepositoryLayer.Utilities;

namespace QuantityMeasurementAppBusinessLayer.Service
{
    /// <summary>
    /// Authentication service - sign-up, login, token refresh, logout, google login.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository      _userRepo;
        private readonly PasswordService      _passwordService;
        private readonly JwtTokenService      _jwtService;
        private readonly RedisService         _redis;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserRepository      userRepo,
            PasswordService      passwordService,
            JwtTokenService      jwtService,
            RedisService         redis,
            ILogger<AuthService> logger)
        {
            _userRepo        = userRepo;
            _passwordService = passwordService;
            _jwtService      = jwtService;
            _redis           = redis;
            _logger          = logger;
        }

        // ─── Sign Up ──────────────────────────────────────────────────────────

        public async Task<AuthResponseDTO> SignUpAsync(SignUpDTO dto)
        {
            _logger.LogInformation("SignUp attempt for email: {Email}", dto.Email);

            if (await _userRepo.ExistsByEmailAsync(dto.Email))
                throw new InvalidOperationException(
                    $"Email '{dto.Email}' is already registered");

            if (await _userRepo.ExistsByUsernameAsync(dto.Username))
                throw new InvalidOperationException(
                    $"Username '{dto.Username}' is already taken");

            var salt         = _passwordService.GenerateSalt();
            var passwordHash = _passwordService.HashPassword(dto.Password, salt);

            var user = new UserEntity
            {
                Username     = dto.Username,
                Email        = dto.Email.ToLower(),
                PasswordHash = passwordHash,
                Salt         = salt,
                Role         = "User",
                IsActive     = true,
                CreatedAt    = DateTime.UtcNow
            };

            var saved = await _userRepo.SaveAsync(user);
            _logger.LogInformation("User registered successfully: {Email}", saved.Email);

            return await IssueTokens(saved, "Registration successful");
        }

        // ─── Login ────────────────────────────────────────────────────────────

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto)
        {
            _logger.LogInformation("Login attempt for email: {Email}", dto.Email);

            var user = await _userRepo.FindByEmailAsync(dto.Email);

            var passwordValid = user != null &&
                _passwordService.VerifyPassword(dto.Password, user.PasswordHash, user.Salt);

            if (!passwordValid || user == null)
            {
                _logger.LogWarning("Login failed for email: {Email}", dto.Email);
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            if (!user.IsActive)
            {
                _logger.LogWarning("Login attempt for inactive account: {Email}", dto.Email);
                throw new UnauthorizedAccessException("Account is inactive");
            }

            user.LastLogin = DateTime.UtcNow;
            await _userRepo.UpdateAsync(user);

            _logger.LogInformation("Login successful for: {Email}", dto.Email);
            return await IssueTokens(user, "Login successful");
        }

        // ─── Google Login ─────────────────────────────────────────────────────

        public async Task<AuthResponseDTO> GoogleLoginAsync(GoogleAuthDTO dto)
        {
            _logger.LogInformation("Google login attempt");

            // Validate the Google token
            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(dto.Token);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Google token validation failed: {Message}", ex.Message);
                throw new UnauthorizedAccessException("Invalid Google token");
            }

            // Check if user already exists
            var user = await _userRepo.FindByEmailAsync(payload.Email);

            if (user == null)
            {
                // Auto-register the Google user
                var salt     = _passwordService.GenerateSalt();
                var randomPw = _passwordService.HashPassword(Guid.NewGuid().ToString(), salt);

                user = new UserEntity
                {
                    Username     = payload.Name ?? payload.Email.Split('@')[0],
                    Email        = payload.Email.ToLower(),
                    PasswordHash = randomPw,
                    Salt         = salt,
                    Role         = "User",
                    IsActive     = true,
                    CreatedAt    = DateTime.UtcNow
                };

                user = await _userRepo.SaveAsync(user);
                _logger.LogInformation("New Google user registered: {Email}", user.Email);
            }
            else
            {
                _logger.LogInformation("Existing Google user logged in: {Email}", user.Email);
            }

            user.LastLogin = DateTime.UtcNow;
            await _userRepo.UpdateAsync(user);

            return await IssueTokens(user, "Google login successful");
        }

        // ─── Refresh Token ────────────────────────────────────────────────────

        public async Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenDTO dto)
        {
            _logger.LogInformation("Token refresh requested");

            var principal = _jwtService.GetPrincipalFromExpiredToken(dto.AccessToken);
            if (principal == null)
                throw new UnauthorizedAccessException("Invalid access token");

            var userIdClaim = principal.FindFirst(
                                  System.Security.Claims.ClaimTypes.NameIdentifier)
                           ?? principal.FindFirst("sub");

            if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out var userId))
                throw new UnauthorizedAccessException("Invalid token claims");

            var user = await _userRepo.FindByIdAsync(userId);
            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            if (user.RefreshToken != dto.RefreshToken)
                throw new UnauthorizedAccessException("Invalid refresh token");

            if (user.RefreshTokenExpiry <= DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token has expired");

            _logger.LogInformation("Token refreshed for user: {Email}", user.Email);
            return await IssueTokens(user, "Token refreshed successfully");
        }

        // ─── Logout ───────────────────────────────────────────────────────────

        public async Task LogoutAsync(string jti, string refreshToken)
        {
            _logger.LogInformation("Logout with jti: {Jti}", jti);

            await _redis.BlacklistTokenAsync(jti);

            var users = await _userRepo.FindAllAsync();
            var user  = users.FirstOrDefault(u => u.RefreshToken == refreshToken);

            if (user != null)
            {
                user.RefreshToken       = null;
                user.RefreshTokenExpiry = null;
                await _userRepo.UpdateAsync(user);
                _logger.LogInformation("Refresh token cleared for: {Email}", user.Email);
            }
        }

        // ─── Private Helper ───────────────────────────────────────────────────

        private async Task<AuthResponseDTO> IssueTokens(UserEntity user, string message)
        {
            var accessToken                   = _jwtService.GenerateAccessToken(user);
            var (refreshToken, refreshExpiry) = _jwtService.GenerateRefreshToken();

            user.RefreshToken       = refreshToken;
            user.RefreshTokenExpiry = refreshExpiry;
            await _userRepo.UpdateAsync(user);

            return new AuthResponseDTO
            {
                AccessToken  = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt    = _jwtService.GetTokenExpiry(),
                Username     = user.Username,
                Email        = user.Email,
                Role         = user.Role,
                Message      = message
            };
        }
    }
}