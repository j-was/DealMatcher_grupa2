namespace DealMatcher.Backend.Web.Endpoints.Cart;


public sealed record AddToCartRequest(
    int OfferId,
    int Quantity = 1
);
