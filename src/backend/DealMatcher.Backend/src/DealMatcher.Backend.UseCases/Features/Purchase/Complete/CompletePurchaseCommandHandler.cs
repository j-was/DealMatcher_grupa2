namespace DealMatcher.Backend.UseCases.Features.Purchase.Complete;

public sealed class CompletePurchaseCommandHandler(
    IRepository<CartItem> cartRepository,
    IRepository<OfferEntity> offersRepository,
    IPublisher publisher
) : IRequestHandler<CompletePurchaseCommand, Result>
{
    public async Task<Result> Handle(CompletePurchaseCommand request, CancellationToken cancellationToken)
    {
        var cartItems = await cartRepository.ListAsync(new CartItemsByUserIdSpec(request.UserId), cancellationToken);

        foreach (var item in cartItems)
        {
            var offer = await offersRepository.GetByIdAsync(item.OfferId, cancellationToken);
            if (offer is null)
                return Result.NotFound("Offer not found");
            offer.DecreaseAvailability(item.Quantity);
            item.Delete();
            await cartRepository.UpdateAsync(item, cancellationToken);
            await offersRepository.UpdateAsync(offer, cancellationToken);
            await publisher.Publish(
                new OfferPurchasedEvent(offer.SellerId, offer.Id, item.Quantity, offer.Availability,
                    item.Quantity * offer.Price),
                cancellationToken);
        }

        return Result.Success();
    }
}
