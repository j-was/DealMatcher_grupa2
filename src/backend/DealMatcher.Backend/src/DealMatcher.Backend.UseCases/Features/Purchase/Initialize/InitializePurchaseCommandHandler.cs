using Microsoft.Extensions.Configuration;

namespace DealMatcher.Backend.UseCases.Features.Purchase.Initialize;

public sealed class InitializePurchaseCommandHandler(
    IRepository<CartItem> cartItemsRepository,
    IReadRepository<OfferEntity> offersRepository, IConfiguration configuration)
    : IRequestHandler<InitializePurchaseCommand, CustomResult>
{
    public async Task<CustomResult> Handle(InitializePurchaseCommand request, CancellationToken cancellationToken)
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

                total += offer.Price * item.Quantity;

                item.Delete();
                await cartItemsRepository.UpdateAsync(item, cancellationToken);
            }

            var frontendOrigin = configuration["FrontendOrigin"]?.TrimEnd('/');

            var url = $"{frontendOrigin}/payment/{request.PaymentMethodId}/{total}";

            return new RedirectResult(url);
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

            cartItem.Delete();
            await cartItemsRepository.UpdateAsync(cartItem, cancellationToken);

            var frontendOrigin = configuration["FrontendOrigin"]?.TrimEnd('/');

            var url = $"{frontendOrigin}/payment/{request.PaymentMethodId}/{offer.Price * request.Quantity}";

            return new RedirectResult(url);
        }
    }
}
