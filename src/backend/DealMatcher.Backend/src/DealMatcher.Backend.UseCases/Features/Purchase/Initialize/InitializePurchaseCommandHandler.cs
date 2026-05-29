using DealMatcher.Backend.Core.Events;
using Microsoft.Extensions.Configuration;

namespace DealMatcher.Backend.UseCases.Features.Purchase.Initialize;

public sealed class InitializePurchaseCommandHandler(
    IRepository<CartItem> cartItemsRepository,
    IReadRepository<OfferEntity> offersRepository, IConfiguration configuration,
    IPublisher publisher)
    : IRequestHandler<InitializePurchaseCommand, Result<InitializePurchaseResponse>>
{
    public async Task<Result<InitializePurchaseResponse>> Handle(InitializePurchaseCommand request, CancellationToken cancellationToken)
    {
        if (request.OfferId == -2137)
        {
            var cartItems = await cartItemsRepository.ListAsync(
                new CartItemsByUserIdSpec(request.UserId),
                cancellationToken);

            if (cartItems.Count < 1)
                return Result.Invalid(new ValidationError("Invalid purchase data"));

            var total = 0m;
            foreach (var item in cartItems)
            {
                var offer = await offersRepository.GetByIdAsync(item.OfferId, cancellationToken);
                if (offer is null)
                    continue;

                var price = offer.Price * item.Quantity;

                total += price;

                await publisher.Publish(new OfferPurchasedEvent(offer.SellerId, offer.Id, item.Quantity, offer.Availability, price), cancellationToken);
            }

            var frontendOrigin = configuration["FrontendOrigin"]?.TrimEnd('/');

            var url = $"{frontendOrigin}/payment/{request.PaymentMethodId}/{total}";

            return Result.Success(new InitializePurchaseResponse(url));
        }
        else
        {
            if (request.Quantity < 1)
                return Result.Invalid(new ValidationError("Invalid purchase data"));

            var offer = await offersRepository.GetByIdAsync(request.OfferId, cancellationToken);
            if (offer is null)
                return Result.NotFound("Offer not found");

            var cartItems = await cartItemsRepository.ListAsync(
                new CartItemsByUserIdSpec(request.UserId),
                cancellationToken);


            if (!(cartItems.Any(x => x.OfferId == request.OfferId)))
            {
                return Result.Conflict("Offer not available to purchase");
            }

            var cartItem = cartItems.First(x => x.OfferId == request.OfferId);

            var frontendOrigin = configuration["FrontendOrigin"]?.TrimEnd('/');

            var price = offer.Price * request.Quantity;

            var url = $"{frontendOrigin}/payment/{request.PaymentMethodId}/{price}";
            await publisher.Publish(
                new OfferPurchasedEvent(offer.SellerId, offer.Id, cartItem.Quantity, offer.Availability, price),
                cancellationToken);

            return Result.Success(new InitializePurchaseResponse(url));
        }
    }
}
