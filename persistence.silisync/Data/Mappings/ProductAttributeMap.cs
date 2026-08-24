using domain.silisync.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace persistence.silisync.Data.Mappings;

public class ProductAttributeMap : IEntityTypeConfiguration<ProductAttribute>
{
    public void Configure(EntityTypeBuilder<ProductAttribute> builder)
    {
        builder.ToTable("product_attributes");

        builder.HasKey(x => x.PrivateId);
        builder.Property(x => x.PrivateId)
            .HasColumnName("private_id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("name")
            .HasMaxLength(100);

        builder.Property(x => x.Value)
            .IsRequired()
            .HasColumnName("value")
            .HasMaxLength(100);

        builder.Property(x => x.MeliAttributeId)
            .HasColumnName("meli_attribute_id")
            .HasMaxLength(100);

        builder.Property(x => x.MeliValueId)
            .HasColumnName("meli_value_id")
            .HasMaxLength(100);
        
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
    }
}