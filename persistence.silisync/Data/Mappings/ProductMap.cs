using domain.silisync.Entities;
using domain.silisync.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace persistence.silisync.Data.Mappings;

public class ProductMap : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        
        builder.HasKey(x => x.PrivateId);
        builder.Property(x => x.PrivateId)
            .HasColumnName("private_id")
            .ValueGeneratedOnAdd();
        
        builder.Property(x => x.MeliId)
            .HasColumnName("meli_id")
            .HasMaxLength(200);

        builder.Property(x => x.SyncStatus)
            .IsRequired()
            .HasColumnName("sync_status")
            .HasConversion<string>()
            .HasMaxLength(30)
            .HasDefaultValue(ESyncStatus.NotSynced);
        
        builder.Property(x => x.Title)
            .IsRequired()
            .HasColumnName("title")
            .HasMaxLength(60);

        builder.Property(x => x.Sku)
            .IsRequired()
            .HasColumnName("sku")
            .HasMaxLength(30);

        builder.Property(x => x.PartNumber)
            .IsRequired()
            .HasColumnName("part_number")
            .HasMaxLength(60);
        
        builder.Property(x => x.Barcode)
            .HasColumnName("barcode")
            .HasMaxLength(60);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasColumnName("description")
            .HasMaxLength(5000);

        builder.Property(x => x.CostPrice)
            .IsRequired()
            .HasColumnName("cost_price")
            .HasPrecision(18, 2);
        
        builder.Property(x => x.SalePrice)
            .IsRequired()
            .HasColumnName("sale_price")
            .HasPrecision(18, 2);

        builder.Property(x => x.InStock)
            .IsRequired()
            .HasColumnName("in_stock");

        builder.Property(x => x.Condition)
            .IsRequired()
            .HasColumnName("condition")
            .HasConversion<string>()
            .HasMaxLength(30)
            .HasDefaultValue(EProductCondition.New);

        builder.Property(x => x.Weight)
            .IsRequired()
            .HasColumnName("weight");
        builder.Property(x => x.Height)
            .IsRequired()
            .HasColumnName("height");
        builder.Property(x => x.Width)
            .IsRequired()
            .HasColumnName("width");
        builder.Property(x => x.Length)
            .IsRequired()
            .HasColumnName("length");
        
        builder.Property(x => x.Brand)
            .IsRequired()
            .HasColumnName("brand")
            .HasMaxLength(20);

        builder.Property(x => x.Voltage)
            .HasColumnName("voltage")
            .HasMaxLength(10);

        builder.Property(x => x.WarrantyTime)
            .IsRequired()
            .HasColumnName("warranty_time");
        
        builder.Property(x => x.WarrantyType)
            .IsRequired()
            .HasColumnName("warranty_type")
            .HasConversion<string>()
            .HasMaxLength(30)
            .HasDefaultValue(EWarrantyType.None);
        
        // Entity
        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at")
            .HasDefaultValueSql("SYSUTCDATETIME()");
        
        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(x => x.IsDeleted)
            .IsRequired()
            .HasColumnName("is_deleted")
            .HasDefaultValue(false);
            
        builder.HasIndex(x => x.PublicId)
            .IsUnique();

        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasMany(x => x.Attributes)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}