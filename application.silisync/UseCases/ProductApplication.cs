using application.silisync.Interfaces.Application;
using domain.silisync.Repositories;

namespace application.silisync.UseCases;

public class ProductApplication(
    IProductRepository repository) : IProductApplication
{
    public async Task<string> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await repository.GetAllAsync(cancellationToken);

        return products.Any() ? "Encontrei" : "Nao tem";
    }

    public async Task<string> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await repository.GetByIdAsync(id, cancellationToken);
        
        return product is not null
            ? $"Encontrei {product.Title}"
            : "Nao tem";
    }
}