using Ardalis.SharedKernel;
using DealMatcher.Backend.Core.Aggregates.Cart;

namespace DealMatcher.Backend.UseCases.Features.Cart.Delete;

public sealed class DeleteCartItemHandler(
    IRepository<CartItem> cartRepository
) : IRequestHandler<DeleteCartItemCommand, Result>
{
    public async Task<Result> Handle(DeleteCartItemCommand request, CancellationToken ct)
    {
        var cartItem = await cartRepository.GetByIdAsync(request.CartItemId, ct);

        if (cartItem is null)
        {
            return Result.NotFound("Cart item not found");
        }

        if (cartItem.UserId != request.UserId)
        {
            return Result.Forbidden("Forbidden - not your cart item");
        }

        cartItem.Delete();
        await cartRepository.UpdateAsync(cartItem, ct);

        return Result.Success();
    }
}
