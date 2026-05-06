using DealMatcher.Backend.Core.Aggregates.Conversation.DTOs;
using DealMatcher.Backend.Core.Aggregates.Conversation.Specifications;

namespace DealMatcher.Backend.UseCases.Features.Conversation.Create;

public sealed class CreateNewConversationCommandHandler(IRepository<ConversationEntity> conversationRepository, IRepository<OfferEntity> offerRepository, IMapper mapper)
: ICommandHandler<CreateNewConversationCommand, Result<ConversationDTO>>
{
    async Task<Result<ConversationDTO>> IRequestHandler<CreateNewConversationCommand, Result<ConversationDTO>>.Handle(CreateNewConversationCommand request, CancellationToken cancellationToken)
    {
        var offer = await offerRepository.GetByIdAsync(request.OfferId, cancellationToken);

        if (offer is null)
        {
            return Result.NotFound("Offer not found");
        }

        if (offer.SellerId == request.BuyerId)
        {
            return Result.Forbidden("Cannot create conversation with yourself");
        }

        var existingSpec = new ExistingConversationSpec(offer.Id, offer.SellerId, request.BuyerId);
        var existingConv = await conversationRepository.FirstOrDefaultAsync(existingSpec, cancellationToken);
        if (existingConv is not null)
        {
            return Result.Conflict("Conversation for this offer already exists");
        }

        var newConversation = new ConversationEntity(request.OfferId, request.BuyerId, offer!.SellerId, request.Message);

        await conversationRepository.AddAsync(newConversation, cancellationToken);
        await conversationRepository.SaveChangesAsync(cancellationToken);

        var dto = mapper.Map<ConversationDTO>(newConversation);

        return Result.Created(dto);
    }
}
