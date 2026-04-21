using DealMatcher.Backend.Core.Aggregates.Offer.DTOs;

namespace DealMatcher.Backend.Core.Aggregates.Cart.DTOs;

public sealed record CartItemDTO(
    int Id,
    OfferDTO Offer,
    int Quantity,
    DateTime AddedAt);
