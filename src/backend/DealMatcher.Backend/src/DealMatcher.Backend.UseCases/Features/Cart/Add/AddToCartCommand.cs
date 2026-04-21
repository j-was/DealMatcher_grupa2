namespace DealMatcher.Backend.UseCases.Features.Cart.Add;

public sealed record AddToCartCommand(int UserId, int OfferId, int Quantity)
    : IRequest<Result<CartItemDTO>>;
