namespace Trips.Domain;

public sealed class TripTimelineEntry
{
    private TripTimelineEntry()
    {
        EventType = string.Empty;
        Description = string.Empty;
    }

    private TripTimelineEntry(Guid id, string eventType, string description, DateTimeOffset occurredAt)
    {
        Id = id;
        EventType = eventType;
        Description = description;
        OccurredAt = occurredAt;
    }

    public Guid Id { get; private set; }

    public string EventType { get; private set; }

    public string Description { get; private set; }

    public DateTimeOffset OccurredAt { get; private set; }

    public static TripTimelineEntry Create(string eventType, string description)
        => new(Guid.NewGuid(), eventType, description, DateTimeOffset.UtcNow);
}
