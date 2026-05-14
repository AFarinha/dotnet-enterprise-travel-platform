# ADR 0001: Use Modular Monolith First

## Status

Accepted

## Context

The platform needs to demonstrate enterprise backend practices without creating unnecessary operational complexity too early. The domain includes trips, bookings, payments, documents, notifications and administration. These areas benefit from clear boundaries, but they do not require separate deployable services at the start.

## Decision

Build the system as a modular monolith first. Each business capability is represented as a module with explicit project boundaries. The default split is:

- `Api`
- `Application`
- `Domain`
- `Infrastructure`

Cross-cutting concerns live in `BuildingBlocks`.

## Consequences

Positive:

- Lower local development complexity.
- Easier refactoring while the domain model is still evolving.
- Clear boundaries that can later be reinforced by architecture tests.
- Better portfolio readability for reviewers.

Trade-offs:

- Runtime isolation is lower than in a microservice architecture.
- Module discipline must be enforced through code review and tests.
- Extraction to services later will require explicit contract and data ownership work.
