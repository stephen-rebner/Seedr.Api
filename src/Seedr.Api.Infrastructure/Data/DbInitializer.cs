using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Seedr.Api.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(SeedrDbContext dbContext, ILogger logger)
    {
        try
        {
            await dbContext.Database.MigrateAsync();
            logger.LogInformation("Database migrations applied successfully.");

            // Add seed data here — idempotent checks should guard against duplicates.
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
}
