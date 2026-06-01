namespace DealMatcher.Backend.UnitTests.UseCases.Features.Cart;

public class GetMyCartQueryHandlerTests
{
    private readonly IReadRepository<CartItemEntity> _cartItemsRepository;
    private readonly IReadRepository<OfferEntity> _offersRepository;
    private readonly IMapper _mapper;
    private readonly GetMyCartQueryHandler _handler;

    public GetMyCartQueryHandlerTests()
    {
        _cartItemsRepository = Substitute.For<IReadRepository<CartItemEntity>>();
        _offersRepository = Substitute.For<IReadRepository<OfferEntity>>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetMyCartQueryHandler(_cartItemsRepository, _offersRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenCartIsEmpty()
    {
        _cartItemsRepository
            .ListAsync(Arg.Any<CartItemsByUserIdSpec>(), Arg.Any<CancellationToken>())
            .Returns([]);

        var result = await _handler.Handle(new GetMyCartQuery(1), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeEmpty();

        _mapper.DidNotReceiveWithAnyArgs().Map<CartItemDTO>(default!);
    }

    [Fact]
    public async Task Handle_ShouldReturnCartItems_WhenCartHasItems()
    {
        var cartItems = new List<CartItemEntity>
        {
            CreateCartItem(userId: 1, offerId: 10, quantity: 2), CreateCartItem(userId: 1, offerId: 20, quantity: 1)
        };

        var offer1 = CreateOfferEntity(10);
        var offer2 = CreateOfferEntity(20);

        _cartItemsRepository
            .ListAsync(Arg.Any<CartItemsByUserIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(cartItems);

        _offersRepository
            .GetByIdAsync(10, Arg.Any<CancellationToken>())
            .Returns(offer1);
        _offersRepository
            .GetByIdAsync(20, Arg.Any<CancellationToken>())
            .Returns(offer2);

        _mapper
            .Map<CartItemDTO>(Arg.Any<CartItemProfile.CartItemInfo>())
            .Returns(default(CartItemDTO));

        var result = await _handler.Handle(new GetMyCartQuery(1), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Count.ShouldBe(2);

        _mapper.Received(2).Map<CartItemDTO>(Arg.Any<CartItemProfile.CartItemInfo>());
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken()
    {
        var cts = new CancellationTokenSource();

        _cartItemsRepository
            .ListAsync(Arg.Any<CartItemsByUserIdSpec>(), cts.Token)
            .Returns([]);

        await _handler.Handle(new GetMyCartQuery(1), cts.Token);

        await _cartItemsRepository.Received(1).ListAsync(Arg.Any<CartItemsByUserIdSpec>(), cts.Token);
    }

    private static CartItemEntity CreateCartItem(int userId, int offerId, int quantity)
    {
        return (CartItemEntity)Activator.CreateInstance(typeof(CartItemEntity), userId, offerId, quantity)!;
    }

    private static OfferEntity CreateOfferEntity(int id)
    {
        var offer = Activator.CreateInstance(typeof(OfferEntity), nonPublic: true) as OfferEntity
                    ?? throw new InvalidOperationException("Nie udało się utworzyć OfferEntity.");

        var property = typeof(OfferEntity).GetProperty(nameof(OfferEntity.Id),
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (property?.SetMethod is not null)
        {
            property.SetValue(offer, id);
            return offer;
        }

        var field = typeof(OfferEntity).GetField($"<{nameof(OfferEntity.Id)}>k__BackingField",
                        BindingFlags.Instance | BindingFlags.NonPublic)
                    ?? typeof(OfferEntity).GetField(nameof(OfferEntity.Id),
                        BindingFlags.Instance | BindingFlags.NonPublic)
                    ?? throw new InvalidOperationException($"Nie udało się ustawić Id na OfferEntity.");

        field.SetValue(offer, id);
        return offer;
    }
}
