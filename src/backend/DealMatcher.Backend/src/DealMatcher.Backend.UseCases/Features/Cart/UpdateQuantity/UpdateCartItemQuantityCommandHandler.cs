using Ardalis.Result;
using Ardalis.SharedKernel;
using AutoMapper;
using DealMatcher.Backend.Core.Aggregates.Cart;
using DealMatcher.Backend.Core.Aggregates.Cart.DTOs;

namespace DealMatcher.Backend.UseCases.Features.Cart.UpdateQuantity;

public sealed class UpdateCartItemQuantityCommandHandler(
    IRepository<CartItem> cartItemsRepository,
    IReadRepository<OfferEntity> offersRepository,
    IMapper mapper
) : IRequestHandler<UpdateCartItemQuantityCommand, Result<object>>
{
    public async Task<Result<object>> Handle(UpdateCartItemQuantityCommand request, CancellationToken cancellationToken)
    {
        if (request.Quantity < 1)
        {
            return Result.Invalid(new ValidationError("Invalid quantity"));
        }

        var cartItem = await cartItemsRepository.GetByIdAsync(request.CartItemId, cancellationToken);

        if (cartItem is null)
        {
            return Result.NotFound();
        }

        if (cartItem.UserId != request.UserId)
        {
            return Result.Forbidden();
        }

        var offer = await offersRepository.GetByIdAsync(cartItem.OfferId, cancellationToken);

        if (offer is null)
        {
            return Result.NotFound("Offer not found");
        }

        cartItem.UpdateQuantity(request.Quantity);

        await cartItemsRepository.UpdateAsync(cartItem, cancellationToken);
        await cartItemsRepository.SaveChangesAsync(cancellationToken);

        var dto = mapper.Map<CartItem>(new CartItemProfile.CartItemInfo(cartItem, offer));
        return Result.Success(dto);
    }
}
