using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using persistence.silisync.Identity;

namespace persistence.silisync.Data.Mappings.Identity;

public class IdentityUserMap : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("IdentityUser");
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => x.NormalizedUserName).IsUnique();
        builder.HasIndex(x => x.NormalizedEmail).IsUnique();

        builder.Property(x => x.Email).HasMaxLength(256);
        builder.Property(x => x.NormalizedUserName).HasMaxLength(256);
        builder.Property(x => x.UserName).HasMaxLength(180);
        builder.Property(x => x.NormalizedUserName).HasMaxLength(180);
        builder.Property(x => x.PhoneNumber).HasMaxLength(20);
        builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        
        builder.HasMany<IdentityUserClaim<Guid>>().WithOne().HasForeignKey(x => x.UserId).IsRequired();
        builder.HasMany<IdentityUserLogin<Guid>>().WithOne().HasForeignKey(x => x.UserId).IsRequired();
        builder.HasMany<IdentityUserToken<Guid>>().WithOne().HasForeignKey(x => x.UserId).IsRequired();
        builder.HasMany<IdentityUserRole<Guid>>().WithOne().HasForeignKey(x => x.UserId).IsRequired();
        
    }
}