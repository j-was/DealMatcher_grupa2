namespace DealMatcher.Backend.Core.Aggregates.Admin.Specifications;

public class ActivityRecordsByUserIdSpec : Specification<ActivityRecord>
{
    public ActivityRecordsByUserIdSpec(int userId)
    {
        Query.Where(o => o.UserId == userId).OrderByDescending(o => o.CreatedAt);
    }
}
