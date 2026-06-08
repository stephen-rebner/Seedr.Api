namespace Seedr.Api.Features.Environments.Models;

public static class EnvironmentMappings
{
    public static EnvironmentResponse ToResponse(this Environment environment) =>
        new(
            environment.Id,
            environment.Name,
            environment.Description,
            environment.CreatedAtUtc,
            environment.UpdatedAtUtc);
}
