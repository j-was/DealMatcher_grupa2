namespace DealMatcher.Backend.UseCases.Mapping.Profiles;

public sealed class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserEntity, UserDTO>()
            .ForCtorParam(nameof(UserDTO.Id), opt => opt.MapFrom(src => src.Id))
            .ForCtorParam(nameof(UserDTO.Email), opt => opt.MapFrom(src => src.Email))
            .ForCtorParam(nameof(UserDTO.Name), opt => opt.MapFrom(src => src.Name))
            .ForCtorParam(nameof(UserDTO.Surname), opt => opt.MapFrom(src => src.Surname))
            .ForCtorParam(nameof(UserDTO.Status), opt => opt.MapFrom(src => src.Status.Value.ToUpper()))
            .ForCtorParam(nameof(UserDTO.CreatedAt), opt => opt.MapFrom(src => src.CreatedAt));
    }
}
