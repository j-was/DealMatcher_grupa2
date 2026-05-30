using DealMatcher.Backend.Core.Aggregates.Offer.DTOs;

namespace DealMatcher.Backend.Core.Aggregates.Conversation.DTOs;

public sealed record ConversationDTO(
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
