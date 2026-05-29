using System.Text.Json.Serialization;

namespace DealMatcher.Backend.UseCases.Features.Purchase.Initialize;

public sealed record InitializePurchaseResponse(
    [property: JsonPropertyName("redirectUrl")]
    string RedirectUrl
);