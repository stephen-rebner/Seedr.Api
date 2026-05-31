using Seedr.Api.Common.Exceptions;
using Seedr.Api.Features.Environments.Repositories;
using Environment = Seedr.Api.Features.Environments.Models.Environment;

namespace Seedr.Api.Features.Environments.Handlers;

public class DeleteEnvironmentHandler
{
    private readonly IEnvironmentRepository _repository;

    public DeleteEnvironmentHandler(IEnvironmentRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(int id, CancellationToken cancellationToken = default)
    {
        var environment = await _repository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(Environment), id);

        await _repository.DeleteAsync(environment, cancellationToken);
    }
}
