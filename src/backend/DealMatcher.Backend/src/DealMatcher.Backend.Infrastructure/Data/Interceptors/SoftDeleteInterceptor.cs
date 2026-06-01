namespace DealMatcher.Backend.Infrastructure.Data.Interceptors;

public sealed class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var entries = eventData.Context?.ChangeTracker.Entries()
            .Where(e =>
            {
                return e is { State: EntityState.Deleted, Entity: DealMatcherEntityBase };
            }).ToList();

        if (entries is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        foreach (var entry in entries)
        {
            entry.State = EntityState.Modified;
            var baseEntity = entry.Entity as DealMatcherEntityBase;
            baseEntity?.Delete();
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
