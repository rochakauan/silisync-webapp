namespace domain.silisync.Enums;

public enum EErrorCategory : byte
{
    NotFound,
    Timeout,
    Validation,
    Conflict,
    Unauthorized,
    Forbidden,
    RateLimited,
    Unexpected
}