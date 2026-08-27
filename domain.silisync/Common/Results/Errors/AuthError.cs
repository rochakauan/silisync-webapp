using domain.silisync.Enums;

namespace domain.silisync.Common.Results.Errors;

public sealed class AuthError : ResultError
{
    private AuthError(string code, string message, EErrorCategory category, IEnumerable<string>? details = null) : 
        base(code, message, category, details) { }
    
    public static AuthError EmailAlreadyInUse(string email)
        => new("AUTH_EMAIL_IN_USE", $"Email '{email}' is already in use.", EErrorCategory.Conflict);
    
    public static AuthError Validation(IEnumerable<string> details)
        => new("AUTH_VALIDATION", "Oops! We were almost there... Please correct it and try again", EErrorCategory.Validation, details);
    
    public static AuthError InvalidCredentials(string message)
        => new("AUTH_IDENTITY_ERROR", message, EErrorCategory.Unauthorized);

    public static AuthError UserTagGeneration(string message)
        => new("AUTH_TAG_GENERATION", message, EErrorCategory.Unexpected);
}