namespace DealMatcher.Backend.UseCases.Features.Purchase.PaymentMethods;

public sealed record GetPaymentMethodsQuery : IQuery<Result<List<PaymentMethodDTO>>>;
