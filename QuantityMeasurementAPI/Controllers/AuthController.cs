using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using QuantityMeasurementAppBusinessLayer.Interface;
using QuantityMeasurementAppModel.DTOs;

namespace QuantityMeasurementAPI.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    [Produces("application/json")]
    [SwaggerTag("Authentication - User registration and JWT token management")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Register a new user", OperationId = "SignUp")]
        [SwaggerResponse(201, "User created successfully")]
        [SwaggerResponse(400, "Validation error or duplicate email/username")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDTO dto)
        {
            _logger.LogInformation("POST /signup for email: {Email}", dto.Email);

            if (!ModelState.IsValid)
                return BadRequest(ApiResponseDTO<object>.Fail(
                    "Validation failed",
                    ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()));

            try
            {
                var result = await _authService.SignUpAsync(dto);
                return StatusCode(201, ApiResponseDTO<AuthResponseDTO>.Ok(result, result.Message));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("SignUp conflict: {Message}", ex.Message);
                return Conflict(ApiResponseDTO<object>.Fail(ex.Message));
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Login and receive JWT tokens", OperationId = "Login")]
        [SwaggerResponse(200, "Login successful")]
        [SwaggerResponse(401, "Invalid credentials")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            _logger.LogInformation("POST /login for email: {Email}", dto.Email);

            if (!ModelState.IsValid)
                return BadRequest(ApiResponseDTO<object>.Fail(
                    "Validation failed",
                    ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()));

            try
            {
                var result = await _authService.LoginAsync(dto);
                return Ok(ApiResponseDTO<AuthResponseDTO>.Ok(result, result.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Login failed: {Message}", ex.Message);
                return Unauthorized(ApiResponseDTO<object>.Fail(ex.Message));
            }
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Refresh access token", OperationId = "RefreshToken")]
        [SwaggerResponse(200, "New tokens issued")]
        [SwaggerResponse(401, "Invalid or expired refresh token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO dto)
        {
            _logger.LogInformation("POST /refresh");

            if (!ModelState.IsValid)
                return BadRequest(ApiResponseDTO<object>.Fail("Validation failed"));

            try
            {
                var result = await _authService.RefreshTokenAsync(dto);
                return Ok(ApiResponseDTO<AuthResponseDTO>.Ok(result, result.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Token refresh failed: {Message}", ex.Message);
                return Unauthorized(ApiResponseDTO<object>.Fail(ex.Message));
            }
        }

        [HttpPost("logout")]
        [Authorize]
        [SwaggerOperation(Summary = "Logout current user", OperationId = "Logout")]
        [SwaggerResponse(200, "Logged out successfully")]
        [SwaggerResponse(401, "Not authenticated")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenDTO dto)
        {
            var jti = User.FindFirstValue(
                          System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti)
                   ?? User.FindFirstValue("jti")
                   ?? string.Empty;

            _logger.LogInformation("POST /logout jti={Jti}", jti);

            await _authService.LogoutAsync(jti, dto.RefreshToken);
            return Ok(ApiResponseDTO<object>.Ok(null!, "Logged out successfully"));
        }

        [HttpGet("me")]
        [Authorize]
        [SwaggerOperation(Summary = "Get current user profile", OperationId = "GetMe")]
        [SwaggerResponse(200, "User profile")]
        [SwaggerResponse(401, "Not authenticated")]
        public IActionResult GetMe()
        {
            var profile = new
            {
                Id = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue("sub"),
                Email = User.FindFirstValue(ClaimTypes.Email)
                      ?? User.FindFirstValue(
                          System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email),
                Username = User.FindFirstValue(ClaimTypes.Name)
                        ?? User.FindFirstValue(
                            System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.UniqueName),
                Role = User.FindFirstValue(ClaimTypes.Role)
            };

            return Ok(ApiResponseDTO<object>.Ok(profile));
        }
    }
}