namespace Trips.Domain;

public sealed class Traveller
{
    private Traveller()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
    }

    private Traveller(Guid id, string firstName, string lastName, string email, DateOnly? birthDate)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        BirthDate = birthDate;
    }

    public Guid Id { get; private set; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string Email { get; private set; }

    public DateOnly? BirthDate { get; private set; }

    public static Traveller Create(string firstName, string lastName, string email, DateOnly? birthDate = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new TripDomainException("Traveller first name is required.");
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new TripDomainException("Traveller last name is required.");
        }

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@', StringComparison.Ordinal))
        {
            throw new TripDomainException("Traveller email is invalid.");
        }

        return new Traveller(Guid.NewGuid(), firstName.Trim(), lastName.Trim(), email.Trim(), birthDate);
    }
}
