using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PersonalBloggingPlatformAPI.Data;

// Design-time factory so EF tools can create the DbContext when they run outside the app host.
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // Use the same SQL Server instance and database used by the application.
        var connectionString =
            "Data Source=(localdb)\\ProjectModels;Initial Catalog=PersonalBloggingPlatformDb;Integrated Security=True;TrustServerCertificate=True;";

        builder.UseSqlServer(connectionString);

        return new ApplicationDbContext(builder.Options);
    }
}