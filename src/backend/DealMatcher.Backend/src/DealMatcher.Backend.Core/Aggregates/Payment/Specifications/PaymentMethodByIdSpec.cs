namespace DealMatcher.Backend.Core.Aggregates.Payment.Specifications;

public sealed class PaymentMethodByIdSpec : SingleResultSpecification<PaymentMethod>
{
    public PaymentMethodByIdSpec(int paymentId)
    {
        Query.Where(pm => pm.Id == paymentId);
    }
}
