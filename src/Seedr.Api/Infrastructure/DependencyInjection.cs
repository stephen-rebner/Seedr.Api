using Microsoft.EntityFrameworkCore;
using Seedr.Api.Infrastructure.Data;

namespace Seedr.Api.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

        services.AddDbContext<SeedrDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}
