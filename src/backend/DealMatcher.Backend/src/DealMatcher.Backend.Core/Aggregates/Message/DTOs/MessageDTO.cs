namespace DealMatcher.Backend.Core.Aggregates.Message.DTOs;

public sealed record MessageDTO(
int Id,
int SenderId,
string Content,
string Status,
DateTime CreatedAt
);
