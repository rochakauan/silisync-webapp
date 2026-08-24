using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace persistence.silisync.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=silisync;User ID=sa;Password=1q2w3e4r@#$;TrustServerCertificate=true");
        
        return new AppDbContext(optionsBuilder.Options);
    }
}