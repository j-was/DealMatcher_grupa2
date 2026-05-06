namespace DealMatcher.Backend.UseCases.Features.Purchase.DeliveryMethods;

public sealed record GetDeliveryMethodsQuery : IQuery<Result<List<DeliveryMethodDTO>>>;
