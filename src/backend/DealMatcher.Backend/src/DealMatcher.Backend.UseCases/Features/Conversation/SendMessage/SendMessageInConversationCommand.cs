using DealMatcher.Backend.Core.Aggregates.Conversation.DTOs;

namespace DealMatcher.Backend.UseCases.Features.Conversation.SendMessage;

public sealed record SendMessageInConversationCommand(
    int ConversationId,
    int SenderId,
    string Message
) : ICommand<Result<MessageDTO>>;
