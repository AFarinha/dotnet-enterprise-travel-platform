using Trips.Application.Abstractions;
using Trips.Domain;

namespace Trips.Application;

internal sealed class TripService(ITripRepository repository) : ITripService
{
    public async Task<TripDto> CreateAsync(CreateTripCommand command, CancellationToken cancellationToken)
    {
        var trip = Trip.Create(command.Title, command.OwnerUserId);

        if (command.DestinationCountryCode is not null || command.DestinationCity is not null || command.StartsOn.HasValue || command.EndsOn.HasValue)
        {
            if (command.StartsOn is null || command.EndsOn is null)
            {
                throw new TripDomainException("Both start and end dates are required when defining trip dates.");
            }

            trip.UpdateDraftDetails(
                command.Title,
                Destination.Create(command.DestinationCountryCode ?? string.Empty, command.DestinationCity ?? string.Empty),
                TravelPeriod.Create(command.StartsOn.Value, command.EndsOn.Value));
        }

        await repository.AddAsync(trip, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return TripDto.FromDomain(trip);
    }

    public async Task<TripDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => TripDto.FromDomain(await GetRequiredTripAsync(id, cancellationToken));

    public async Task<IReadOnlyCollection<TripDto>> ListAsync(TripsQuery query, CancellationToken cancellationToken)
    {
        var trips = await repository.ListAsync(query, cancellationToken);
        return trips.Select(TripDto.FromDomain).ToArray();
    }

    public async Task<TravellerDto> AddTravellerAsync(AddTravellerCommand command, CancellationToken cancellationToken)
    {
        var trip = await GetRequiredTripAsync(command.TripId, cancellationToken);
        var traveller = trip.AddTraveller(command.FirstName, command.LastName, command.Email, command.BirthDate);

        await repository.SaveChangesAsync(cancellationToken);

        return TravellerDto.FromDomain(traveller);
    }

    public async Task<TripDto> PlanAsync(PlanTripCommand command, CancellationToken cancellationToken)
    {
        var trip = await GetRequiredTripAsync(command.TripId, cancellationToken);
        trip.Plan();

        await repository.SaveChangesAsync(cancellationToken);

        return TripDto.FromDomain(trip);
    }

    public async Task<TripDto> CancelAsync(CancelTripCommand command, CancellationToken cancellationToken)
    {
        var trip = await GetRequiredTripAsync(command.TripId, cancellationToken);
        trip.Cancel(command.Reason);

        await repository.SaveChangesAsync(cancellationToken);

        return TripDto.FromDomain(trip);
    }

    public async Task<IReadOnlyCollection<TripTimelineEntryDto>> GetTimelineAsync(Guid id, CancellationToken cancellationToken)
    {
        var trip = await GetRequiredTripAsync(id, cancellationToken);
        return trip.Timeline
            .OrderBy(entry => entry.OccurredAt)
            .Select(TripTimelineEntryDto.FromDomain)
            .ToArray();
    }

    private async Task<Trip> GetRequiredTripAsync(Guid id, CancellationToken cancellationToken)
        => await repository.GetByIdAsync(id, cancellationToken) ?? throw new TripNotFoundException(id);
}
