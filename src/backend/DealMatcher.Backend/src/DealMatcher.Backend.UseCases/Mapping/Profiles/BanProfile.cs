using DealMatcher.Backend.Core.Aggregates.Ban.DTOs;

namespace DealMatcher.Backend.UseCases.Mapping.Profiles;

public sealed class BanProfile : Profile
{
    public BanProfile()
    {
        CreateMap<BanEntity, BanDTO>()
            .ForCtorParam(nameof(BanDTO.Id),
                opt => opt.MapFrom(src => src.Id))
            .ForCtorParam(nameof(BanDTO.UserId),
                opt => opt.MapFrom(src => src.UserId))
            .ForCtorParam(nameof(BanDTO.Reason),
                opt => opt.MapFrom(src => src.Reason))
            .ForCtorParam(nameof(BanDTO.IssuedBy),
                opt => opt.MapFrom(src => src.IssuedBy))
            .ForCtorParam(nameof(BanDTO.IssuedAt),
                opt => opt.MapFrom(src => src.IssuedAt))
            .ForCtorParam(nameof(BanDTO.ExpiresAt),
                opt => opt.MapFrom(src => src.ExpiresAt))
            .ForCtorParam(nameof(BanDTO.IsActive),
                opt => opt.MapFrom(src => src.IsActive));
    }
}
