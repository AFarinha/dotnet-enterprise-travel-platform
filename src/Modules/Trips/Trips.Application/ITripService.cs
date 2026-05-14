namespace Trips.Application;

public interface ITripService
{
    Task<TripDto> CreateAsync(CreateTripCommand command, CancellationToken cancellationToken);

    Task<TripDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<TripDto>> ListAsync(TripsQuery query, CancellationToken cancellationToken);

    Task<TravellerDto> AddTravellerAsync(AddTravellerCommand command, CancellationToken cancellationToken);

    Task<TripDto> PlanAsync(PlanTripCommand command, CancellationToken cancellationToken);

    Task<TripDto> CancelAsync(CancelTripCommand command, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<TripTimelineEntryDto>> GetTimelineAsync(Guid id, CancellationToken cancellationToken);
}
