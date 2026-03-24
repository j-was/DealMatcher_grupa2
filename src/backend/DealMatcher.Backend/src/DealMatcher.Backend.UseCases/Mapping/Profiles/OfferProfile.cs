using Microsoft.AspNetCore.Identity;

namespace DealMatcher.Backend.UseCases.Mapping.Profiles;

public sealed class OfferProfile : Profile
{
    public sealed record OfferInfo(OfferEntity Offer, UserEntity Seller, CategoryEntity Category);

    public OfferProfile()
    {
        CreateMap<OfferEntity, OfferDTO>()
            .ForCtorParam(nameof(OfferDTO.Images),
                opt => opt.MapFrom(src => src.ImageUrls))
            .ForCtorParam(nameof(OfferDTO.Status),
                opt => opt.MapFrom(src => src.Status.ToString().ToUpper()))
            .ForCtorParam(nameof(OfferDTO.Seller),
                opt => opt.MapFrom(src => new SellerDTO(src.SellerId, string.Empty, 0f)))
            .ForCtorParam(nameof(OfferDTO.Category),
                opt => opt.MapFrom(src =>
                    new CategoryDTO(src.CategoryId, string.Empty, string.Empty)))
            .ForCtorParam(nameof(OfferDTO.Properties),
                opt => opt.MapFrom(src =>
                    src.Properties.ToDictionary(p => p.Name, p => p.Value)));

        CreateMap<OfferInfo, OfferDTO>()
            .ForCtorParam(nameof(OfferDTO.Images),
                opt => opt.MapFrom(src => src.Offer.ImageUrls))
            .ForCtorParam(nameof(OfferDTO.Status),
                opt => opt.MapFrom(src => src.Offer.Status.ToString().ToUpper()))
            .ForCtorParam(nameof(OfferDTO.Seller),
                opt => opt.MapFrom(src => new SellerDTO(src.Seller.Id, src.Seller.Name, src.Seller.Rating)))
            .ForCtorParam(nameof(OfferDTO.Category),
                opt => opt.MapFrom(src =>
                    new CategoryDTO(src.Category.Id, src.Category.Name, src.Category.Description)))
            .ForCtorParam(nameof(OfferDTO.Properties),
                opt => opt.MapFrom(src =>
                    src.Offer.Properties.ToDictionary(p => p.Name, p => p.Value)));
    }
}
