# DevHabit API

DevHabit is a RESTful habit-tracking API built with ASP.NET Core and PostgreSQL. It provides endpoints for managing habits and tags, assigning tags to habits, and registering users. The project also demonstrates production-oriented API patterns such as content negotiation, media-type API versioning, pagination, filtering, sorting, sparse fieldsets, HATEOAS links, centralized error handling, validation, database migrations, and OpenTelemetry.

## Features

- Create, read, update, partially update, and delete habits
- Create, read, update, and delete tags
- Assign or remove tags from a habit
- Register users with ASP.NET Core Identity
- Search and filter habits by type and status
- Sort results and request only selected response fields
- Paginated collection responses
- Optional HATEOAS links through custom media types
- JSON and XML content negotiation
- Media-type API versioning for habit responses
- RFC 7807-style problem details and centralized exception handling
- PostgreSQL persistence with Entity Framework Core migrations
- Distributed tracing, metrics, and logs with OpenTelemetry
- Docker Compose services for the API, PostgreSQL, Seq, and the Aspire Dashboard

## Tech stack

- .NET 9 / ASP.NET Core Web API
- Entity Framework Core 9
- PostgreSQL 17 and Npgsql
- ASP.NET Core Identity
- FluentValidation
- Newtonsoft.Json and JSON Patch
- Asp.Versioning
- OpenTelemetry
- Docker and Docker Compose

## Project structure

```text
DevHabit/
├── DevHabit.Api/
│   ├── Controllers/       # HTTP endpoints
│   ├── Database/          # DbContexts and entity configurations
│   ├── DTOs/              # Request/response models, validation, and mappings
│   ├── Entities/          # Domain and persistence entities
│   ├── Extensions/        # Database migration helpers
│   ├── Middleware/        # Global exception handlers
│   ├── Migrations/        # Application and Identity migrations
│   └── Services/          # Links, sorting, and data shaping
├── docker-compose.yml
├── Directory.Build.props
├── Directory.Packages.props
└── DevHabit.sln
```

## Getting started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) for the recommended setup

### Run with Docker Compose

From the repository root:

```bash
docker compose up --build
```

The main services are then available at:

| Service | Address |
| --- | --- |
| API (HTTP) | `http://localhost:5000` |
| API (HTTPS) | `https://localhost:5001` |
| PostgreSQL | `localhost:5432` |
| Seq | `http://localhost:8080` |
| Aspire Dashboard | `http://localhost:18888` |

In Development, the API applies both application and Identity migrations automatically at startup.

Stop the services with:

```bash
docker compose down
```

PostgreSQL and Seq data are stored under `.containers/`, which is intentionally excluded from Git.

### Run the API locally

Start PostgreSQL first, then provide a connection string. The committed development configuration uses the Docker Compose hostname, so a host-run API can override it with an environment variable:

```powershell
$env:ConnectionStrings__Database = "Host=localhost;Port=5432;Database=devhabit;Username=postgres;Password=postgres"
dotnet restore
dotnet run --project DevHabit.Api
```

The local launch profile exposes the API at `http://localhost:5000` and `https://localhost:5001`.

## API overview

| Method | Endpoint | Purpose |
| --- | --- | --- |
| `POST` | `/auth/register` | Register an Identity user and application profile |
| `GET` | `/users/{id}` | Get a user by ID |
| `GET` | `/habits` | List, search, filter, sort, shape, and paginate habits |
| `GET` | `/habits/{id}` | Get a habit and its tags |
| `POST` | `/habits` | Create a habit |
| `PUT` | `/habits/{id}` | Replace the editable habit fields |
| `PATCH` | `/habits/{id}` | Partially update a habit using JSON Patch |
| `DELETE` | `/habits/{id}` | Delete a habit |
| `GET` | `/tags` | List tags |
| `GET` | `/tags/{id}` | Get a tag by ID |
| `POST` | `/tags` | Create a tag |
| `PUT` | `/tags/{id}` | Update a tag |
| `DELETE` | `/tags/{id}` | Delete a tag |
| `PUT` | `/habits/{habitId}/tags` | Replace a habit's tag assignments |
| `DELETE` | `/habits/{habitId}/tags/{tagId}` | Remove one tag from a habit |

### Querying habits

`GET /habits` supports these query parameters:

- `page` and `pageSize` for pagination
- `q` for matching habit names and descriptions
- `type` and `status` for filtering
- `sort` for ordering by supported fields
- `fields` for sparse fieldsets/data shaping
- `includeLinks` to add HATEOAS links

Example:

```http
GET /habits?page=1&pageSize=10&q=read&sort=name&fields=id,name,status&includeLinks=true
```

### Content negotiation and versioning

The API supports standard JSON/XML responses and custom vendor media types. Habit API versions are selected through the `Accept` header; version 1 is used when no version is specified.

```http
Accept: application/json;v=1
Accept: application/json;v=2
Accept: application/vnd.dev-habit.hateoas.1+json
Accept: application/vnd.dev-habit.hateoas.2+json
```

Use `application/json-patch+json` for `PATCH /habits/{id}` requests.

## Database migrations

Migrations are separated by context and schema:

- `ApplicationDbContext` stores habits, tags, relationships, and application user profiles.
- `ApplicationIdentityDbContext` stores ASP.NET Core Identity data.

To create a new migration, specify the context and output directory:

```bash
dotnet ef migrations add <MigrationName> --project DevHabit.Api --context ApplicationDbContext --output-dir Migrations/Application
dotnet ef migrations add <MigrationName> --project DevHabit.Api --context ApplicationIdentityDbContext --output-dir Migrations/Identity
```

## Build

```bash
dotnet restore
dotnet build DevHabit.sln
```

The solution enables nullable reference types, current .NET analyzers, enforced code style, and warnings-as-errors.

## Current scope

User registration and Identity persistence are implemented. Login/token issuance and authorization policies are not yet included, so the resource endpoints are currently unauthenticated.

## License

No license has been selected yet. Add a `LICENSE` file before distributing or accepting external contributions.
