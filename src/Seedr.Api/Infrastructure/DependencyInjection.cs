using Microsoft.EntityFrameworkCore;
using Seedr.Api.Features.Environments.Handlers;
using Seedr.Api.Features.Environments.Repositories;
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

        services.AddEnvironmentsFeature();

        return services;
    }

    private static IServiceCollection AddEnvironmentsFeature(this IServiceCollection services)
    {
        services.AddScoped<IEnvironmentRepository, EnvironmentRepository>();

        services.AddScoped<CreateEnvironmentHandler>();
        services.AddScoped<GetEnvironmentHandler>();
        services.AddScoped<GetAllEnvironmentsHandler>();
        services.AddScoped<UpdateEnvironmentHandler>();
        services.AddScoped<DeleteEnvironmentHandler>();

        return services;
    }
}
