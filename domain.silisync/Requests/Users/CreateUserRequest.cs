using System.ComponentModel.DataAnnotations;
using domain.silisync.Abstractions;

namespace domain.silisync.Requests.Users;

public sealed class CreateUserRequest : Request
{
    [Required(ErrorMessage = "Username must not have white spaces, just letters or digits")]
    [MaxLength(100)]
    public string Username { get; init; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; init; } = string.Empty;
    
    [Required]
    [MinLength(6, ErrorMessage = "Password must have at least 6 characters")]
    [MaxLength(40, ErrorMessage = "Password is too long")]
    public string Password { get; init; } = string.Empty;
}