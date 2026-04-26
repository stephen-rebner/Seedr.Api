namespace Seedr.Api.Application.Common.Interfaces;

public interface IHandler<TRequest, TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}
