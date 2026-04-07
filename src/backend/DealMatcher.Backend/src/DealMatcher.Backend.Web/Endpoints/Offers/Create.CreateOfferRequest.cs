namespace DealMatcher.Backend.Web.Endpoints.Offers;

public sealed record CreateOfferRequest(
    string Title,
    string Description,
    double Price,
    List<string> Images,
    List<string> Tags,
    int CategoryId,
    Dictionary<string, string> Properties,
    int Availability
);
