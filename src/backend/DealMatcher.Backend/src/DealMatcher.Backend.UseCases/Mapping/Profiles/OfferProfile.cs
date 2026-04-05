using Microsoft.AspNetCore.Identity;

namespace DealMatcher.Backend.UseCases.Mapping.Profiles;

public sealed class OfferProfile : Profile
{
    public sealed record OfferInfo(OfferEntity Offer, UserEntity Seller, CategoryEntity Category);

    public OfferProfile()
    {
        CreateMap<OfferEntity, OfferDTO>()
            .ForCtorParam(nameof(OfferDTO.Id), opt => opt.MapFrom(src => src.Id))
            .ForCtorParam(nameof(OfferDTO.Title), opt => opt.MapFrom(src => src.Title))
            .ForCtorParam(nameof(OfferDTO.Description), opt => opt.MapFrom(src => src.Description))
            .ForCtorParam(nameof(OfferDTO.Price), opt => opt.MapFrom(src => (double)src.Price))
            .ForCtorParam(nameof(OfferDTO.Images), opt => opt.MapFrom(src => src.ImageUrls))
            .ForCtorParam(nameof(OfferDTO.Seller), opt => opt.MapFrom(src => new SellerDTO(src.SellerId, "", 0f)))
            .ForCtorParam(nameof(OfferDTO.Tags), opt => opt.MapFrom(src => src.Tags))
            .ForCtorParam(nameof(OfferDTO.Category), opt => opt.MapFrom(src => new CategoryDTO(src.CategoryId, "", "", new List<CategoryPropertyDTO>())))
            .ForCtorParam(nameof(OfferDTO.Properties), opt => opt.MapFrom(src => src.Properties.ToDictionary(p => p.Name, p => p.Value)))
            .ForCtorParam(nameof(OfferDTO.Availability), opt => opt.MapFrom(src => src.Availability))
            .ForCtorParam(nameof(OfferDTO.Status), opt => opt.MapFrom(src => src.Status.Value.ToUpper()))
            .ForCtorParam(nameof(OfferDTO.CreatedAt), opt => opt.MapFrom(src => src.CreatedAt))
            .ForCtorParam(nameof(OfferDTO.UpdatedAt), opt => opt.MapFrom(src => src.UpdatedAt));

        CreateMap<OfferInfo, OfferDTO>()
            .ForCtorParam(nameof(OfferDTO.Id),
                opt => opt.MapFrom(src => src.Offer.Id))
            .ForCtorParam(nameof(OfferDTO.Title),
                opt => opt.MapFrom(src => src.Offer.Title))
            .ForCtorParam(nameof(OfferDTO.Description),
                opt => opt.MapFrom(src => src.Offer.Description))
            .ForCtorParam(nameof(OfferDTO.Price),
                opt => opt.MapFrom(src => (double)src.Offer.Price))
            .ForCtorParam(nameof(OfferDTO.Images),
                opt => opt.MapFrom(src => src.Offer.ImageUrls))
            .ForCtorParam(nameof(OfferDTO.Seller),
                opt => opt.MapFrom(src =>
                    new SellerDTO(src.Seller.Id, src.Seller.Name, src.Seller.Rating)))
            .ForCtorParam(nameof(OfferDTO.Tags),
                opt => opt.MapFrom(src => src.Offer.Tags))
            .ForCtorParam(nameof(OfferDTO.Category),
                opt => opt.MapFrom(src =>
                    new CategoryDTO(src.Category.Id, src.Category.Name, src.Category.Description, src.Category.Properties
                .Select(p => new CategoryPropertyDTO(p.Id, p.Name, p.Description))
                .ToList())))
            .ForCtorParam(nameof(OfferDTO.Properties),
                opt => opt.MapFrom(src =>
                    src.Offer.Properties.ToDictionary(p => p.Name, p => p.Value)))
            .ForCtorParam(nameof(OfferDTO.Availability),
                opt => opt.MapFrom(src => src.Offer.Availability))
            .ForCtorParam(nameof(OfferDTO.Status),
                opt => opt.MapFrom(src => src.Offer.Status.Value.ToUpper()))
            .ForCtorParam(nameof(OfferDTO.CreatedAt),
                opt => opt.MapFrom(src => src.Offer.CreatedAt))
            .ForCtorParam(nameof(OfferDTO.UpdatedAt),
                opt => opt.MapFrom(src => src.Offer.UpdatedAt));
    }
}
