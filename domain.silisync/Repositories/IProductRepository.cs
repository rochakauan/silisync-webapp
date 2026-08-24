using domain.silisync.Entities;

namespace domain.silisync.Repositories;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Product>> GetAllDeletedProductsAsync(CancellationToken cancellationToken);
    Task<Product?> GetDeletedProductAsync(Guid id, CancellationToken cancellationToken);
}