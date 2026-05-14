# dotnet-enterprise-travel-platform

Production-inspired .NET backend platform for travel planning and booking management, demonstrating senior backend engineering practices through modular architecture, domain-driven design, booking workflows, idempotent payments, travel document validation, configurable multilingual notifications, outbox processing, structured logging, distributed tracing, automated testing and CI/CD.

## Objective

This repository is being built incrementally as a public portfolio project for Senior Backend .NET engineering. The goal is a pragmatic modular monolith that is easy to understand, easy to run locally, and prepared for future extraction into services where it makes sense.

## Current Phase

**Phase 0 - Bootstrap da solucao**

Implemented:

- Initial repository structure.
- .NET solution file.
- Base source projects for Gateway, modules and building blocks.
- Initial documentation structure under `docs/`.
- ADR 0001 for the modular monolith decision.
- Minimal Gateway API placeholder with `/` and `/health`.

Not implemented yet:

- Business functionality.
- Persistence.
- Authentication and authorization.
- Tests.
- Docker and CI/CD.

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

Phase 0 has no test projects yet, so `dotnet test` is expected to report no tests once the SDK is available.

## Roadmap

- Phase 1: Trips Domain and basic API.
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

See [docs/phase-0-bootstrap.md](docs/phase-0-bootstrap.md).
