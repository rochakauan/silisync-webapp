using domain.silisync.Enums;

namespace domain.silisync.Common.Results.Errors;

public sealed class ApplicationUsersError : ResultError
{
    private ApplicationUsersError(string code, string message, EErrorCategory category, IEnumerable<string>? details = null) : 
        base(code, message, category, details) { }
    
    public static ApplicationUsersError SqlError(string errorMessage)
        => new("ID_USERS_SQL_ERROR", errorMessage, EErrorCategory.Unexpected);
    
    public static ApplicationUsersError UnexpectedError(string errorMessage)
        => new("ID_USERS_UNEXPECTED", errorMessage, EErrorCategory.Unexpected);
}