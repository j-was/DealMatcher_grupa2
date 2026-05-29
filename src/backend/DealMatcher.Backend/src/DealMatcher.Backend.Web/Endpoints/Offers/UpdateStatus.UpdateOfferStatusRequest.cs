namespace DealMatcher.Backend.Web.Endpoints.Offers;

public sealed record UpdateOfferStatusRequest(
    int OfferId,
    string Status
);
