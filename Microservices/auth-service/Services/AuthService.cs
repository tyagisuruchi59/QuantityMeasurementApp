using auth_service.Data;
using auth_service.Models;
using Microsoft.EntityFrameworkCore;

namespace auth_service.Services
{
    public class AuthService
    {
        private readonly AuthDbContext _db;
        private readonly TokenService _tokenService;
        private readonly IConfiguration _config;

        public AuthService(AuthDbContext db, TokenService tokenService, IConfiguration config)
        {
            _db           = db;
            _tokenService = tokenService;
            _config       = config;
        }

        // ── Signup ──────────────────────────────────
        public async Task<AuthResponseDTO> SignUpAsync(SignUpDTO dto)
        {
            // Check duplicate
            if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
                throw new InvalidOperationException("Email already exists");

            if (await _db.Users.AnyAsync(u => u.Username == dto.Username))
                throw new InvalidOperationException("Username already exists");

            var user = new UserEntity
            {
                Username     = dto.Username,
                Email        = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role         = "User"
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return new AuthResponseDTO { Message = "Account created successfully" };
        }

        // ── Login ────────────────────────────────────
        public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email)
                ?? throw new UnauthorizedAccessException("Invalid credentials");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            var accessToken  = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Save refresh token
            user.RefreshToken       = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(
                double.Parse(_config["Jwt:RefreshTokenExpiryDays"] ?? "7"));
            await _db.SaveChangesAsync();

            return new AuthResponseDTO
            {
                AccessToken  = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt    = DateTime.UtcNow.AddMinutes(
                    double.Parse(_config["Jwt:ExpiryMinutes"] ?? "1440")),
                Username = user.Username,
                Email    = user.Email,
                Role     = user.Role,
                Message  = "Login successful"
            };
        }

        // ── Logout ───────────────────────────────────
        public async Task LogoutAsync(string refreshToken)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user != null)
            {
                user.RefreshToken       = null;
                user.RefreshTokenExpiry = null;
                await _db.SaveChangesAsync();
            }
        }

        // ── Refresh Token ────────────────────────────
        public async Task<AuthResponseDTO> RefreshAsync(RefreshTokenDTO dto)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == dto.RefreshToken)
                ?? throw new UnauthorizedAccessException("Invalid refresh token");

            if (user.RefreshTokenExpiry < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token expired");

            var accessToken  = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken       = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _db.SaveChangesAsync();

            return new AuthResponseDTO
            {
                AccessToken  = accessToken,
                RefreshToken = refreshToken,
                Username     = user.Username,
                Email        = user.Email,
                Role         = user.Role,
                Message      = "Token refreshed"
            };
        }
    }
}