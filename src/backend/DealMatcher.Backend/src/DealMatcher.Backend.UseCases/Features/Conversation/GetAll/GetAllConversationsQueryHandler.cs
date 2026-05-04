using DealMatcher.Backend.Core.Aggregates.Conversation.DTOs;
using DealMatcher.Backend.Core.Aggregates.Conversation.Specifications;

namespace DealMatcher.Backend.UseCases.Features.Conversation.GetAll;

public sealed class GetAllConversationsQueryHandler(IRepository<ConversationEntity> conversationRepository, IMapper mapper)
: IQueryHandler<GetAllConversationsQuery, Result<List<ConversationDTO>>>
{
    public async Task<Result<List<ConversationDTO>>> Handle(GetAllConversationsQuery request, CancellationToken cancellationToken)
    {
        var getByUserIdSpec = new ConversationsByUserIdSpec(request.UserId);
        var conversations = await conversationRepository.ListAsync(getByUserIdSpec, cancellationToken);
        var dto = mapper.Map<List<ConversationDTO>>(conversations);
        return Result.Success(dto);
    }
}