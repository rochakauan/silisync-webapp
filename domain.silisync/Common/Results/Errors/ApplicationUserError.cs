using domain.silisync.Enums;

namespace domain.silisync.Common.Results.Errors;

public sealed class ApplicationUsersError : ResultError
{
    private ApplicationUsersError(string code, string message, EErrorCategory category, IEnumerable<string>? details = null) : 
        base(code, message, category, details) { }
    
    public static ApplicationUsersError None()
        => new("ID_USERS_NONE", "There's no Application Users registered for now.", EErrorCategory.NotFound);
    
    public static ApplicationUsersError SqlError()
        => new("ID_USERS_SQL_ERROR", "An inner SQL Exception occurred", EErrorCategory.Unexpected);
    
    public static ApplicationUsersError UnexpectedError(string errorMessage)
        => new("ID_USERS_UNEXPECTED", errorMessage, EErrorCategory.Unexpected);
}