using System.ComponentModel.DataAnnotations;

namespace application.silisync.Dtos;

public sealed class RegisterDto
{
    [Required(ErrorMessage = "Username must not have white spaces, just letters or digits")]
    [MaxLength(100)]
    public string Username { get; init; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [MaxLength(255, ErrorMessage = "Email is too long")]
    public string Email { get; init; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    [MaxLength(60, ErrorMessage = "Password is too long")]
    public string Password { get; init; } = string.Empty;
}