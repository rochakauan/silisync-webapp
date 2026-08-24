using domain.silisync.Entities;
using domain.silisync.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace persistence.silisync.Data.Repositories;

public sealed class ProductRepository(
    AppDbContext context,
    ILogger<ProductRepository> logger) : IProductRepository
{
    public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken)
        => ExecuteAsync<IReadOnlyList<Product>>(async () => 
            await context.Products.AsNoTracking().ToListAsync(cancellationToken));

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => ExecuteAsync(() => context.Products.AsNoTracking()
            .FirstOrDefaultAsync(x => x.PublicId == id, cancellationToken));

    public Task<IReadOnlyList<Product>> GetAllDeletedProductsAsync(CancellationToken cancellationToken)
        => ExecuteAsync<IReadOnlyList<Product>>(async () =>
            await context.Products.AsNoTracking().IgnoreQueryFilters().ToListAsync(cancellationToken));

    public Task<Product?> GetDeletedProductAsync(Guid id, CancellationToken cancellationToken)
        => ExecuteAsync(() => 
            context.Products.AsNoTracking().IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.PublicId == id, cancellationToken));

    private async Task<T> ExecuteAsync<T>(Func<Task<T>> operation, string? errorMessage = null)
    {
        try { return await operation(); }
        
        catch (Exception ex) when (ex.InnerException is SqlException)
        {
            logger.LogError(ex, "{error}", errorMessage ?? "Native database server error.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{error}", errorMessage ?? "An unhandled exception occurred during the execution of a query.");
            throw;
        }
    }
}