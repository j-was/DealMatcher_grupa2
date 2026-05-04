using DealMatcher.Backend.Core.Aggregates.Conversation.DTOs;

namespace DealMatcher.Backend.UseCases.Mapping.Profiles;

public sealed class ConversationProfile: Profile
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
            .ForCtorParam(nameof(ConversationDTO.OfferId), opt => opt.MapFrom(src => src.OfferId))
            .ForCtorParam(nameof(ConversationDTO.BuyerId), opt => opt.MapFrom(src => src.BuyerId))
            .ForCtorParam(nameof(ConversationDTO.SellerId), opt => opt.MapFrom(src => src.SellerId))
            .ForCtorParam(nameof(ConversationDTO.LastMessage), opt => opt.MapFrom(src => src.LastMessage))
            .ForCtorParam(nameof(ConversationDTO.LastMessageAt), opt => opt.MapFrom(src => src.LastMessageAt))
            .ForCtorParam(nameof(ConversationDTO.UnreadCount), opt => opt.MapFrom(src => src.UnreadCount))
            .ForCtorParam(nameof(ConversationDTO.Status), opt => opt.MapFrom(src => src.Status.ToString()))
            .ForCtorParam(nameof(ConversationDTO.CreatedAt), opt => opt.MapFrom(src => src.CreatedAt));

         CreateMap<ConversationEntity, ConversationDetailsDTO>()
            .ForCtorParam(nameof(ConversationDetailsDTO.Id), opt => opt.MapFrom(src => src.Id))
            .ForCtorParam(nameof(ConversationDetailsDTO.OfferId), opt => opt.MapFrom(src => src.OfferId))
            .ForCtorParam(nameof(ConversationDetailsDTO.BuyerId), opt => opt.MapFrom(src => src.BuyerId))
            .ForCtorParam(nameof(ConversationDetailsDTO.SellerId), opt => opt.MapFrom(src => src.SellerId))
            .ForCtorParam(nameof(ConversationDetailsDTO.LastMessage), opt => opt.MapFrom(src => src.LastMessage))
            .ForCtorParam(nameof(ConversationDetailsDTO.LastMessageAt), opt => opt.MapFrom(src => src.LastMessageAt))
            .ForCtorParam(nameof(ConversationDetailsDTO.UnreadCount), opt => opt.MapFrom(src => src.UnreadCount))
            .ForCtorParam(nameof(ConversationDetailsDTO.Status), opt => opt.MapFrom(src => src.Status.ToString()))
            .ForCtorParam(nameof(ConversationDetailsDTO.CreatedAt), opt => opt.MapFrom(src => src.CreatedAt))
            .ForCtorParam(nameof(ConversationDetailsDTO.Messages), opt => opt.MapFrom(src => src.Messages));
    }
}