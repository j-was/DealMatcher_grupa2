// using DealMatcher.Backend.Core.ContributorAggregate;

namespace DealMatcher.Backend.Infrastructure.Data;

public static class SeedData
{

    public static async Task InitializeAsync(AppDbContext dbContext)
    {
        if (!dbContext.Set<Offer>().
                Any())
        {
            await SeedOffers(dbContext);
        }
    }

    public static async Task SeedOffers(AppDbContext dbContext)
    {
        var offers = new List<Offer>();

        // TODO

        await dbContext.Set<Offer>().AddRangeAsync(offers);
        _ = await dbContext.SaveChangesAsync();
    }
}
