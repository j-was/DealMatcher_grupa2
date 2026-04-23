namespace DealMatcher.Backend.UseCases.Mapping.Profiles;

public sealed class ConversationProfile:Profile
{
    public sealed record ConversationInfo(Conversation Conversation, OfferProfile.OfferInfo Offer, List<Message> Messages, UserEntity Seller, UserEntity Buyer);

    public ConversationProfile()
    {
        CreateMap<ConversationInfo, ConversationDTO>()
            .ForCtorParam(nameof(ConversationDTO.Id), opt => opt.MapFrom(src => src.Conversation.Id))
            .ForCtorParam(nameof(ConversationDTO.Offer), opt => opt.MapFrom(src => src.Offer))
            .ForCtorParam(nameof(ConversationDTO.Buyer), opt => opt.MapFrom(src =>
                new ConversationUserDTO(src.Buyer.Id, $"{src.Buyer.Name} {src.Buyer.Surname}".TrimEnd())))
            .ForCtorParam(nameof(ConversationDTO.Seller), opt => opt.MapFrom(src =>
                new ConversationUserDTO(src.Seller.Id, $"{src.Seller.Name} {src.Seller.Surname}".TrimEnd())))
            .ForCtorParam(nameof(ConversationDTO.LastMessage), opt => opt.MapFrom(src =>
                src.Messages.OrderByDescending(m => m.CreatedAt).Select(m => m.Content).FirstOrDefault() ?? ""))
            .ForCtorParam(nameof(ConversationDTO.LastMessageAt), opt => opt.MapFrom(src =>
                src.Messages.OrderByDescending(m => m.CreatedAt).Select(m => m.CreatedAt).FirstOrDefault()))
            .ForCtorParam(nameof(ConversationDTO.UnreadCount), opt => opt.MapFrom(src =>
                src.Messages.Count(m => m.Status != MessageStatus.Read)))
            .ForCtorParam(nameof(ConversationDTO.Status), opt => opt.MapFrom(src => src.Conversation.Status.Name.ToUpper()))
    }

}
