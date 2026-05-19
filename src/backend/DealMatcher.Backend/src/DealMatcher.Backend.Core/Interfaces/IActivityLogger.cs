using DealMatcher.Backend.Core.Aggregates.Admin;

namespace DealMatcher.Backend.Core.Interfaces;

public interface IActivityLogger
{
    Task LogUserActivityAsync(
        int userId,
        ActionType action,
        List<ActivityDetail>? details = null,
        string? ipAddress = null,
        CancellationToken cancellationToken = default);

    Task LogOfferActivityAsync(
        int userId,
        ActionType action,
        int offerId,
        List<ActivityDetail>? details = null,
        string? ipAddress = null,
        CancellationToken cancellationToken = default);
}
