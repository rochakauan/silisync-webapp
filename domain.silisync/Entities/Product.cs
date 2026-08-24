using domain.silisync.Abstractions;
using domain.silisync.Enums;
using domain.silisync.Exceptions;

namespace domain.silisync.Entities;

public sealed class Product : Entity
{
    public string? MeliId { get; private set; }
    public ESyncStatus SyncStatus { get; private set; } = ESyncStatus.NotSynced;
    
    public string Title { get; init; } = string.Empty;
    public string Sku { get; init; } = string.Empty;
    public string PartNumber { get; init; } = string.Empty;
    public string? Barcode { get; init; }
    public string Description { get; init; } = string.Empty;
    
    public decimal CostPrice { get; init; }
    public decimal SalePrice { get; init; }
    
    public int InStock { get; private set; }
    public EProductCondition Condition { get; init; }
    
    public int Weight { get; init; }
    public int Height { get; init; }
    public int Width { get; init; }
    public int Length { get; init; }
    
    public int CategoryId { get; init; }
    public Category Category { get; init; } = null!;
    
    public string Brand { get; init; } = string.Empty;
    public string? Voltage { get; init; }

    public int WarrantyTime { get; init; }
    public EWarrantyType WarrantyType { get; init; } = EWarrantyType.None;
    
    public ICollection<ProductAttribute> Attributes { get; init; } = new List<ProductAttribute>();

    public void SetMercadoLibreId(string meliId)
    {
        MeliId = meliId;
        SyncStatus = ESyncStatus.Synced;
    }
    
    public void DebitStock(int quantity)
    {
        if (quantity < 0)
            throw new DebitStockOutOfRangeException("The quantity cannot be negative.");
        
        if (quantity > InStock)
            throw new DebitStockOutOfRangeException("The amount to be debited cannot exceed the stock.");
        
        InStock -= quantity;
    }
}