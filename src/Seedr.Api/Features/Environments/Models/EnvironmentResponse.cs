namespace Seedr.Api.Features.Environments.Models;

public record EnvironmentResponse(
    Guid Id,
    string Name,
    string Description,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);
