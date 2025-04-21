using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace RentIt.Infrastructure.Persistence.Setup;

public static class MigrationManager
{
    public static void ApplyMigrations(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<MigrationLogger>>();

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            logger.LogInformation("Applying database migrations...");
            db.Database.Migrate();
            logger.LogInformation("Database up-to-date.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error applying migrations.");
            throw;
        }
    }
}

public class MigrationLogger { }

