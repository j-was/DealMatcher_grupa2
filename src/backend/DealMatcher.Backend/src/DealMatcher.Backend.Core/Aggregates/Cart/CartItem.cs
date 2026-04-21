namespace DealMatcher.Backend.Core.Aggregates.Cart;

public sealed class CartItem : DealMatcherEntityBase, IAggregateRoot
{
    public int UserId { get; private set; }
    public int OfferId { get; private set; }
    public int Quantity { get; private set; }
    public DateTime AddedAt { get; private set; }

    public CartItem(int userId, int offerId, int quantity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(offerId);
        ArgumentOutOfRangeException.ThrowIfLessThan(quantity, DataSchemaConstants.CartItemQuantityMinValue);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(quantity, DataSchemaConstants.CartItemQuantityMaxValue);

        UserId = userId;
        OfferId = offerId;
        Quantity = quantity;
        AddedAt = DateTime.UtcNow;
    }

    public void UpdateQuantity(int newQuantity)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(newQuantity, DataSchemaConstants.CartItemQuantityMinValue);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(newQuantity, DataSchemaConstants.CartItemQuantityMaxValue);

        Quantity = newQuantity;
    }


#pragma warning disable CS8618
    private CartItem()
    {
        /* EF */
    }
#pragma warning restore CS8618


}
