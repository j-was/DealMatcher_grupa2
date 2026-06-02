namespace DealMatcher.Backend.UseCases.Features.Purchase.Initialize;

public sealed record InitializePurchaseResponse(
    [property: JsonPropertyName("redirectUrl")]
    string RedirectUrl
);
