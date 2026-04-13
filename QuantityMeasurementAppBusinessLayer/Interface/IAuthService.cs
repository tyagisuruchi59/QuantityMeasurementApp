using QuantityMeasurementAppModel.DTOs;

namespace QuantityMeasurementAppBusinessLayer.Interface
{
    /// <summary>
    /// Service interface for authentication and authorization.
    /// Handles sign-up, login, token refresh, logout and google login.
    /// </summary>
    public interface IAuthService
    {
        Task<AuthResponseDTO> SignUpAsync(SignUpDTO signUpDto);
        Task<AuthResponseDTO> LoginAsync(LoginDTO loginDto);
        Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenDTO refreshDto);
        Task LogoutAsync(string jti, string refreshToken);
        Task<AuthResponseDTO> GoogleLoginAsync(GoogleAuthDTO dto);
    }
}