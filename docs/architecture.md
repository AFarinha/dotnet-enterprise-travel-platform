# Architecture

This platform is designed as a pragmatic modular monolith. The first production-style boundary is the module, not the process.

The intended dependency direction is:

```text
Gateway.Api -> Module Api -> Application -> Domain
Infrastructure -> Application/Domain
BuildingBlocks -> shared abstractions only
```

Future phases will add diagrams, module contracts, persistence boundaries and architecture tests.
