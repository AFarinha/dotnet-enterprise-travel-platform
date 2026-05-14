using Microsoft.EntityFrameworkCore;
using Trips.Domain;

namespace Trips.Infrastructure;

public sealed class TripsDbContext(DbContextOptions<TripsDbContext> options) : DbContext(options)
{
    public DbSet<Trip> Trips => Set<Trip>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("trips");

        var trip = modelBuilder.Entity<Trip>();
        trip.ToTable("Trips");
        trip.HasKey(x => x.Id);
        trip.Property(x => x.Title).HasMaxLength(180).IsRequired();
        trip.Property(x => x.OwnerUserId).HasMaxLength(120).IsRequired();
        trip.Property(x => x.Status).HasConversion<string>().HasMaxLength(32).IsRequired();
        trip.Property(x => x.CreatedAt).IsRequired();
        trip.Property(x => x.UpdatedAt);

        trip.OwnsOne(x => x.Destination, destination =>
        {
            destination.Property(x => x.CountryCode).HasColumnName("DestinationCountryCode").HasMaxLength(2);
            destination.Property(x => x.City).HasColumnName("DestinationCity").HasMaxLength(120);
        });

        trip.OwnsOne(x => x.Period, period =>
        {
            period.Property(x => x.StartsOn).HasColumnName("StartsOn");
            period.Property(x => x.EndsOn).HasColumnName("EndsOn");
        });

        trip.Navigation(x => x.Destination).IsRequired(false);
        trip.Navigation(x => x.Period).IsRequired(false);

        trip.HasMany(x => x.Travellers)
            .WithOne()
            .HasForeignKey("TripId")
            .OnDelete(DeleteBehavior.Cascade);

        trip.Navigation(x => x.Travellers)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        trip.HasMany(x => x.Timeline)
            .WithOne()
            .HasForeignKey("TripId")
            .OnDelete(DeleteBehavior.Cascade);

        trip.Navigation(x => x.Timeline)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        var traveller = modelBuilder.Entity<Traveller>();
        traveller.ToTable("Travellers");
        traveller.HasKey(x => x.Id);
        traveller.Property(x => x.FirstName).HasMaxLength(80).IsRequired();
        traveller.Property(x => x.LastName).HasMaxLength(80).IsRequired();
        traveller.Property(x => x.Email).HasMaxLength(180).IsRequired();
        traveller.Property(x => x.BirthDate);

        var timelineEntry = modelBuilder.Entity<TripTimelineEntry>();
        timelineEntry.ToTable("TripTimeline");
        timelineEntry.HasKey(x => x.Id);
        timelineEntry.Property(x => x.EventType).HasMaxLength(80).IsRequired();
        timelineEntry.Property(x => x.Description).HasMaxLength(500).IsRequired();
        timelineEntry.Property(x => x.OccurredAt).IsRequired();
    }
}
