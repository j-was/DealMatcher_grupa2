namespace DealMatcher.Backend.Core.Aggregates.Offer.Specifications;

public sealed class OffersBySellerIdSpec : Specification<Offer>
{
    public OffersBySellerIdSpec(int sellerId)
    {
        Query.Where(o => o.SellerId == sellerId).OrderByDescending(o => o.CreatedAt);
    }
}
