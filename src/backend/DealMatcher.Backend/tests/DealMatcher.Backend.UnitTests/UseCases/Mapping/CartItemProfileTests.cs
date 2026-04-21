namespace DealMatcher.Backend.UnitTests.UseCases.Mapping;

public class CartItemProfileTests
{
    private readonly IMapper _mapper;

    public CartItemProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CartItemProfile>();
            cfg.AddProfile<OfferProfile>();
        }, new SerilogLoggerFactory());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Configuration_IsValid()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CartItemProfile>();
            cfg.AddProfile<OfferProfile>();
        }, new SerilogLoggerFactory());

        configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void Map_CartItemInfoToCartItemDTO_MapsBasicPropertiesCorrectly()
    {
        var cartItem = new CartItemEntity(1, 2, 5);
        typeof(CartItemEntity).GetProperty("Id")!.SetValue(cartItem, 100);

        var offer = CreateOfferEntity();
        offer.GetType().GetProperty("Id")!.SetValue(offer, 2);

        var cartItemInfo = new CartItemProfile.CartItemInfo(cartItem, offer);

        var dto = _mapper.Map<CartItemDTO>(cartItemInfo);

        dto.Id.ShouldBe(100);
        dto.Quantity.ShouldBe(5);
        dto.AddedAt.ShouldBe(cartItem.AddedAt);
    }

    [Fact]
    public void Map_CartItemInfoToCartItemDTO_MapsOfferCorrectly()
    {
        var cartItem = new CartItemEntity(1, 2, 5);
        var offer = CreateOfferEntity();
        offer.GetType().GetProperty("Id")!.SetValue(offer, 2);

        var cartItemInfo = new CartItemProfile.CartItemInfo(cartItem, offer);

        var dto = _mapper.Map<CartItemDTO>(cartItemInfo);

        dto.Offer.ShouldNotBeNull();
        dto.Offer.Id.ShouldBe(2);
        dto.Offer.Title.ShouldBe(offer.Title);
    }

    private static Offer CreateOfferEntity()
    {
        return new Offer(
            "Test Offer",
            "Test Description",
            99.99m,
            ["https://example.com/image.jpg"],
            1,
            ["tag1"],
            1,
            [new OfferProperty("Color", "Red")],
            10
        );
    }
}
