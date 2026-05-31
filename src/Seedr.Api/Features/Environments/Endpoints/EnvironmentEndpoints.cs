using FluentValidation;
using Seedr.Api.Features.Environments.Handlers;
using Seedr.Api.Features.Environments.Models;

namespace Seedr.Api.Features.Environments.Endpoints;

public static class EnvironmentEndpoints
{
    public static RouteGroupBuilder MapEnvironmentEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (GetAllEnvironmentsHandler handler, CancellationToken cancellationToken) =>
            Results.Ok(await handler.HandleAsync(cancellationToken)));

        group.MapGet("/{id:int}", async (int id, GetEnvironmentHandler handler, CancellationToken cancellationToken) =>
            Results.Ok(await handler.HandleAsync(id, cancellationToken)));

        group.MapPost("/", async (
            CreateEnvironmentRequest request,
            IValidator<CreateEnvironmentRequest> validator,
            CreateEnvironmentHandler handler,
            CancellationToken cancellationToken) =>
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var response = await handler.HandleAsync(request, cancellationToken);
            return Results.Created($"/api/v1/environments/{response.Id}", response);
        });

        group.MapPut("/{id:int}", async (
            int id,
            UpdateEnvironmentRequest request,
            IValidator<UpdateEnvironmentRequest> validator,
            UpdateEnvironmentHandler handler,
            CancellationToken cancellationToken) =>
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var response = await handler.HandleAsync(id, request, cancellationToken);
            return Results.Ok(response);
        });

        group.MapDelete("/{id:int}", async (int id, DeleteEnvironmentHandler handler, CancellationToken cancellationToken) =>
        {
            await handler.HandleAsync(id, cancellationToken);
            return Results.NoContent();
        });

        return group;
    }
}
