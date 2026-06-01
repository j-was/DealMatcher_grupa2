namespace DealMatcher.Backend.UnitTests.UseCases.Features.Cart;

public class AddToCartCommandHandlerTests
{
    private readonly IRepository<CartItemEntity> _cartItemsRepository;
    private readonly IReadRepository<OfferEntity> _offersRepository;
    private readonly IMapper _mapper;
    private readonly AddToCartCommandHandler _handler;

    public AddToCartCommandHandlerTests()
    {
        _cartItemsRepository = Substitute.For<IRepository<CartItemEntity>>();
        _offersRepository = Substitute.For<IReadRepository<OfferEntity>>();
        _mapper = Substitute.For<IMapper>();
        _handler = new AddToCartCommandHandler(_cartItemsRepository, _offersRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalid_WhenQuantityIsLessThanOne()
    {
        var result = await _handler.Handle(new AddToCartCommand(1, 10, 0), CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors.ShouldContain(e => e.ErrorMessage == "Invalid request");

        await _cartItemsRepository.DidNotReceiveWithAnyArgs().ListAsync(default!, default);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenOfferDoesNotExist()
    {
        _offersRepository
            .GetByIdAsync(10, Arg.Any<CancellationToken>())
            .Returns((OfferEntity?)null);

        var result = await _handler.Handle(new AddToCartCommand(1, 10, 1), CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.Errors.ShouldContain("Offer not found");

        await _cartItemsRepository.DidNotReceiveWithAnyArgs().ListAsync(default!, default);
    }

    [Fact]
    public async Task Handle_ShouldReturnConflict_WhenItemAlreadyInCart()
    {
        var offer = CreateOfferEntity(10);
        var existingCartItems = new List<CartItemEntity> { CreateCartItem(1, 10, 2) };

        _offersRepository
            .GetByIdAsync(10, Arg.Any<CancellationToken>())
            .Returns(offer);

        _cartItemsRepository
            .ListAsync(Arg.Any<CartItemsByUserIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(existingCartItems);

        var result = await _handler.Handle(new AddToCartCommand(1, 10, 1), CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Conflict);
        result.Errors.ShouldContain("Item already in cart");

        await _cartItemsRepository.DidNotReceive().AddAsync(Arg.Any<CartItemEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldAddItemToCart_WhenRequestIsValid()
    {
        var offer = CreateOfferEntity(10);

        _offersRepository
            .GetByIdAsync(10, Arg.Any<CancellationToken>())
            .Returns(offer);

        _cartItemsRepository
            .ListAsync(Arg.Any<CartItemsByUserIdSpec>(), Arg.Any<CancellationToken>())
            .Returns([]);

        _mapper
            .Map<CartItemDTO>(Arg.Any<CartItemProfile.CartItemInfo>())
            .Returns(default(CartItemDTO));

        var result = await _handler.Handle(new AddToCartCommand(1, 10, 3), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Status.ShouldBe(ResultStatus.Created);

        await _cartItemsRepository.Received(1).AddAsync(Arg.Any<CartItemEntity>(), Arg.Any<CancellationToken>());
        await _cartItemsRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<CartItemDTO>(Arg.Any<CartItemProfile.CartItemInfo>());
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken()
    {
        var cts = new CancellationTokenSource();
        var offer = CreateOfferEntity(10);

        _offersRepository
            .GetByIdAsync(10, cts.Token)
            .Returns(offer);

        _cartItemsRepository
            .ListAsync(Arg.Any<CartItemsByUserIdSpec>(), cts.Token)
            .Returns([]);

        _mapper
            .Map<CartItemDTO>(Arg.Any<CartItemProfile.CartItemInfo>())
            .Returns(default(CartItemDTO));

        await _handler.Handle(new AddToCartCommand(1, 10, 2), cts.Token);

        await _offersRepository.Received(1).GetByIdAsync(10, cts.Token);
        await _cartItemsRepository.Received(1).ListAsync(Arg.Any<CartItemsByUserIdSpec>(), cts.Token);
        await _cartItemsRepository.Received(1).AddAsync(Arg.Any<CartItemEntity>(), cts.Token);
    }

    private static OfferEntity CreateOfferEntity(int id)
    {
        var offer = (OfferEntity)Activator.CreateInstance(typeof(OfferEntity), nonPublic: true)!;
        SetMemberValue(offer, nameof(OfferEntity.Id), id);
        return offer;
    }

    private static CartItemEntity CreateCartItem(int userId, int offerId, int quantity)
    {
        return (CartItemEntity)Activator.CreateInstance(typeof(CartItemEntity), userId, offerId, quantity)!;
    }

    private static void SetMemberValue(object target, string memberName, object value)
    {
        var type = target.GetType();
        var property =
            type.GetProperty(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
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
