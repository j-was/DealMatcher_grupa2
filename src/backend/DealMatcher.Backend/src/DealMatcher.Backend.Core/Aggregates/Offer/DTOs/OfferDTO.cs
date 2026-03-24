using DealMatcher.Backend.Core.Aggregates.Category.DTOs;

namespace DealMatcher.Backend.Core.Aggregates.Offer.DTOs;

public sealed record OfferDTO(
    int Id,
    string Title,
    string Description,
    double Price,
    List<string> Images,
    SellerDTO Seller,
    List<string> Tags,
    CategoryDTO Category,
    Dictionary<string, string> Properties,
    int Availability,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public sealed record SellerDTO(
    int Id,
    string Name,
    float Rating
);
