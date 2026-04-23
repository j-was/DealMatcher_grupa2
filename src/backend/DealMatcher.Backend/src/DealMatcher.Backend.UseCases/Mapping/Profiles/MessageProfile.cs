namespace DealMatcher.Backend.UseCases.Mapping.Profiles;

public sealed class MessageProfile:Profile
{
    public MessageProfile()
    {
        CreateMap<Message, MessageDTO>()
            .ForCtorParam(nameof(MessageDTO.Id), opt => opt.MapFrom(src => src.Id))
            .ForCtorParam(nameof(MessageDTO.Content), opt => opt.MapFrom(src => src.Content))
            .ForCtorParam(nameof(MessageDTO.SenderId), opt => opt.MapFrom(src => src.SenderId))
            .ForCtorParam(nameof(MessageDTO.Status), opt => opt.MapFrom(src => src.Status.Name.ToUpper()))
            .ForCtorParam(nameof(MessageDTO.CreatedAt), opt => opt.MapFrom(src => src.CreatedAt));
    }
}
