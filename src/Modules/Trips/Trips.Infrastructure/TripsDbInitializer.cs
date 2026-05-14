using Microsoft.EntityFrameworkCore;

namespace Trips.Infrastructure;

public interface ITripsDbInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken);
}

internal sealed class TripsDbInitializer(TripsDbContext dbContext) : ITripsDbInitializer
{
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if (dbContext.Database.IsRelational())
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
            return;
        }

        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
    }
}
