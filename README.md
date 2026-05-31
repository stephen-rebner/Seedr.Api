# Seedr.Api

The CRUD backbone of the Seedr platform. Manages **environments** (isolated workspaces), **database connections** stored within those environments, and **users**. All endpoints require a valid JWT issued by `entrypoint-auth`.

Built on .NET 10 with ASP.NET Core Minimal APIs, Vertical Slice handlers, Entity Framework Core 10, and PostgreSQL.

## Architecture

**Vertical slice** — all application code lives in a single project (`src/Seedr.Api`). Each feature is a self-contained slice owning everything it needs end-to-end (endpoints, handlers, models, repository, validators). Shared infrastructure and cross-cutting concerns live in well-known top-level folders (`Infrastructure`, `Common`).

```
src/Seedr.Api/
├── Features/<FeatureName>/
│   ├── Endpoints/      Minimal API route definitions
│   ├── Handlers/       Request handlers (commands and queries)
│   ├── Models/         Domain entity + request/response DTOs
│   ├── Repositories/   Feature-specific repository interface + EF implementation
│   └── Validators/     FluentValidation validators
├── Infrastructure/     EF Core DbContext, configurations, migrations, DI wiring
└── Common/             Exceptions, shared interfaces, middleware
```

| Concern | Location | Responsibility |
|---------|----------|---------------|
| Features | `src/Seedr.Api/Features` | Self-contained vertical slices (endpoints, handlers, models, repositories, validators) |
| Infrastructure | `src/Seedr.Api/Infrastructure` | EF Core DbContext, entity configurations, migrations, `AddInfrastructure()` DI wiring |
| Common | `src/Seedr.Api/Common` | Domain exceptions, shared handler interface, exception-handling middleware |

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker](https://www.docker.com/) and Docker Compose

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/stephen-rebner/Seedr.Api.git
cd Seedr.Api
```

### 2. Run with Docker Compose

```bash
docker-compose up --build
```

This starts:
- **API** at `http://localhost:8080`
- **PostgreSQL** at `localhost:5432`

### 3. Run locally (without Docker)

Start a PostgreSQL instance (or update the connection string), then:

```bash
dotnet run --project src/Seedr.Api
```

The connection string is configured in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=seedr;Username=seedr;Password=seedr"
  }
}
```

### 4. Apply database migrations

Migrations are applied automatically on startup via `DbInitializer.SeedAsync()`.

To add a new migration manually:

```bash
dotnet ef migrations add <MigrationName> \
  --project src/Seedr.Api \
  --output-dir Infrastructure/Data/Migrations
```

## Solution Structure

```
Seedr.Api/
├── src/
│   └── Seedr.Api/
│       ├── Features/
│       │   └── Environments/
│       │       ├── Endpoints/
│       │       ├── Handlers/
│       │       ├── Models/
│       │       ├── Repositories/
│       │       └── Validators/
│       ├── Infrastructure/
│       │   ├── Data/
│       │   │   ├── Configurations/
│       │   │   ├── Migrations/
│       │   │   └── SeedrDbContext.cs
│       │   ├── DbInitializer.cs
│       │   └── DependencyInjection.cs
│       ├── Common/
│       │   ├── Exceptions/
│       │   ├── Interfaces/
│       │   └── Middleware/
│       └── Program.cs
├── tests/
│   └── Seedr.Api.Tests/
│       └── Features/            # Unit and integration tests mirroring the feature structure
├── Dockerfile
├── docker-compose.yml
└── Seedr.Api.sln
```

## Running Tests

```bash
# Run all tests
dotnet test

# Run the test project (requires Docker for Testcontainers integration tests)
dotnet test tests/Seedr.Api.Tests
```

## Tech Stack

- **.NET 10** / **C# 14**
- **ASP.NET Core Minimal APIs**
- **Entity Framework Core 10** with PostgreSQL (Npgsql)
- **FluentValidation 12**
- **NUnit 4**
- **Testcontainers.PostgreSql** for integration tests
- **WebApplicationFactory\<Program\>** for API integration tests
