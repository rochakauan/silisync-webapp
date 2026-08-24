namespace application.silisync.Interfaces.Application;

public interface IProductApplication
{
    Task<string> GetAllAsync(CancellationToken cancellationToken = default);
    Task<string> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}