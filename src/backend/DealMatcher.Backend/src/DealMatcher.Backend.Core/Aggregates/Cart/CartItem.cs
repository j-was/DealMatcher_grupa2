namespace DealMatcher.Backend.Core.Aggregates.Cart;

public sealed class CartItem : DealMatcherEntityBase, IAggregateRoot
{
    public int UserId { get; private set; }
    public int OfferId { get; private set; }
    public int Quantity { get; private set; }
    public DateTime AddedAt { get; private set; }

    public CartItem(int userId, int offerId, int quantity)
    {
        OfferId = offerId;
        Quantity = quantity;
        AddedAt = DateTime.UtcNow;
        UserId = userId;
    }

#pragma warning disable CS8618
    private CartItem()
    {
        /* EF */
    }
#pragma warning restore CS8618


}
