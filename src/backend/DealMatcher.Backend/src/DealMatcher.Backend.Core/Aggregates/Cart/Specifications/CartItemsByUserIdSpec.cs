namespace DealMatcher.Backend.Core.Aggregates.Cart.Specifications;

public sealed class CartItemsByUserIdSpec : Specification<CartItem>
{
    public CartItemsByUserIdSpec(int userId)
    {
        Query.Where(c => c.UserId == userId).OrderByDescending(o => o.AddedAt);
    }
}
