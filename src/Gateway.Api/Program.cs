var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    name = "dotnet-enterprise-travel-platform",
    phase = "Phase 0 - Bootstrap",
    status = "Bootstrapped"
}));

app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));

app.Run();
