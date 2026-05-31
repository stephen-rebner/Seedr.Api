using Microsoft.EntityFrameworkCore;
using Seedr.Api.Infrastructure.Data;
using Environment = Seedr.Api.Features.Environments.Models.Environment;

namespace Seedr.Api.Features.Environments.Repositories;

public class EnvironmentRepository : IEnvironmentRepository
{
    private readonly SeedrDbContext _dbContext;

    public EnvironmentRepository(SeedrDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Environment>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _dbContext.Environments
            .AsNoTracking()
            .OrderBy(e => e.Name)
            .ToListAsync(cancellationToken);

    public async Task<Environment?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _dbContext.Environments
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task AddAsync(Environment environment, CancellationToken cancellationToken = default)
    {
        await _dbContext.Environments.AddAsync(environment, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Environment environment, CancellationToken cancellationToken = default)
    {
        _dbContext.Environments.Update(environment);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Environment environment, CancellationToken cancellationToken = default)
    {
        _dbContext.Environments.Remove(environment);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
