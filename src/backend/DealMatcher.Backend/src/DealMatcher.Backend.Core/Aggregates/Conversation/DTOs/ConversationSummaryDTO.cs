using DealMatcher.Backend.Core.Aggregates.Offer.DTOs;

namespace DealMatcher.Backend.Core.Aggregates.Conversation.DTOs;

/// <summary>
/// Conversation schema in Swagger spec
/// </summary>
public sealed record ConversationSummaryDTO(
    int Id,
    OfferDTO Offer,
    ConversationUserDTO Buyer,
    ConversationUserDTO Seller,
    string LastMessage,
    DateTime LastMessageAt,
    int UnreadCount,
    string Status,
    DateTime CreatedAt
);
