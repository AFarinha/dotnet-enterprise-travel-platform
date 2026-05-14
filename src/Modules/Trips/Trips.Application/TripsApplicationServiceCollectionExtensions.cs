using Microsoft.Extensions.DependencyInjection;

namespace Trips.Application;

public static class TripsApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddTripsApplication(this IServiceCollection services)
    {
        services.AddScoped<ITripService, TripService>();
        return services;
    }
}
