# dotnet-enterprise-travel-platform

Production-inspired .NET backend platform for travel planning and booking management, demonstrating senior backend engineering practices through modular architecture, domain-driven design, booking workflows, idempotent payments, travel document validation, configurable multilingual notifications, outbox processing, structured logging, distributed tracing, automated testing and CI/CD.

## Objective

This repository is being built incrementally as a public portfolio project for Senior Backend .NET engineering. The goal is a pragmatic modular monolith that is easy to understand, easy to run locally, and prepared for future extraction into services where it makes sense.

## Current Phase

**Phase 1 - Trips Domain e API basica**

Implemented:

- Initial repository structure.
- .NET solution file.
- Base source projects for Gateway, modules and building blocks.
- Initial documentation structure under `docs/`.
- ADR 0001 for the modular monolith decision.
- Minimal Gateway API with `/`, `/health` and OpenAPI document at `/openapi/v1.json`.
- Trips domain model with `Trip`, `Traveller`, destination and travel period.
- Trips state transitions for `Draft`, `Planned` and `Cancelled`.
- Trips application service and repository abstraction.
- Trips EF Core infrastructure with PostgreSQL support and an in-memory local fallback.
- Trips endpoints for create, get, list, add traveller, plan, cancel and timeline.
- Trips unit tests for domain rules.

Not implemented yet:

- Authentication and authorization.
- Docker and CI/CD.
- Advanced API quality foundation, including FluentValidation and full ProblemDetails mapping.

## Target Stack

- .NET 10 LTS target framework.
- ASP.NET Core Web API.
- Entity Framework Core and PostgreSQL in later phases.
- Serilog, OpenTelemetry, FluentValidation, JWT, ProblemDetails and Health Checks in later phases.
- xUnit, integration tests, architecture tests and Testcontainers in later phases.
- Docker, GitHub Actions and GitHub Pages in later phases.

## Architecture Direction

The platform follows a modular monolith structure:

- `Gateway.Api` is the public HTTP entry point.
- `Modules/*` contain bounded business capabilities.
- `BuildingBlocks/*` contain reusable cross-cutting abstractions.
- Each module is split into `Api`, `Application`, `Domain` and `Infrastructure` projects where applicable.

The intended dependency direction is:

```text
Api -> Application -> Domain
Infrastructure -> Application/Domain
Gateway.Api -> module Api projects
BuildingBlocks -> no module-specific dependencies
```

## Repository Layout

```text
src/
  AppHost/
  ServiceDefaults/
  Gateway.Api/
  Modules/
    Trips/
    Bookings/
    Payments/
    Documents/
    Notifications/
    Administration/
  BuildingBlocks/
tests/
docs/
  architecture/
  decisions/
  api/
  observability/
  security/
  testing/
  github-pages/
```

## Local Validation

Expected commands:

```powershell
dotnet restore
dotnet build
dotnet test
```

Phase 1 includes focused unit tests for Trips domain rules.

## Running The API

```powershell
dotnet run --project src/Gateway.Api/Gateway.Api.csproj
```

By default the Trips module uses a lightweight in-memory repository so the first functional API can run without PostgreSQL. To use PostgreSQL through EF Core/Npgsql, set:

```json
{
  "Trips": {
    "DatabaseProvider": "PostgreSql"
  },
  "ConnectionStrings": {
    "Trips": "Host=localhost;Port=5432;Database=travel_platform;Username=postgres;Password=postgres"
  }
}
```

## Example Requests

Create a trip:

```http
POST /api/trips
Content-Type: application/json

{
  "title": "Lisbon business trip",
  "ownerUserId": "user-1",
  "destinationCountryCode": "PT",
  "destinationCity": "Lisbon",
  "startsOn": "2026-06-01",
  "endsOn": "2026-06-05"
}
```

Add a traveller:

```http
POST /api/trips/{id}/travellers
Content-Type: application/json

{
  "firstName": "Ana",
  "lastName": "Silva",
  "email": "ana@example.com",
  "birthDate": "1990-01-15"
}
```

Plan a trip:

```http
POST /api/trips/{id}/plan
```

## Roadmap

- Phase 1: Trips Domain and basic API. Done.
- Phase 2: API quality foundation with ProblemDetails, validation, health checks, logging and correlation ID.
- Phase 3: Security with JWT, roles and authorization policies.
- Phase 4: Bookings module.
- Phase 5: Documents module.
- Phase 6: Payments module with idempotency.
- Phase 7: Outbox pattern and internal events.
- Phase 8: Notifications and email.
- Phase 9: Administration API.
- Phase 10: Localization.
- Phase 11: Advanced observability.
- Phase 12: Advanced tests and architecture tests.
- Phase 13: CI/CD.
- Phase 14: GitHub Pages showcase.

## Phase Notes

See:

- [docs/phase-0-bootstrap.md](docs/phase-0-bootstrap.md)
- [docs/phase-1-trips.md](docs/phase-1-trips.md)
