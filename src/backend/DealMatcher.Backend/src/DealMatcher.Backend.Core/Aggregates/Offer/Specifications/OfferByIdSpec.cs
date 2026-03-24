namespace DealMatcher.Backend.Core.Aggregates.Offer.Specifications;

public sealed class OfferByIdSpec : SingleResultSpecification<Offer>
{
    public OfferByIdSpec(int offerId)
    {
        Query.Where(o => o.Id == offerId);
    }
}
