using domain.silisync.Enums;

namespace domain.silisync.Common.Results.Errors;

public sealed class AuthError : ResultError
{
    private AuthError(string code, string message, EErrorCategory category, IEnumerable<string>? details = null) : 
        base(code, message, category, details) { }
    
    public static AuthError EmailAlreadyInUse(string email)
        => new("AUTH_EMAIL_IN_USE", $"Email '{email}' is already in use.", EErrorCategory.Conflict);

    public static AuthError InvalidCredentials()
        => new("AUTH_INVALID_CREDENTIALS", "Invalid email or password", EErrorCategory.Unauthorized);
    
    public static AuthError Validation(IEnumerable<string> details)
        => new("AUTH_VALIDATION", "Invalid field(s)", EErrorCategory.Validation, details);
}