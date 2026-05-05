namespace DealMatcher.Backend.UseCases.Features.Purchase.Initialize;

public sealed record InitializePurchaseCommand(
    int UserId,
    int OfferId,
    string PaymentMethodId,
    int Quantity
) : IRequest<CustomResult>;
