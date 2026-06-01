namespace DealMatcher.Backend.UnitTests.UseCases.Features.Cart;

public class DeleteCartItemCommandHandlerTests
{
    private readonly IRepository<CartItemEntity> _cartRepository;
    private readonly DeleteCartItemHandler _handler;

    public DeleteCartItemCommandHandlerTests()
    {
        _cartRepository = Substitute.For<IRepository<CartItemEntity>>();
        _handler = new DeleteCartItemHandler(_cartRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenCartItemDoesNotExist()
    {
        _cartRepository
            .GetByIdAsync(99, Arg.Any<CancellationToken>())
            .Returns((CartItemEntity?)null);

        var result = await _handler.Handle(new DeleteCartItemCommand(99, 1), CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.Errors.ShouldContain("Cart item not found");

        await _cartRepository.DidNotReceive().UpdateAsync(Arg.Any<CartItemEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnForbidden_WhenUserDoesNotOwnCartItem()
    {
        var cartItem = CreateCartItem(userId: 2, offerId: 10, quantity: 1);
        SetMemberValue(cartItem, "Id", 5);

        _cartRepository
            .GetByIdAsync(5, Arg.Any<CancellationToken>())
            .Returns(cartItem);

        var result = await _handler.Handle(new DeleteCartItemCommand(1, 5), CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Forbidden);
        result.Errors.ShouldContain("Forbidden - not your cart item");

        await _cartRepository.DidNotReceive().UpdateAsync(Arg.Any<CartItemEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldDeleteCartItem_WhenUserOwnsIt()
    {
        var cartItem = CreateCartItem(userId: 1, offerId: 10, quantity: 1);
        SetMemberValue(cartItem, "Id", 5);
        _cartRepository
            .GetByIdAsync(5, Arg.Any<CancellationToken>())
            .Returns(cartItem);

        var result = await _handler.Handle(new DeleteCartItemCommand(1, 5), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();

        await _cartRepository.Received(1).UpdateAsync(cartItem, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken()
    {
        var cts = new CancellationTokenSource();
        var cartItem = CreateCartItem(userId: 1, offerId: 10, quantity: 1);
        SetMemberValue(cartItem, "Id", 5);
        _cartRepository
            .GetByIdAsync(5, cts.Token)
            .Returns(cartItem);

        await _handler.Handle(new DeleteCartItemCommand(1, 5), cts.Token);

        await _cartRepository.Received(1).GetByIdAsync(5, cts.Token);
        await _cartRepository.Received(1).UpdateAsync(cartItem, cts.Token);
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
