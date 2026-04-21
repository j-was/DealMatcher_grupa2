namespace DealMatcher.Backend.Core.Aggregates.Cart;

public sealed class CartItem: DealMatcherEntityBase, IAggregateRoot
{
    public int OfferId { get; private set; }
    public int Quantity { get; private set; }
    public DateTime AddedAt { get; private set; }

    public CartItem(int offerId, int quantity)
    {
        OfferId = offerId;
        Quantity = quantity;
        AddedAt = DateTime.UtcNow;
    }

#pragma warning disable CS8618
    private CartItem()
    {
        /* EF */
    }
#pragma warning restore CS8618


}
