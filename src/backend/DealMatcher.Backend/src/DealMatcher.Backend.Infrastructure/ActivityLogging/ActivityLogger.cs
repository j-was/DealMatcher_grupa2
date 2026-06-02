namespace DealMatcher.Backend.Infrastructure.ActivityLogging;

public sealed class ActivityLogger(
    AppDbContext dbContext,
    IHttpContextAccessor httpContextAccessor) : IActivityLogger
{
    public async Task LogUserActivityAsync(
        int userId,
        ActionType action,
        List<ActivityDetail>? details = null,
        string? ipAddress = null,
        CancellationToken cancellationToken = default)
    {
        var activityRecord = new ActivityRecord(userId, action, ipAddress ?? GetIpAddress(), details ?? []);

        await dbContext.Set<ActivityRecord>().AddRangeAsync(activityRecord);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task LogOfferActivityAsync(
        int userId,
        ActionType action,
        int offerId,
        List<ActivityDetail>? details = null,
        string? ipAddress = null,
        CancellationToken cancellationToken = default)
    {
        var activityRecord = new ActivityRecord(userId, offerId, action, ipAddress ?? GetIpAddress(), details ?? []);

        await dbContext.Set<ActivityRecord>().AddRangeAsync(activityRecord);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private string GetIpAddress()
    {
        return httpContextAccessor.HttpContext?
            .Connection?
            .RemoteIpAddress?
            .ToString() ?? "unknown";
    }
}
