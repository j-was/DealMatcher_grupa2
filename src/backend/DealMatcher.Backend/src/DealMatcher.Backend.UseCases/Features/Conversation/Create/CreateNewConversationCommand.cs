using DealMatcher.Backend.Core.Aggregates.Conversation.DTOs;

namespace DealMatcher.Backend.UseCases.Features.Conversation.Create;

public sealed record CreateNewConversationCommand(
    int OfferId,
    int BuyerId,
    string Message
) : ICommand<Result<ConversationDTO>>;
