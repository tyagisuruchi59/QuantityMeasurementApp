using auth_service.Models;
using auth_service.Services;
using Microsoft.AspNetCore.Mvc;

namespace auth_service.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponseDTO<object>.Fail("Validation failed"));

            try
            {
                var result = await _authService.SignUpAsync(dto);
                return StatusCode(201,
                    ApiResponseDTO<AuthResponseDTO>.Ok(result, result.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResponseDTO<object>.Fail(ex.Message));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponseDTO<object>.Fail("Validation failed"));

            try
            {
                var result = await _authService.LoginAsync(dto);
                return Ok(ApiResponseDTO<AuthResponseDTO>.Ok(result, result.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponseDTO<object>.Fail(ex.Message));
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenDTO dto)
        {
            await _authService.LogoutAsync(dto.RefreshToken);
            return Ok(ApiResponseDTO<object>.Ok(null!, "Logged out successfully"));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDTO dto)
        {
            try
            {
                var result = await _authService.RefreshAsync(dto);
                return Ok(ApiResponseDTO<AuthResponseDTO>.Ok(result, result.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponseDTO<object>.Fail(ex.Message));
            }
        }
    }
}