namespace DealMatcher.Backend.UseCases.Mapping.Profiles;

public sealed class OfferProfile : Profile
{
    public OfferProfile()
    {
        CreateMap<OfferEntity, OfferDTO>()
            .ForCtorParam(nameof(OfferDTO.Status),
                opt => opt.MapFrom(src => src.Status.ToString()));
    }
}
