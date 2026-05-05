namespace DealMatcher.Backend.Web.Endpoints.Purchases;

public sealed record InitializePurchaseRequest(
    int OfferId,
    string DeliveryMethodId,
    string PaymentMethodId,
    int Quantity = 1
);
