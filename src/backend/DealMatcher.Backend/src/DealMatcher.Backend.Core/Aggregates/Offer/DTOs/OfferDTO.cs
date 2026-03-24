namespace DealMatcher.Backend.Core.Aggregates.Offer.DTOs;

public sealed record OfferDTO(
    int Id,
    string Title,
    string Description,
    double Price,
    List<string> ImageUrls,
    int SellerId,
    List<string> Tags,
    int CategoryId,
    List<OfferProperty> Properties,
    int Availability,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
