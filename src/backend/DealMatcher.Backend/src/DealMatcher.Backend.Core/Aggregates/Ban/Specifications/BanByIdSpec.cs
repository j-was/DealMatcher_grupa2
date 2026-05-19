namespace DealMatcher.Backend.Core.Aggregates.Ban.Specifications;

public sealed class BanByIdSpec : SingleResultSpecification<Ban>
{
    public BanByIdSpec(int banId)
    {
        Query.Where(b => b.Id == banId);
    }
}
