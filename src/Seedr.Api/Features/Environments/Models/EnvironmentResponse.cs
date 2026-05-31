namespace Seedr.Api.Features.Environments.Models;

public record EnvironmentResponse(
    int Id,
    string Name,
    string? Description,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);
