namespace DealMatcher.Backend.UnitTests.UseCases.Features.Cart;

public class GetCartTotalQueryHandlerTests
{
    private readonly IReadRepository<CartItemEntity> _cartItemsRepository;
    private readonly IReadRepository<OfferEntity> _offersRepository;
    private readonly GetCartTotalQueryHandler _handler;

    public GetCartTotalQueryHandlerTests()
    {
        _cartItemsRepository = Substitute.For<IReadRepository<CartItemEntity>>();
        _offersRepository = Substitute.For<IReadRepository<OfferEntity>>();
        _handler = new GetCartTotalQueryHandler(_cartItemsRepository, _offersRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnZero_WhenCartIsEmpty()
    {
        _cartItemsRepository
            .ListAsync(Arg.Any<CartItemsByUserIdSpec>(), Arg.Any<CancellationToken>())
            .Returns([]);

        var result = await _handler.Handle(new GetCartTotalQuery(1), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.TotalPrice.ShouldBe(0);
        result.Value.Currency.ShouldBe("PLN");
    }

    [Fact]
    public async Task Handle_ShouldCalculateTotal_WhenCartHasItems()
    {
        var cartItems = new List<CartItemEntity>
        {
            CreateCartItem(userId: 1, offerId: 10, quantity: 2),
            CreateCartItem(userId: 1, offerId: 20, quantity: 1)
        };

        var offer1 = CreateOfferEntity(10, price: 49.99m);
        var offer2 = CreateOfferEntity(20, price: 150.00m);

        _cartItemsRepository
            .ListAsync(Arg.Any<CartItemsByUserIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(cartItems);

        _offersRepository
            .GetByIdAsync(10, Arg.Any<CancellationToken>())
            .Returns(offer1);
        _offersRepository
            .GetByIdAsync(20, Arg.Any<CancellationToken>())
            .Returns(offer2);

        var result = await _handler.Handle(new GetCartTotalQuery(1), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.TotalPrice.ShouldBe(249.98);
    }

    [Fact]
    public async Task Handle_ShouldSkipMissingOffers()
    {
        var cartItems = new List<CartItemEntity>
        {
            CreateCartItem(userId: 1, offerId: 10, quantity: 2)
        };

        _cartItemsRepository
            .ListAsync(Arg.Any<CartItemsByUserIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(cartItems);

        _offersRepository
            .GetByIdAsync(10, Arg.Any<CancellationToken>())
            .Returns((OfferEntity?)null);

        var result = await _handler.Handle(new GetCartTotalQuery(1), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.TotalPrice.ShouldBe(0);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken()
    {
        var cts = new CancellationTokenSource();

        _cartItemsRepository
            .ListAsync(Arg.Any<CartItemsByUserIdSpec>(), cts.Token)
            .Returns([]);

        await _handler.Handle(new GetCartTotalQuery(1), cts.Token);

        await _cartItemsRepository.Received(1).ListAsync(Arg.Any<CartItemsByUserIdSpec>(), cts.Token);
    }

    private static CartItemEntity CreateCartItem(int userId, int offerId, int quantity)
    {
        return (CartItemEntity)Activator.CreateInstance(typeof(CartItemEntity), userId, offerId, quantity)!;
    }

    private static OfferEntity CreateOfferEntity(int id, decimal price)
    {
        var offer = (OfferEntity)Activator.CreateInstance(typeof(OfferEntity), nonPublic: true)!;
        SetMemberValue(offer, nameof(OfferEntity.Id), id);
        SetMemberValue(offer, nameof(OfferEntity.Price), price);
        return offer;
    }

    private static void SetMemberValue(object target, string memberName, object value)
    {
        var type = target.GetType();
        var property = type.GetProperty(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (property?.SetMethod is not null)
        {
            property.SetValue(target, value);
            return;
        }

        var field = type.GetField($"<{memberName}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? type.GetField(memberName, BindingFlags.Instance | BindingFlags.NonPublic);
        field!.SetValue(target, value);
    }
}
