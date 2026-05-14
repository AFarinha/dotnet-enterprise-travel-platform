using Trips.Domain;

namespace Trips.Application;

public sealed record TripsQuery(
    int Page = 1,
    int PageSize = 20,
    TripStatus? Status = null,
    string? DestinationCountry = null,
    string SortBy = "createdAt",
    string SortDirection = "desc")
{
    public int SafePage => Page < 1 ? 1 : Page;

    public int SafePageSize => PageSize is < 1 or > 100 ? 20 : PageSize;
}
