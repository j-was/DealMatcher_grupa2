namespace DealMatcher.Backend.Infrastructure.ActivityLogging;

public sealed class ActivityLogger(
    AppDbContext dbContext,
    IHttpContextAccessor httpContextAccessor) : IActivityLogger
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task LogUserActivityAsync(
        int userId,
        ActionType action,
        List<ActivityDetail>? details = null,
        string? ipAddress = null,
        CancellationToken cancellationToken = default)
    {
        var activityRecord = new ActivityRecord(userId, action, ipAddress ?? GetIpAddress(), details ?? []);

        await _dbContext.Set<ActivityRecord>().AddRangeAsync(activityRecord);
        await _dbContext.SaveChangesAsync(cancellationToken);
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

        await _dbContext.Set<ActivityRecord>().AddRangeAsync(activityRecord);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private string GetIpAddress()
    {
        return _httpContextAccessor.HttpContext?
            .Connection?
            .RemoteIpAddress?
            .ToString() ?? "unknown";
    }
}
