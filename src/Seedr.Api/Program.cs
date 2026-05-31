using FluentValidation;
using Seedr.Api.Common.Middleware;
using Seedr.Api.Infrastructure;
using Seedr.Api.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddProblemDetails();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SeedrDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    await DbInitializer.SeedAsync(dbContext, logger);
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

// Map feature endpoints here, e.g.:
// app.MapGroup("/api/v1/environments").MapEnvironmentEndpoints();

app.Run();

public partial class Program { }
