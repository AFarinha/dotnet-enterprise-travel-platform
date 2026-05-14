using Trips.Domain;

namespace Trips.UnitTests;

public sealed class TripTests
{
    [Fact]
    public void Create_WithValidInput_StartsInDraftAndAddsTimelineEntry()
    {
        var trip = Trip.Create("Lisbon business trip", "user-1");

        Assert.Equal(TripStatus.Draft, trip.Status);
        Assert.Single(trip.Timeline);
        Assert.Equal("TripCreated", trip.Timeline.Single().EventType);
    }

    [Fact]
    public void AddTraveller_WhenTripIsDraft_AddsTravellerAndTimelineEntry()
    {
        var trip = CreateReadyDraftTrip();

        var traveller = trip.AddTraveller("Ana", "Silva", "ana@example.com");

        Assert.Equal(traveller.Id, trip.Travellers.Single().Id);
        Assert.Contains(trip.Timeline, entry => entry.EventType == "TravellerAdded");
    }

    [Fact]
    public void AddTraveller_WhenMaximumTravellersReached_Throws()
    {
        var trip = CreateReadyDraftTrip();

        for (var i = 0; i < Trip.MaximumTravellers; i++)
        {
            trip.AddTraveller($"Traveller{i}", "Silva", $"traveller{i}@example.com");
        }

        var exception = Assert.Throws<TripDomainException>(() => trip.AddTraveller("Extra", "Silva", "extra@example.com"));
        Assert.Contains("more than", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Plan_WhenMissingTraveller_Throws()
    {
        var trip = Trip.Create("Lisbon business trip", "user-1");
        trip.UpdateDraftDetails("Lisbon business trip", Destination.Create("PT", "Lisbon"), TravelPeriod.Create(new DateOnly(2026, 6, 1), new DateOnly(2026, 6, 5)));

        var exception = Assert.Throws<TripDomainException>(trip.Plan);

        Assert.Contains("At least one traveller", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Plan_WhenReady_ChangesStatusToPlanned()
    {
        var trip = CreateReadyDraftTrip();
        trip.AddTraveller("Ana", "Silva", "ana@example.com");

        trip.Plan();

        Assert.Equal(TripStatus.Planned, trip.Status);
        Assert.Contains(trip.Timeline, entry => entry.EventType == "TripPlanned");
    }

    [Fact]
    public void AddTraveller_AfterPlanning_Throws()
    {
        var trip = CreateReadyDraftTrip();
        trip.AddTraveller("Ana", "Silva", "ana@example.com");
        trip.Plan();

        var exception = Assert.Throws<TripDomainException>(() => trip.AddTraveller("Bruno", "Costa", "bruno@example.com"));

        Assert.Contains("draft", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Cancel_WhenDraft_ChangesStatusToCancelled()
    {
        var trip = CreateReadyDraftTrip();

        trip.Cancel("Customer requested cancellation.");

        Assert.Equal(TripStatus.Cancelled, trip.Status);
        Assert.Contains(trip.Timeline, entry => entry.EventType == "TripCancelled");
    }

    [Fact]
    public void TravelPeriod_WhenEndBeforeStart_Throws()
    {
        var exception = Assert.Throws<TripDomainException>(() => TravelPeriod.Create(new DateOnly(2026, 6, 5), new DateOnly(2026, 6, 1)));

        Assert.Contains("end date", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    private static Trip CreateReadyDraftTrip()
    {
        var trip = Trip.Create("Lisbon business trip", "user-1");
        trip.UpdateDraftDetails("Lisbon business trip", Destination.Create("PT", "Lisbon"), TravelPeriod.Create(new DateOnly(2026, 6, 1), new DateOnly(2026, 6, 5)));
        return trip;
    }
}
