using System.Collections.Concurrent;
using Trips.Application;
using Trips.Application.Abstractions;
using Trips.Domain;

namespace Trips.Infrastructure;

internal sealed class InMemoryTripsStore
{
    public ConcurrentDictionary<Guid, Trip> Trips { get; } = [];
}

internal sealed class InMemoryTripsRepository(InMemoryTripsStore store) : ITripRepository
{
    public Task AddAsync(Trip trip, CancellationToken cancellationToken)
    {
        store.Trips.TryAdd(trip.Id, trip);
        return Task.CompletedTask;
    }

    public Task<Trip?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        store.Trips.TryGetValue(id, out var trip);
        return Task.FromResult(trip);
    }

    public Task<IReadOnlyCollection<Trip>> ListAsync(TripsQuery query, CancellationToken cancellationToken)
    {
        IEnumerable<Trip> trips = store.Trips.Values;

        if (query.Status is not null)
        {
            trips = trips.Where(trip => trip.Status == query.Status);
        }

        if (!string.IsNullOrWhiteSpace(query.DestinationCountry))
        {
            var country = query.DestinationCountry.Trim().ToUpperInvariant();
            trips = trips.Where(trip => trip.Destination?.CountryCode == country);
        }

        trips = (query.SortBy.ToLowerInvariant(), query.SortDirection.ToLowerInvariant()) switch
        {
            ("title", "asc") => trips.OrderBy(trip => trip.Title),
            ("title", _) => trips.OrderByDescending(trip => trip.Title),
            ("createdat", "asc") => trips.OrderBy(trip => trip.CreatedAt),
            _ => trips.OrderByDescending(trip => trip.CreatedAt)
        };

        return Task.FromResult<IReadOnlyCollection<Trip>>(trips
            .Skip((query.SafePage - 1) * query.SafePageSize)
            .Take(query.SafePageSize)
            .ToArray());
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

internal sealed class NoOpTripsDbInitializer : ITripsDbInitializer
{
    public Task InitializeAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
