using DealMatcher.Backend.Core.Aggregates.Message.DTOs;
using DealMatcher.Backend.Core.Aggregates.Offer.DTOs;

namespace DealMatcher.Backend.Core.Aggregates.Conversation.DTOs;

/// <summary>
/// Convesation detail schema in Swagger spec
/// </summary>
public sealed record ConversationDTO(
    int Id,
    OfferDTO Offer,
    ConversationUserDTO Buyer,
    ConversationUserDTO Seller,
    string LastMessage,
    DateTime LastMessageAt,
    int UnreadCount,
    string Status,
    DateTime CreatedAt,
    List<MessageDTO> Messages
);
