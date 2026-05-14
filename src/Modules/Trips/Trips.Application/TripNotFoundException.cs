namespace Trips.Application;

public sealed class TripNotFoundException(Guid tripId) : KeyNotFoundException($"Trip '{tripId}' was not found.")
{
    public Guid TripId { get; } = tripId;
}
