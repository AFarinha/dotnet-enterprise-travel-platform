using System.Text.Json.Serialization;
using Trips.Api;
using Trips.Application;
using Trips.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddTripsApplication();
builder.Services.AddTripsInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapOpenApi();

app.MapGet("/", () => Results.Ok(new
{
    name = "dotnet-enterprise-travel-platform",
    phase = "Phase 1 - Trips Domain and basic API",
    status = "Trips API available"
}));

app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));

app.MapTripsEndpoints();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<ITripsDbInitializer>();
    await initializer.InitializeAsync(CancellationToken.None);
}

app.Run();
