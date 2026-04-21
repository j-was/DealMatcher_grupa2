namespace DealMatcher.Backend.Web.Endpoints.Offers;

public sealed record UpdateOfferRequest(
    string Title,
    string Description,
    double Price,
    List<string> Tags,
    int? CategoryId,
    Dictionary<string, string>? Properties,
    int? Availability
);
