using domain.silisync.Abstractions;
using domain.silisync.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using persistence.silisync.Identity;

namespace persistence.silisync.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) 
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public new DbSet<User> Users => Set<User>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    { 
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        ApplyDeletedQueryFilters(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }
    
    public override int SaveChanges()
    {
        ApplyTimestamps();
        ApplySafeDelete();
        return base.SaveChanges();
    }
    
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyTimestamps();
        ApplySafeDelete();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyTimestamps();
        ApplySafeDelete();
        return await base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        ApplyTimestamps();
        ApplySafeDelete();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    
    private void ApplyTimestamps()
    {
        var utcNow = DateTimeOffset.UtcNow;

        foreach (var entry in ChangeTracker.Entries<Entity>())
        {
            if (entry.State is EntityState.Added)
                entry.Property(entity => entity.CreatedAt).CurrentValue = utcNow;
            
            else if (entry.State is EntityState.Modified)
                entry.Property(entity => entity.UpdatedAt).CurrentValue = utcNow;
        }
    }

    private void ApplySafeDelete()
    {
        foreach (var entry in ChangeTracker.Entries<Entity>())
        {
            if (entry.State is not EntityState.Deleted) continue;
            entry.State = EntityState.Modified;
            entry.Entity.IsDeleted = true;
        }
    }

    private void ApplyDeletedQueryFilters(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(Entity).IsAssignableFrom(entityType.ClrType)) continue;
            if (entityType.ClrType == typeof(Entity)) continue;

            var parameter = Expression.Parameter(entityType.ClrType, "x");
            var property = Expression.Property(parameter, nameof(Entity.IsDeleted));
            var notDeleted = Expression.Not(property);
            var lambda = Expression.Lambda(notDeleted, parameter);
            
            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }
    }
}