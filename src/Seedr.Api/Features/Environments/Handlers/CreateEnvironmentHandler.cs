using Seedr.Api.Common.Interfaces;
using Seedr.Api.Features.Environments.Models;
using Seedr.Api.Features.Environments.Repositories;
using Environment = Seedr.Api.Features.Environments.Models.Environment;

namespace Seedr.Api.Features.Environments.Handlers;

public class CreateEnvironmentHandler : IHandler<CreateEnvironmentRequest, EnvironmentResponse>
{
    private readonly IEnvironmentRepository _repository;

    public CreateEnvironmentHandler(IEnvironmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<EnvironmentResponse> HandleAsync(
        CreateEnvironmentRequest request,
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        var environment = new Environment
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        await _repository.AddAsync(environment, cancellationToken);

        return environment.ToResponse();
    }
}
