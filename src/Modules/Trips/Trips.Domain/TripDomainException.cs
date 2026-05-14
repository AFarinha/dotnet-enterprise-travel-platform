namespace Trips.Domain;

public sealed class TripDomainException(string message) : InvalidOperationException(message);
