namespace DealMatcher.Backend.Core.Aggregates.Payment.Specifications;

public sealed class DeliveryMethodByIdSpec : SingleResultSpecification<DeliveryMethod>
{
    public DeliveryMethodByIdSpec(int deliveryId)
    {
        Query.Where(dm => dm.Id == deliveryId);
    }
}
