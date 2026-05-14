namespace Trips.Application;

public sealed record CreateTripCommand(string Title, string OwnerUserId, string? DestinationCountryCode, string? DestinationCity, DateOnly? StartsOn, DateOnly? EndsOn);

public sealed record AddTravellerCommand(Guid TripId, string FirstName, string LastName, string Email, DateOnly? BirthDate);

public sealed record PlanTripCommand(Guid TripId);

public sealed record CancelTripCommand(Guid TripId, string Reason);
