using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace persistence.silisync.Data.Mappings.Identity;

public class IdentityUserClaimMap : IEntityTypeConfiguration<IdentityUserClaim<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> builder)
    {
        builder.ToTable("IdentityClaim");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ClaimType).HasMaxLength(255);
        builder.Property(x => x.ClaimValue).HasMaxLength(255);
    }
}