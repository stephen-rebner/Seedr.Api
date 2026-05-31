using Seedr.Api.Common.Exceptions;
using Seedr.Api.Features.Environments.Models;
using Seedr.Api.Features.Environments.Repositories;
using Environment = Seedr.Api.Features.Environments.Models.Environment;

namespace Seedr.Api.Features.Environments.Handlers;

public class UpdateEnvironmentHandler
{
    private readonly IEnvironmentRepository _repository;

    public UpdateEnvironmentHandler(IEnvironmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<EnvironmentResponse> HandleAsync(
        Guid id,
        UpdateEnvironmentRequest request,
        CancellationToken cancellationToken = default)
    {
        var environment = await _repository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(Environment), id);

        environment.Name = request.Name;
        environment.Description = request.Description;
        environment.UpdatedAtUtc = DateTime.UtcNow;

        await _repository.UpdateAsync(environment, cancellationToken);

        return environment.ToResponse();
    }
}
