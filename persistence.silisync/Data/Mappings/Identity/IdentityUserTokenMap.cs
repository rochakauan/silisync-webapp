using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace persistence.silisync.Data.Mappings.Identity;

public class IdentityUserTokenMap : IEntityTypeConfiguration<IdentityUserToken<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<Guid>> builder)
    {
        builder.ToTable("IdentityUserToken");
        builder.HasKey(x => new { x.UserId, x.LoginProvider, x.Name });
        builder.Property(x => x.LoginProvider).HasMaxLength(120);
        builder.Property(x => x.Name).HasMaxLength(180);
    }
}