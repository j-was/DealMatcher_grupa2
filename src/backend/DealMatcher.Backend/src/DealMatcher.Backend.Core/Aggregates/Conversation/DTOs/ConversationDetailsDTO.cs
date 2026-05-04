namespace DealMatcher.Backend.Core.Aggregates.Conversation.DTOs;

public sealed record ConversationDetailsDTO(
    int Id,
    int OfferId,
    int BuyerId,
    int SellerId,
    string LastMessage,
    DateTime LastMessageAt,
    int UnreadCount,
    string Status,
    DateTime CreatedAt,
    List<MessageDTO> Messages
);