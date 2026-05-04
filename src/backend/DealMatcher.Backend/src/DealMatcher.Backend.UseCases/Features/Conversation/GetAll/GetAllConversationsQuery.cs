using DealMatcher.Backend.Core.Aggregates.Conversation.DTOs;

namespace DealMatcher.Backend.UseCases.Features.Conversation.GetAll;

public sealed record GetAllConversationsQuery(
    int UserId
) : IQuery<Result<List<ConversationDTO>>>;
