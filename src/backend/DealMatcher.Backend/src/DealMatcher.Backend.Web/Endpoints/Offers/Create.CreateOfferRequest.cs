namespace DealMatcher.Backend.Web.Endpoints.Offers;

 public sealed record CreateOfferRequest(
      string Title,
      string Description,
      double Price,
      string Tags,
      int CategoryId,
      string Properties,
      int Availability,
      List<IFormFile> Images
  );
