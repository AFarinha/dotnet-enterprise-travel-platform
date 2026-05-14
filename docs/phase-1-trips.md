# Phase 1 - Trips Domain e API basica

## Objective

Implement the first functional module with a clean domain model, application use cases, EF Core infrastructure and HTTP endpoints.

## Implemented

- `Trip` aggregate.
- `Traveller` entity.
- `Destination` and `TravelPeriod` value objects.
- `TripStatus` enum.
- Domain rules for:
  - required trip title and owner;
  - valid destination;
  - valid date range;
  - maximum traveller count;
  - minimum traveller count before planning;
  - preventing traveller changes after planning;
  - valid cancellation states.
- Timeline entries for relevant trip changes.
- Application service for Trips use cases.
- Repository abstraction.
- EF Core `TripsDbContext`.
- PostgreSQL-ready configuration using Npgsql.
- In-memory local repository fallback for easy first-run development.
- Initial EF migration class for the Trips schema.
- Minimal API endpoints:
  - `POST /api/trips`
  - `GET /api/trips/{id}`
  - `GET /api/trips`
  - `POST /api/trips/{id}/travellers`
  - `POST /api/trips/{id}/plan`
  - `POST /api/trips/{id}/cancel`
  - `GET /api/trips/{id}/timeline`
- OpenAPI document at `/openapi/v1.json`.
- Unit tests for Trips domain behavior.

## Technical Decisions

- The domain layer has no EF Core or ASP.NET dependencies.
- The application layer coordinates use cases and exposes DTOs.
- The infrastructure layer owns EF Core mapping and persistence.
- The Gateway hosts the module endpoints for now.
- A lightweight in-memory repository is the default provider in Phase 1 so the API is usable without Docker.
- PostgreSQL is supported by configuration and will become the default local dependency when Docker Compose is introduced.

## Validation Commands

```powershell
dotnet restore
dotnet build
dotnet test
```

## What Remains For Phase 2

- FluentValidation.
- Consistent ProblemDetails mapping.
- Health checks beyond the placeholder endpoint.
- Structured logging.
- Correlation ID middleware.
- Improved Swagger/OpenAPI metadata.
- Stronger pagination response metadata.
