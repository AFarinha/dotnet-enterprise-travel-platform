namespace Trips.Domain;

public sealed record Destination(string CountryCode, string City)
{
    public Destination() : this(string.Empty, string.Empty)
    {
    }

    public static Destination Create(string countryCode, string city)
    {
        if (string.IsNullOrWhiteSpace(countryCode) || countryCode.Trim().Length != 2)
        {
            throw new TripDomainException("Destination country code must be an ISO 3166-1 alpha-2 code.");
        }

        if (string.IsNullOrWhiteSpace(city))
        {
            throw new TripDomainException("Destination city is required.");
        }

        return new Destination(countryCode.Trim().ToUpperInvariant(), city.Trim());
    }
}
