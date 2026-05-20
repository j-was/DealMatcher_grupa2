namespace DealMatcher.Backend.Core.Aggregates.Admin;

public sealed class ActivityRecord : DealMatcherEntityBase,
    IAggregateRoot
{
    public int UserId { get; }
    public int? OfferId { get; }
    public ActionType Action { get; }
    public List<ActivityDetail> Details { get; private set; }
    public string IpAddress { get; }

#pragma warning disable CS8618
    private ActivityRecord()
    {
        /* EF */
    }
#pragma warning restore CS8618

    public ActivityRecord(int userId, ActionType action, string ipAddress, List<ActivityDetail> details)
    {
        UserId = userId;
        OfferId = null;
        Action = action;
        Details = details;
        IpAddress = ipAddress;
    }

    public ActivityRecord(int userId, int offerId, ActionType action, string ipAddress, List<ActivityDetail> details)
    {
        UserId = userId;
        OfferId = offerId;
        Action = action;
        Details = details;
        IpAddress = ipAddress;
    }
}
