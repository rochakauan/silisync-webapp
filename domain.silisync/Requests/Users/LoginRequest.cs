using System.ComponentModel.DataAnnotations;
using domain.silisync.Abstractions;

namespace domain.silisync.Requests.Users;

public sealed class LoginRequest : Request
{
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MinLength(6, ErrorMessage = "Password must have at least 6 characters")]
    [MaxLength(40, ErrorMessage = "Password is too long")]
    public string Password { get; set; } = string.Empty;
}