using Environment = Seedr.Api.Features.Environments.Models.Environment;

namespace Seedr.Api.Features.Environments.Repositories;

public interface IEnvironmentRepository
{
    Task<IReadOnlyList<Environment>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Environment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task AddAsync(Environment environment, CancellationToken cancellationToken = default);

    Task UpdateAsync(Environment environment, CancellationToken cancellationToken = default);

    Task DeleteAsync(Environment environment, CancellationToken cancellationToken = default);
}
