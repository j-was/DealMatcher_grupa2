using DealMatcher.Backend.Core.Aggregates.Conversation.DTOs;

namespace DealMatcher.Backend.UseCases.Features.Conversation.Get;

public sealed record GetConversationQuery(
    int ConversationId
) : IQuery<Result<ConversationDetailsDTO>>;
