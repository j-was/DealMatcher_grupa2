using DealMatcher.Backend.Core.Aggregates.Cart.Specifications;

namespace DealMatcher.Backend.UseCases.Features.Cart.Total;

public sealed class GetCartTotalQueryHandler(
    IReadRepository<CartItem> cartItemsRepository,
    IReadRepository<OfferEntity> offersRepository)
    : IRequestHandler<GetCartTotalQuery, Result<CartTotalDTO>>
{
    public async Task<Result<CartTotalDTO>> Handle(GetCartTotalQuery request, CancellationToken ct)
    {
        var cartItems = await cartItemsRepository.ListAsync(new CartItemsByUserIdSpec(request.UserId), ct);

        decimal total = 0m;

        foreach (var item in cartItems)
        {
            var offer = await offersRepository.GetByIdAsync(item.OfferId, ct);
            if (offer is null)
                continue;

            total += offer.Price * item.Quantity;
        }

        return Result.Success(new CartTotalDTO((double)decimal.Round(total, 2), "PLN"));
    }
}
