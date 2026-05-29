namespace DealMatcher.Backend.UseCases.Features.Purchase.Complete;

public sealed class CompletePurchaseCommandHandler(
    IRepository<CartItem> cartRepository
) : IRequestHandler<CompletePurchaseCommand, Result>
{
    public async Task<Result> Handle(CompletePurchaseCommand request, CancellationToken cancellationToken)
    {
        var cartItems = await cartRepository.ListAsync(new CartItemsByUserIdSpec(request.UserId), cancellationToken);

        foreach (var item in cartItems)
        {
            item.Delete();
            await cartRepository.UpdateAsync(item, cancellationToken);
        }

        return Result.Success();
    }
}
