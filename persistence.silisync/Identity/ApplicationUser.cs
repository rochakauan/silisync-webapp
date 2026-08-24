using Microsoft.AspNetCore.Identity;

namespace persistence.silisync.Identity;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public List<IdentityRole<Guid>>? Roles { get; set; }
}