using System.ComponentModel.DataAnnotations;

namespace auth_service.Models
{
    public class SignUpDTO
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class RefreshTokenDTO
    {
        [Required]
        public string AccessToken  { get; set; } = string.Empty;

        [Required]
        public string RefreshToken { get; set; } = string.Empty;
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
}