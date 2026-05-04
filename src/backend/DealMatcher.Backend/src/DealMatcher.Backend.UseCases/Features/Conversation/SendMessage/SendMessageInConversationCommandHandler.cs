using DealMatcher.Backend.Core.Aggregates.Conversation.DTOs;

namespace DealMatcher.Backend.UseCases.Features.Conversation.SendMessage;

public sealed class SendMessageInConversationCommandHandler(IRepository<ConversationEntity> conversationRepository, IMapper mapper)
: ICommandHandler<SendMessageInConversationCommand, Result<MessageDTO>>
{
    public async Task<Result<MessageDTO>> Handle(SendMessageInConversationCommand request, CancellationToken cancellationToken)
    {
        var conversation = await conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken);

        if (conversation is null)
        {
            return Result.NotFound("Conversation not found");
        }

        var message = conversation.AddMessage(request.SenderId, request.Message);

        await conversationRepository.UpdateAsync(conversation, cancellationToken);
        await conversationRepository.SaveChangesAsync(cancellationToken);

        var dto = mapper.Map<MessageDTO>(message);
        return Result.Created(dto);
    }
}