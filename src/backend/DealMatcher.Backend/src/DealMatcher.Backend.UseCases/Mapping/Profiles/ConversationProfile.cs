using DealMatcher.Backend.Core.Aggregates.Conversation.DTOs;

namespace DealMatcher.Backend.UseCases.Mapping.Profiles;

public sealed class ConversationProfile : Profile
{
    public ConversationProfile()
    {
        CreateMap<MessageEntity, MessageDTO>()
            .ForCtorParam(nameof(MessageDTO.Id), opt => opt.MapFrom(src => src.Id))
            .ForCtorParam(nameof(MessageDTO.SenderId), opt => opt.MapFrom(src => src.SenderId))
            .ForCtorParam(nameof(MessageDTO.Content), opt => opt.MapFrom(src => src.Content))
            .ForCtorParam(nameof(MessageDTO.Status), opt => opt.MapFrom(src => src.Status))
            .ForCtorParam(nameof(MessageDTO.CreatedAt), opt => opt.MapFrom(src => src.CreatedAt));

        CreateMap<ConversationEntity, ConversationDTO>()
            .ForCtorParam(nameof(ConversationDTO.Id), opt => opt.MapFrom(src => src.Id))
            .ForCtorParam(nameof(ConversationDTO.Offer), opt => opt.MapFrom(src => src.Offer))
            .ForCtorParam(
                  nameof(ConversationDTO.Buyer),
                  opt => opt.MapFrom(src => new ConversationUserDTO(src.Buyer.Id, src.Buyer.Name))
              )
              .ForCtorParam(
                  nameof(ConversationDTO.Seller),
                  opt => opt.MapFrom(src => new ConversationUserDTO(src.Seller.Id, src.Seller.Name))
              )
            .ForCtorParam(nameof(ConversationDTO.LastMessage), opt => opt.MapFrom(src => src.LastMessage))
            .ForCtorParam(nameof(ConversationDTO.LastMessageAt), opt => opt.MapFrom(src => src.LastMessageAt))
            .ForCtorParam(nameof(ConversationDTO.UnreadCount), opt => opt.MapFrom(src => src.UnreadCount))
            .ForCtorParam(nameof(ConversationDTO.Status), opt => opt.MapFrom(src => src.Status.ToString()))
            .ForCtorParam(nameof(ConversationDTO.CreatedAt), opt => opt.MapFrom(src => src.CreatedAt));

        CreateMap<ConversationEntity, ConversationDetailsDTO>()
           .ForCtorParam(nameof(ConversationDetailsDTO.Id), opt => opt.MapFrom(src => src.Id))
           .ForCtorParam(nameof(ConversationDetailsDTO.Offer), opt => opt.MapFrom(src => src.Offer))
           .ForCtorParam(
                 nameof(ConversationDetailsDTO.Buyer),
                 opt => opt.MapFrom(src => new ConversationUserDTO(src.Buyer.Id, src.Buyer.Name))
             )
             .ForCtorParam(
                 nameof(ConversationDetailsDTO.Seller),
                 opt => opt.MapFrom(src => new ConversationUserDTO(src.Seller.Id, src.Seller.Name))
             )
           .ForCtorParam(nameof(ConversationDetailsDTO.LastMessage), opt => opt.MapFrom(src => src.LastMessage))
           .ForCtorParam(nameof(ConversationDetailsDTO.LastMessageAt), opt => opt.MapFrom(src => src.LastMessageAt))
           .ForCtorParam(nameof(ConversationDetailsDTO.UnreadCount), opt => opt.MapFrom(src => src.UnreadCount))
           .ForCtorParam(nameof(ConversationDetailsDTO.Status), opt => opt.MapFrom(src => src.Status.ToString()))
           .ForCtorParam(nameof(ConversationDetailsDTO.CreatedAt), opt => opt.MapFrom(src => src.CreatedAt))
           .ForCtorParam(nameof(ConversationDetailsDTO.Messages), opt => opt.MapFrom(src => src.Messages));
    }
}
