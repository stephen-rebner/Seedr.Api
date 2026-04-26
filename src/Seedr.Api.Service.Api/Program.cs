using Seedr.Api.Application;
using Seedr.Api.Infrastructure;
using Seedr.Api.Infrastructure.Data;
using Seedr.Api.Service.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddProblemDetails();

var app = builder.Build();

// Seed the database.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SeedrDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    await DbInitializer.SeedAsync(dbContext, logger);
}

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

// Map feature endpoints here, e.g.:
// app.MapGroup("/api/v1/example").MapExampleEndpoints();

app.Run();

public partial class Program { }

