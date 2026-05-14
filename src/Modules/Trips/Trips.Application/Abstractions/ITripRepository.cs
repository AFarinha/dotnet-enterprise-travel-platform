using Trips.Domain;

namespace Trips.Application.Abstractions;

public interface ITripRepository
{
    Task AddAsync(Trip trip, CancellationToken cancellationToken);

    Task<Trip?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Trip>> ListAsync(TripsQuery query, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
