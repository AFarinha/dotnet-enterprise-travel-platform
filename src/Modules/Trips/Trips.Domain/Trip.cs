namespace Trips.Domain;

public sealed class Trip
{
    public const int MaximumTravellers = 9;

    private readonly List<Traveller> _travellers = [];
    private readonly List<TripTimelineEntry> _timeline = [];

    private Trip()
    {
        Title = string.Empty;
        OwnerUserId = string.Empty;
    }

    private Trip(Guid id, string title, string ownerUserId)
    {
        Id = id;
        Title = title;
        OwnerUserId = ownerUserId;
        Status = TripStatus.Draft;
        CreatedAt = DateTimeOffset.UtcNow;
        AddTimeline("TripCreated", "Trip was created.");
    }

    public Guid Id { get; private set; }

    public string Title { get; private set; }

    public string OwnerUserId { get; private set; }

    public TripStatus Status { get; private set; }

    public Destination? Destination { get; private set; }

    public TravelPeriod? Period { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset? UpdatedAt { get; private set; }

    public IReadOnlyCollection<Traveller> Travellers => _travellers.AsReadOnly();

    public IReadOnlyCollection<TripTimelineEntry> Timeline => _timeline.AsReadOnly();

    public static Trip Create(string title, string ownerUserId)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new TripDomainException("Trip title is required.");
        }

        if (string.IsNullOrWhiteSpace(ownerUserId))
        {
            throw new TripDomainException("Trip owner is required.");
        }

        return new Trip(Guid.NewGuid(), title.Trim(), ownerUserId.Trim());
    }

    public void UpdateDraftDetails(string title, Destination destination, TravelPeriod period)
    {
        EnsureDraft("Only draft trips can be edited.");

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new TripDomainException("Trip title is required.");
        }

        Title = title.Trim();
        Destination = destination;
        Period = period;
        MarkUpdated();
        AddTimeline("TripUpdated", "Trip draft details were updated.");
    }

    public Traveller AddTraveller(string firstName, string lastName, string email, DateOnly? birthDate = null)
    {
        EnsureDraft("Travellers can only be added while the trip is in draft.");

        if (_travellers.Count >= MaximumTravellers)
        {
            throw new TripDomainException($"A trip cannot have more than {MaximumTravellers} travellers.");
        }

        var traveller = Traveller.Create(firstName, lastName, email, birthDate);
        _travellers.Add(traveller);
        MarkUpdated();
        AddTimeline("TravellerAdded", $"Traveller {traveller.Id} was added.");

        return traveller;
    }

    public void Plan()
    {
        EnsureDraft("Only draft trips can be planned.");

        if (Destination is null)
        {
            throw new TripDomainException("Destination must be defined before planning a trip.");
        }

        if (Period is null)
        {
            throw new TripDomainException("Trip dates must be defined before planning a trip.");
        }

        if (_travellers.Count == 0)
        {
            throw new TripDomainException("At least one traveller is required before planning a trip.");
        }

        Status = TripStatus.Planned;
        MarkUpdated();
        AddTimeline("TripPlanned", "Trip was planned.");
    }

    public void Cancel(string reason)
    {
        if (Status is TripStatus.Cancelled or TripStatus.Completed)
        {
            throw new TripDomainException("This trip cannot be cancelled from its current status.");
        }

        Status = TripStatus.Cancelled;
        MarkUpdated();
        AddTimeline("TripCancelled", string.IsNullOrWhiteSpace(reason) ? "Trip was cancelled." : reason.Trim());
    }

    private void EnsureDraft(string message)
    {
        if (Status != TripStatus.Draft)
        {
            throw new TripDomainException(message);
        }
    }

    private void MarkUpdated() => UpdatedAt = DateTimeOffset.UtcNow;

    private void AddTimeline(string eventType, string description)
        => _timeline.Add(TripTimelineEntry.Create(eventType, description));
}
