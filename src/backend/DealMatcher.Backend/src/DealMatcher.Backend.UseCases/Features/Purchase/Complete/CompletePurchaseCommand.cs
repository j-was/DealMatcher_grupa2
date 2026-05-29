namespace DealMatcher.Backend.UseCases.Features.Purchase.Complete;

public sealed record CompletePurchaseCommand(int UserId) : IRequest<Result>;
