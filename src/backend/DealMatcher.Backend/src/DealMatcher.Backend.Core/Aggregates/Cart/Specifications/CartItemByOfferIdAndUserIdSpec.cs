namespace DealMatcher.Backend.Core.Aggregates.Cart.Specifications;

public sealed class CartItemByOfferIdAndByUserIdSpec : Specification<CartItem>
{
    public  CartItemByOfferIdAndByUserIdSpec(int userId, int offerId)
    {
        Query.Where(c => c.UserId == userId && c.OfferId==offerId);
    }
}
