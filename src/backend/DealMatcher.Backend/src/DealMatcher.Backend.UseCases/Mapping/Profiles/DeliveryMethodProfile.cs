using DealMatcher.Backend.Core.Aggregates.Payment;
using DealMatcher.Backend.Core.Aggregates.Payment.DTOs;

namespace DealMatcher.Backend.UseCases.Mapping.Profiles;

public sealed class DeliveryMethodProfile : Profile
{
    public DeliveryMethodProfile()
    {
        CreateMap<DeliveryMethod, DeliveryMethodDTO>()
            .ForCtorParam(nameof(DeliveryMethodDTO.Id),
                opt => opt.MapFrom(src => src.StringId))
            .ForCtorParam(nameof(DeliveryMethodDTO.Name),
                opt => opt.MapFrom(src => src.Name))
            .ForCtorParam(nameof(DeliveryMethodDTO.Description),
                opt => opt.MapFrom(src => src.Description))
            .ForCtorParam(nameof(DeliveryMethodDTO.Price),
                opt => opt.MapFrom(src => (double)src.Price))
            .ForCtorParam(nameof(DeliveryMethodDTO.EstimatedDays),
                opt => opt.MapFrom(src => src.EstimatedDays));
    }
}
