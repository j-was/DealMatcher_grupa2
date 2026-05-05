using DealMatcher.Backend.Core.Aggregates.Payment;
using DealMatcher.Backend.Core.Aggregates.Payment.DTOs;

namespace DealMatcher.Backend.UseCases.Mapping.Profiles;

public sealed class PaymentMethodProfile : Profile
{
    public PaymentMethodProfile()
    {
        CreateMap<PaymentMethod, PaymentMethodDTO>()
            .ForCtorParam(nameof(PaymentMethodDTO.Id), 
                opt => opt.MapFrom(src => src.StringId))
            .ForCtorParam(nameof(PaymentMethodDTO.Name), 
                opt => opt.MapFrom(src => src.Name))
            .ForCtorParam(nameof(PaymentMethodDTO.Provider), 
                opt => opt.MapFrom(src => src.Provider))
            .ForCtorParam(nameof(PaymentMethodDTO.Icon), 
                opt => opt.MapFrom(src => src.Icon));
    }
}