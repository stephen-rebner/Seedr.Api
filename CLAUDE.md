# Seedr.Api

## Project overview

ASP.NET Core 10 REST API using vertical slice architecture. All application code lives in a single project (`src/Seedr.Api`). Features are self-contained slices; shared infrastructure and cross-cutting concerns live in well-known top-level folders.

## Architecture

**Vertical slice** — each feature owns everything it needs end-to-end.

```
src/Seedr.Api/
├── Features/
│   └── <FeatureName>/
│       ├── Endpoints/      Minimal API route definitions
│       ├── Handlers/       Request handlers (commands and queries)
│       ├── Models/         Domain entity + request/response DTOs
│       ├── Repositories/   Feature-specific repository interface + EF implementation
│       └── Validators/     FluentValidation validators
├── Infrastructure/
│   ├── Data/
│   │   ├── Configurations/ EF Core IEntityTypeConfiguration<T> classes
│   │   └── SeedrDbContext.cs
│   ├── DbInitializer.cs    Runs migrations on startup; add seed data here
│   └── DependencyInjection.cs  AddInfrastructure() extension used in Program.cs
├── Common/
│   ├── Exceptions/         DomainException, NotFoundException
│   ├── Interfaces/         IHandler<TRequest, TResponse>
│   └── Middleware/         ExceptionHandlingMiddleware (maps exceptions to ProblemDetails)
└── Program.cs

tests/Seedr.Api.Tests/
└── Features/               Unit and integration tests mirroring the feature structure
```

## Key conventions

- **Endpoints** register routes via an extension method (e.g. `MapEnvironmentEndpoints()`) called from `Program.cs`.
- **Validators** are auto-discovered via `AddValidatorsFromAssembly(typeof(Program).Assembly)` — no manual registration needed.
- **EF configurations** in `Infrastructure/Data/Configurations/` are auto-applied via `ApplyConfigurationsFromAssembly` in `SeedrDbContext`.
- **No generic repository** — each feature repository is purpose-built and injected directly.
- **Exception handling** is centralised in `ExceptionHandlingMiddleware`; throw `DomainException` or `NotFoundException` from anywhere and the middleware maps them to the correct HTTP status.

## Database

PostgreSQL via Npgsql. Connection string key: `DefaultConnection`.

Local default (matches `docker-compose.yml`):
```
Host=localhost;Port=5432;Database=seedr;Username=seedr;Password=seedr
```

Migrations run automatically on startup via `DbInitializer.SeedAsync`.

## Running locally

```bash
docker-compose up -d        # starts PostgreSQL
dotnet run --project src/Seedr.Api
```

## Testing

```bash
dotnet test tests/Seedr.Api.Tests
```

Integration tests use Testcontainers (spins up a real PostgreSQL container automatically).

## Tech stack

| Concern | Library |
|---|---|
| Framework | ASP.NET Core 10 Minimal APIs |
| ORM | Entity Framework Core 10 + Npgsql |
| Validation | FluentValidation 12 |
| Testing | NUnit 4 + Testcontainers.PostgreSql |

## Progress log

### Environments feature (branch `claude/environments-feature-endpoints-uRK1H`)

First implemented vertical slice — use it as the reference pattern for new features.

- **Status:** complete and pushed; not yet merged. Built/tested only by review — the dev sandbox has no .NET SDK, so `dotnet build`/`dotnet test` have not been run.
- **Endpoints** under `/api/v1/environments`: get all, get by id, create (201), update, delete (204). Registered via `MapEnvironmentEndpoints()` in `Program.cs`.
- **Model:** `Environment` with `Name` (required, max 200) and `Description` (optional/nullable, max 1000), plus `CreatedAtUtc`/`UpdatedAtUtc`.
- **Id:** auto-incrementing `int` (PostgreSQL `IDENTITY` column), not a Guid. Note: the entity type is named `Environment`, so consuming files use a `using Environment = Seedr.Api.Features.Environments.Models.Environment;` alias to avoid clashing with `System.Environment`.
- **Repository + handlers** registered in `Infrastructure/DependencyInjection.cs` via `AddEnvironmentsFeature()`. Validation runs in the create/update endpoints (FluentValidation → 400 `ValidationProblem`); missing records throw `NotFoundException` → 404.
- **Migration:** initial `AddEnvironments` migration was hand-written (no SDK to scaffold). Regenerate with `dotnet ef migrations add` once an SDK is available to guarantee the model snapshot matches EF's output.
- **Tests:** validator unit tests + Testcontainers-backed endpoint integration tests in `tests/Seedr.Api.Tests/Features/Environments`.
- **README** was updated from the old clean-architecture layout to the current vertical-slice structure.
