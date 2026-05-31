using Microsoft.EntityFrameworkCore;
using Environment = Seedr.Api.Features.Environments.Models.Environment;

namespace Seedr.Api.Infrastructure.Data;

public class SeedrDbContext : DbContext
{
    public SeedrDbContext(DbContextOptions<SeedrDbContext> options) : base(options)
    {
    }

    public DbSet<Environment> Environments => Set<Environment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SeedrDbContext).Assembly);
    }
}
