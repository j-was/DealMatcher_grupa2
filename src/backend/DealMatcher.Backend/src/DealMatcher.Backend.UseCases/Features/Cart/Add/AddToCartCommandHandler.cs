using DealMatcher.Backend.Core.Aggregates.Cart.Specifications;

namespace DealMatcher.Backend.UseCases.Features.Cart.Add;

public sealed class AddToCartCommandHandler(
    IRepository<CartItem> cartItemsRepository,
    IReadRepository<OfferEntity> offersRepository,
    IMapper mapper)
    : IRequestHandler<AddToCartCommand, Result<CartItemDTO>>
{
    public async Task<Result<CartItemDTO>> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        if (request.Quantity < 1)
            return Result.Invalid(new ValidationError("Invalid request"));

        var offer = await offersRepository.GetByIdAsync(request.OfferId, cancellationToken);
        if (offer is null)
            return Result.NotFound("Offer not found");

        var cartItems = await cartItemsRepository.ListAsync(
            new CartItemsByUserIdSpec(request.UserId),
            cancellationToken);

        if (cartItems.Any(x => x.OfferId == request.OfferId))
            return Result.Conflict("Item already in cart");

        var cartItem = new CartItem(request.UserId, request.OfferId, request.Quantity);

        await cartItemsRepository.AddAsync(cartItem, cancellationToken);
        await cartItemsRepository.SaveChangesAsync(cancellationToken);

        var dto = mapper.Map<CartItemDTO>(new CartItemProfile.CartItemInfo(cartItem, offer));

        return Result.Created(dto);
    }
}
