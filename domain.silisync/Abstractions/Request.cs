namespace domain.silisync.Abstractions;

public abstract class Request
{
    public readonly Guid UserId = Guid.NewGuid();
}