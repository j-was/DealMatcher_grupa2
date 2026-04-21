namespace DealMatcher.Backend.UseCases.Mapping.Profiles;

public sealed class CartItemProfile : Profile
{
    public sealed record CartItemInfo(CartItem Item, OfferEntity Offer);

    public  CartItemProfile()
    {
        CreateMap<CartItemInfo, CartItemDTO>()
            .ForCtorParam(nameof(CartItemDTO.Id), opt => opt.MapFrom(src => src.Item.Id))
            .ForCtorParam(nameof(CartItemDTO.Quantity), opt => opt.MapFrom(src => src.Item.Quantity))
            .ForCtorParam(nameof(CartItemDTO.AddedAt), opt => opt.MapFrom(src => src.Item.AddedAt))
            .ForCtorParam(nameof(CartItemDTO.Offer), opt => opt.MapFrom(src => src.Offer));

    }
}
