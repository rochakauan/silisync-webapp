using domain.silisync.Enums;

namespace domain.silisync.Common.Results.Errors;

public sealed class RepositoryError : ResultError
{
    private RepositoryError(string code, string message, EErrorCategory category)
        : base(code, message, category) { }
    
    public static RepositoryError NotFound(string key)
        => new("REPO_NOT_FOUND", $"{key} not found.", EErrorCategory.NotFound);

    public static RepositoryError Critical(string error)
        => new("REPO_FATAL_CRITICAL", error, EErrorCategory.Unexpected);
    
    public static RepositoryError Unexpected(string error)
        => new("REPO_UNEXPECTED", error, EErrorCategory.Unexpected);
}