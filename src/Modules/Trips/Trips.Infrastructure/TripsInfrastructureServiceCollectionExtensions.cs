using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trips.Application.Abstractions;

namespace Trips.Infrastructure;

public static class TripsInfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddTripsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var provider = configuration["Trips:DatabaseProvider"] ?? "InMemory";

        if (!provider.Equals("PostgreSql", StringComparison.OrdinalIgnoreCase))
        {
            services.AddSingleton<InMemoryTripsStore>();
            services.AddScoped<ITripRepository, InMemoryTripsRepository>();
            services.AddScoped<ITripsDbInitializer, NoOpTripsDbInitializer>();
            return services;
        }

        services.AddDbContext<TripsDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("Trips")
                ?? "Host=localhost;Port=5432;Database=travel_platform;Username=postgres;Password=postgres";

            options.UseNpgsql(connectionString, npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "trips"));
        });

        services.AddScoped<ITripRepository, TripsRepository>();
        services.AddScoped<ITripsDbInitializer, TripsDbInitializer>();

        return services;
    }
}
