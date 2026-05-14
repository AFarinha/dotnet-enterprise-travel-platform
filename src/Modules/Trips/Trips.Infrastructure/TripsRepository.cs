using Microsoft.EntityFrameworkCore;
using Trips.Application;
using Trips.Application.Abstractions;
using Trips.Domain;

namespace Trips.Infrastructure;

internal sealed class TripsRepository(TripsDbContext dbContext) : ITripRepository
{
    public async Task AddAsync(Trip trip, CancellationToken cancellationToken)
        => await dbContext.Trips.AddAsync(trip, cancellationToken);

    public async Task<Trip?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await dbContext.Trips
            .Include(trip => trip.Travellers)
            .Include(trip => trip.Timeline)
            .FirstOrDefaultAsync(trip => trip.Id == id, cancellationToken);

    public async Task<IReadOnlyCollection<Trip>> ListAsync(TripsQuery query, CancellationToken cancellationToken)
    {
        var trips = dbContext.Trips
            .Include(trip => trip.Travellers)
            .AsQueryable();

        if (query.Status is not null)
        {
            trips = trips.Where(trip => trip.Status == query.Status);
        }

        if (!string.IsNullOrWhiteSpace(query.DestinationCountry))
        {
            var country = query.DestinationCountry.Trim().ToUpperInvariant();
            trips = trips.Where(trip => trip.Destination != null && trip.Destination.CountryCode == country);
        }

        trips = (query.SortBy.ToLowerInvariant(), query.SortDirection.ToLowerInvariant()) switch
        {
            ("title", "asc") => trips.OrderBy(trip => trip.Title),
            ("title", _) => trips.OrderByDescending(trip => trip.Title),
            ("createdat", "asc") => trips.OrderBy(trip => trip.CreatedAt),
            _ => trips.OrderByDescending(trip => trip.CreatedAt)
        };

        return await trips
            .Skip((query.SafePage - 1) * query.SafePageSize)
            .Take(query.SafePageSize)
            .ToArrayAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => dbContext.SaveChangesAsync(cancellationToken);
}
