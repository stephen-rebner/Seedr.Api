using Seedr.Api.Features.Environments.Models;
using Seedr.Api.Features.Environments.Repositories;

namespace Seedr.Api.Features.Environments.Handlers;

public class GetAllEnvironmentsHandler
{
    private readonly IEnvironmentRepository _repository;

    public GetAllEnvironmentsHandler(IEnvironmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<EnvironmentResponse>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var environments = await _repository.GetAllAsync(cancellationToken);

        return environments.Select(e => e.ToResponse()).ToList();
    }
}
