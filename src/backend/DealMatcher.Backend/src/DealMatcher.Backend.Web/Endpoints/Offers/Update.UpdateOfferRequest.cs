namespace DealMatcher.Backend.Web.Endpoints.Offers;

public sealed record UpdateOfferRequest(
    int OfferId,
    string? Title,
    string? Description,
    double? Price,
    List<string>? Images,
    List<string>? Tags,
    Dictionary<string, object>? Properties,
    int? Availability
);
