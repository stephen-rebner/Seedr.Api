using Seedr.Api.Common.Exceptions;
using Seedr.Api.Common.Interfaces;
using Seedr.Api.Features.Environments.Models;
using Seedr.Api.Features.Environments.Repositories;
using Environment = Seedr.Api.Features.Environments.Models.Environment;

namespace Seedr.Api.Features.Environments.Handlers;

public class GetEnvironmentHandler : IHandler<Guid, EnvironmentResponse>
{
    private readonly IEnvironmentRepository _repository;

    public GetEnvironmentHandler(IEnvironmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<EnvironmentResponse> HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var environment = await _repository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(Environment), id);

        return environment.ToResponse();
    }
}
