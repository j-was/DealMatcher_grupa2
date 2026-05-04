using DealMatcher.Backend.Core.Aggregates.Conversation.DTOs;
using DealMatcher.Backend.Core.Aggregates.Conversation.Specifications;

namespace DealMatcher.Backend.UseCases.Features.Conversation.Get;

public sealed class GetConversationQueryHandler(IRepository<ConversationEntity> conversationRepository, IMapper mapper)
: IQueryHandler<GetConversationQuery, Result<ConversationDetailsDTO>>
{
    async Task<Result<ConversationDetailsDTO>> IRequestHandler<GetConversationQuery, Result<ConversationDetailsDTO>>.Handle(GetConversationQuery request, CancellationToken cancellationToken)
    {
        var spec = new ConversationDetailsByIdSpec(request.ConversationId);
        var conversationDetails = await conversationRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (conversationDetails is null)
        {
            return Result.NotFound("Conversation details not found");
        }

        var dto = mapper.Map<ConversationDetailsDTO>(conversationDetails);

        return Result.Success(dto);
    }
}