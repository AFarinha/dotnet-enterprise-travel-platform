# Phase 0 - Bootstrap

## Objective

Create the repository foundation and solution structure without implementing business functionality.

## Created Or Changed

- `dotnet-enterprise-travel-platform.sln`
- `Directory.Build.props`
- `Directory.Packages.props`
- `.gitignore`
- `src/Gateway.Api`
- `src/AppHost`
- `src/ServiceDefaults`
- `src/Modules/*`
- `src/BuildingBlocks/*`
- `docs/*`

## Technical Decisions

- Target framework is `net10.0`, matching the requested preference for .NET 10 LTS.
- Aspire-specific packages are not referenced in Phase 0 to avoid adding external dependencies before orchestration is implemented.
- Module projects are intentionally minimal and compile-only. Business behavior starts in Phase 1.
- The `Gateway.Api` project is the only HTTP host for now.

## Completion Criteria

- Repository structure created.
- Base projects created.
- README created.
- Initial documentation created.
- ADR 0001 created.
- `dotnet build` should pass when the .NET 10 SDK is available.

## Validation

Run:

```powershell
dotnet restore
dotnet build
dotnet test
```

Validated in this workspace with .NET SDK `10.0.300`.

## Next Phase

Phase 1 should implement the Trips domain and a minimal functional Trips API with focused domain tests.
