using Ardalis.Result;

namespace DealMatcher.Backend.UseCases.Features.Cart.UpdateQuantity;

public sealed record UpdateCartItemQuantityCommand(
    int CartItemId,
    int UserId,
    int Quantity
) : IRequest<Result<object>>;
