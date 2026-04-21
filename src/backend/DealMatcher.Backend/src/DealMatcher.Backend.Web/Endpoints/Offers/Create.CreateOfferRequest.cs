namespace DealMatcher.Backend.Web.Endpoints.Offers;

public sealed record CreateOfferRequest(
    string Data,
    List<IFormFile> Images
);
