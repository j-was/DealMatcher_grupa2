using DealMatcher.Backend.Core.Aggregates.Cart.Specifications;

namespace DealMatcher.Backend.UseCases.Features.Cart.Get;

public sealed class GetMyCartQueryHandler(
    IReadRepository<CartItem> cartItemsRepository,
    IReadRepository<OfferEntity> offersRepository,
    IMapper mapper)
    : IRequestHandler<GetMyCartQuery, Result<List<CartItemDTO>>>
{
    public async Task<Result<List<CartItemDTO>>> Handle(GetMyCartQuery request, CancellationToken cancellationToken)
    {
        var cartItems = await cartItemsRepository.ListAsync(
            new CartItemsByUserIdSpec(request.UserId),
            cancellationToken);

        var dto = new List<CartItemDTO>(cartItems.Count);

        foreach (var item in cartItems)
        {
            var offer = await offersRepository.GetByIdAsync(item.OfferId, cancellationToken) ?? throw new InvalidOperationException($"Offer with id {item.OfferId} referenced by cart item {item.Id} was not found.");
            dto.Add(mapper.Map<CartItemDTO>(new CartItemProfile.CartItemInfo(item, offer)));
        }

        return Result.Success(dto);
    }
}
