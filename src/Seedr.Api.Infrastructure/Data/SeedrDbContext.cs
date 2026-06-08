using Microsoft.EntityFrameworkCore;

namespace Seedr.Api.Infrastructure.Data;

public class SeedrDbContext : DbContext
{
    public SeedrDbContext(DbContextOptions<SeedrDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SeedrDbContext).Assembly);
    }
}
