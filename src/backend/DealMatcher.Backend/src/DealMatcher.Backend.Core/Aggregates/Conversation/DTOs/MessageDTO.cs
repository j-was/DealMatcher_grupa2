namespace DealMatcher.Backend.Core.Aggregates.Conversation.DTOs;

public sealed record MessageDTO(
    int Id,
    int SenderId,
    string Content,
    string Status,
    DateTime CreatedAt
);
