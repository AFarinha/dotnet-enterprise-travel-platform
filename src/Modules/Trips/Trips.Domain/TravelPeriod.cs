namespace Trips.Domain;

public sealed record TravelPeriod(DateOnly StartsOn, DateOnly EndsOn)
{
    public TravelPeriod() : this(default, default)
    {
    }

    public static TravelPeriod Create(DateOnly startsOn, DateOnly endsOn)
    {
        if (startsOn == default || endsOn == default)
        {
            throw new TripDomainException("Trip dates are required.");
        }

        if (endsOn < startsOn)
        {
            throw new TripDomainException("Trip end date cannot be before start date.");
        }

        return new TravelPeriod(startsOn, endsOn);
    }
}
