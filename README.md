# Seedr.Api

The CRUD backbone of the Seedr platform. Manages **environments** (isolated workspaces), **database connections** stored within those environments, and **users**. All endpoints require a valid JWT issued by `entrypoint-auth`.

Built on .NET 10 with ASP.NET Core Minimal APIs, Vertical Slice handlers, Entity Framework Core 10, and PostgreSQL.

## Architecture

```
Core ← Application ← Infrastructure ← Service.Api
```

| Layer | Project | Responsibility |
|-------|---------|---------------|
| Domain | `Seedr.Api.Core` | Domain models, repository interfaces, domain exceptions |
| Application | `Seedr.Api.Application` | Vertical slice handlers, validators, DTOs |
| Infrastructure | `Seedr.Api.Infrastructure` | EF Core DbContext, repository implementations, migrations |
| API | `Seedr.Api.Service.Api` | Minimal API endpoints, middleware, DI wiring |

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
cd src/Seedr.Api.Service.Api
dotnet run
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
  --project src/Seedr.Api.Infrastructure \
  --startup-project src/Seedr.Api.Service.Api
```

## Solution Structure

```
Seedr.Api/
├── src/
│   ├── Seedr.Api.Core/
│   │   ├── Domain/
│   │   │   ├── Exceptions/
│   │   │   └── Models/
│   │   └── Repositories/
│   ├── Seedr.Api.Application/
│   │   ├── Common/Interfaces/
│   │   ├── DTOs/
│   │   ├── Options/
│   │   └── Services/
│   ├── Seedr.Api.Infrastructure/
│   │   ├── Configurations/EntityConfigurations/
│   │   ├── Data/
│   │   │   ├── SeedrDbContext.cs
│   │   │   └── DbInitializer.cs
│   │   └── Repositories/
│   └── Seedr.Api.Service.Api/
│       ├── Endpoints/
│       ├── Middleware/
│       │   └── ExceptionHandlingMiddleware.cs
│       └── Program.cs
├── tests/
│   ├── Seedr.Api.Core.Tests/
│   ├── Seedr.Api.Application.Tests/
│   ├── Seedr.Api.Infrastructure.Tests/
│   ├── Seedr.Api.Infrastructure.IntegrationTests/
│   ├── Seedr.Api.Service.Api.Tests/
│   └── Seedr.Api.Service.Api.IntegrationTests/
├── Dockerfile
├── docker-compose.yml
└── Seedr.Api.sln
```

## Running Tests

```bash
# Run all tests
dotnet test

# Run a specific project
dotnet test tests/Seedr.Api.Core.Tests

# Run integration tests (requires Docker for Testcontainers)
dotnet test tests/Seedr.Api.Infrastructure.IntegrationTests
dotnet test tests/Seedr.Api.Service.Api.IntegrationTests
```

## Tech Stack

- **.NET 10** / **C# 14**
- **ASP.NET Core Minimal APIs**
- **Entity Framework Core 10** with PostgreSQL (Npgsql)
- **FluentValidation 12**
- **NUnit 4**
- **Testcontainers.PostgreSql** for integration tests
- **WebApplicationFactory\<Program\>** for API integration tests
