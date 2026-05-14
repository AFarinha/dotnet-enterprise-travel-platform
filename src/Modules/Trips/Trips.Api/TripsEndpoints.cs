using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Trips.Application;
using Trips.Domain;

namespace Trips.Api;

public static class TripsEndpoints
{
    public static IEndpointRouteBuilder MapTripsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/trips")
            .WithTags("Trips");

        group.MapPost("/", CreateTripAsync)
            .WithName("CreateTrip");

        group.MapGet("/{id:guid}", GetTripAsync)
            .WithName("GetTripById");

        group.MapGet("/", ListTripsAsync)
            .WithName("ListTrips");

        group.MapPost("/{id:guid}/travellers", AddTravellerAsync)
            .WithName("AddTripTraveller");

        group.MapPost("/{id:guid}/plan", PlanTripAsync)
            .WithName("PlanTrip");

        group.MapPost("/{id:guid}/cancel", CancelTripAsync)
            .WithName("CancelTrip");

        group.MapGet("/{id:guid}/timeline", GetTimelineAsync)
            .WithName("GetTripTimeline");

        return app;
    }

    private static async Task<IResult> CreateTripAsync(CreateTripRequest request, ITripService service, CancellationToken cancellationToken)
        => await ExecuteAsync(async () =>
        {
            var trip = await service.CreateAsync(
                new CreateTripCommand(request.Title, request.OwnerUserId, request.DestinationCountryCode, request.DestinationCity, request.StartsOn, request.EndsOn),
                cancellationToken);

            return Results.Created($"/api/trips/{trip.Id}", trip);
        });

    private static async Task<IResult> GetTripAsync(Guid id, ITripService service, CancellationToken cancellationToken)
        => await ExecuteAsync(async () => Results.Ok(await service.GetByIdAsync(id, cancellationToken)));

    private static async Task<IResult> ListTripsAsync(
        ITripService service,
        int page,
        int pageSize,
        TripStatus? status,
        string? destinationCountry,
        string? sortBy,
        string? sortDirection,
        CancellationToken cancellationToken)
        => await ExecuteAsync(async () =>
        {
            var trips = await service.ListAsync(
                new TripsQuery(page, pageSize, status, destinationCountry, sortBy ?? "createdAt", sortDirection ?? "desc"),
                cancellationToken);

            return Results.Ok(trips);
        });

    private static async Task<IResult> AddTravellerAsync(Guid id, AddTravellerRequest request, ITripService service, CancellationToken cancellationToken)
        => await ExecuteAsync(async () =>
        {
            var traveller = await service.AddTravellerAsync(
                new AddTravellerCommand(id, request.FirstName, request.LastName, request.Email, request.BirthDate),
                cancellationToken);

            return Results.Created($"/api/trips/{id}/travellers/{traveller.Id}", traveller);
        });

    private static async Task<IResult> PlanTripAsync(Guid id, ITripService service, CancellationToken cancellationToken)
        => await ExecuteAsync(async () => Results.Ok(await service.PlanAsync(new PlanTripCommand(id), cancellationToken)));

    private static async Task<IResult> CancelTripAsync(Guid id, CancelTripRequest request, ITripService service, CancellationToken cancellationToken)
        => await ExecuteAsync(async () => Results.Ok(await service.CancelAsync(new CancelTripCommand(id, request.Reason), cancellationToken)));

    private static async Task<IResult> GetTimelineAsync(Guid id, ITripService service, CancellationToken cancellationToken)
        => await ExecuteAsync(async () => Results.Ok(await service.GetTimelineAsync(id, cancellationToken)));

    private static async Task<IResult> ExecuteAsync(Func<Task<IResult>> action)
    {
        try
        {
            return await action();
        }
        catch (TripNotFoundException exception)
        {
            return Problem(StatusCodes.Status404NotFound, "Trip not found", exception.Message);
        }
        catch (TripDomainException exception)
        {
            return Problem(StatusCodes.Status409Conflict, "Trip rule violation", exception.Message);
        }
        catch (ArgumentException exception)
        {
            return Problem(StatusCodes.Status400BadRequest, "Invalid request", exception.Message);
        }
    }

    private static ProblemHttpResult Problem(int statusCode, string title, string detail)
        => TypedResults.Problem(new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail
        });
}

public sealed record CreateTripRequest(string Title, string OwnerUserId, string? DestinationCountryCode, string? DestinationCity, DateOnly? StartsOn, DateOnly? EndsOn);

public sealed record AddTravellerRequest(string FirstName, string LastName, string Email, DateOnly? BirthDate);

public sealed record CancelTripRequest(string Reason);
