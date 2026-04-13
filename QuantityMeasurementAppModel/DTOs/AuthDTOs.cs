using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementAppModel.DTOs
{
   public class SignUpDTO
{
    [Required(ErrorMessage = "Username is required")]
    [MinLength(3, ErrorMessage = "Username must be at least 3 characters")]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    public string Password { get; set; } = string.Empty;
}
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponseDTO
    {
        public string AccessToken  { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt  { get; set; }
        public string Username     { get; set; } = string.Empty;
        public string Email        { get; set; } = string.Empty;
        public string Role         { get; set; } = string.Empty;
        public string Message      { get; set; } = string.Empty;

        public class GoogleAuthDTO
{
    [Required(ErrorMessage = "Google token is required")]
    public string Token { get; set; } = string.Empty;
}
    }

    public class RefreshTokenDTO
    {
        [Required(ErrorMessage = "Access token is required")]
        public string AccessToken  { get; set; } = string.Empty;

        [Required(ErrorMessage = "Refresh token is required")]
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class ApiResponseDTO<T>
    {
        public bool Success        { get; set; }
        public string Message      { get; set; } = string.Empty;
        public T? Data             { get; set; }
        public List<string> Errors { get; set; } = new();
        public DateTime Timestamp  { get; set; } = DateTime.UtcNow;

        public static ApiResponseDTO<T> Ok(T data, string message = "Success")
            => new() { Success = true, Message = message, Data = data };

        public static ApiResponseDTO<T> Fail(string message, List<string>? errors = null)
            => new() { Success = false, Message = message, Errors = errors ?? new() };
    }
    public class GoogleAuthDTO
{
    [Required(ErrorMessage = "Google token is required")]
    public string Token { get; set; } = string.Empty;
}
}