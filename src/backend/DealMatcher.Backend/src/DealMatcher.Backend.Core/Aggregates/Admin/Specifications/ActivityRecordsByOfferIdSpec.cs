namespace DealMatcher.Backend.Core.Aggregates.Admin.Specifications;

public class ActivityRecordsByOfferIdSpec: Specification<ActivityRecord>
{
    public ActivityRecordsByOfferIdSpec(int offerId)
    {
        Query.Where(o => o.OfferId == offerId).OrderByDescending(o => o.CreatedAt);
    }
}
