using domain.silisync.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace persistence.silisync.Data.Mappings;

public class CategoryMap : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder.HasKey(x => x.PrivateId);
        builder.Property(x => x.PrivateId)
            .HasColumnName("private_id")
            .ValueGeneratedOnAdd();
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("name")
            .HasMaxLength(200);
        
        builder.Property(x => x.ParentCategoryId)
            .HasColumnName("parent_category_id")
            .HasMaxLength(100);

        builder.Property(x => x.MeliCategoryId)
            .HasColumnName("meli_category_id")
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