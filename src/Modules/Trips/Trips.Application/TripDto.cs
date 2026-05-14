using Trips.Domain;

namespace Trips.Application;

public sealed record TripDto(
    Guid Id,
    string Title,
    string OwnerUserId,
    TripStatus Status,
    DestinationDto? Destination,
    TravelPeriodDto? Period,
    int TravellerCount,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static TripDto FromDomain(Trip trip)
        => new(
            trip.Id,
            trip.Title,
            trip.OwnerUserId,
            trip.Status,
            trip.Destination is null ? null : new DestinationDto(trip.Destination.CountryCode, trip.Destination.City),
            trip.Period is null ? null : new TravelPeriodDto(trip.Period.StartsOn, trip.Period.EndsOn),
            trip.Travellers.Count,
            trip.CreatedAt,
            trip.UpdatedAt);
}

public sealed record DestinationDto(string CountryCode, string City);

public sealed record TravelPeriodDto(DateOnly StartsOn, DateOnly EndsOn);

public sealed record TravellerDto(Guid Id, string FirstName, string LastName, string Email, DateOnly? BirthDate)
{
    public static TravellerDto FromDomain(Traveller traveller)
        => new(traveller.Id, traveller.FirstName, traveller.LastName, traveller.Email, traveller.BirthDate);
}

public sealed record TripTimelineEntryDto(Guid Id, string EventType, string Description, DateTimeOffset OccurredAt)
{
    public static TripTimelineEntryDto FromDomain(TripTimelineEntry entry)
        => new(entry.Id, entry.EventType, entry.Description, entry.OccurredAt);
}
