namespace DealMatcher.Backend.Web.Endpoints.Cart;

public sealed record UpdateCartItemQuantityRequest(
    int CartItemId,
    int Quantity
);
