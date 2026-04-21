namespace DealMatcher.Backend.UnitTests.Core.CartItemAggregate;

public class CartItemTests
{
    [Fact]
    public void Constructor_Should_Create_CartItem_With_Valid_Data()
    {
        var cartItem = new CartItemEntity(1, 2, 5);

        cartItem.UserId.ShouldBe(1);
        cartItem.OfferId.ShouldBe(2);
        cartItem.Quantity.ShouldBe(5);
        cartItem.AddedAt.ShouldBeLessThanOrEqualTo(DateTime.UtcNow);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_Should_Throw_When_UserId_Is_Invalid(int invalidUserId)
    {
        Should.Throw<ArgumentOutOfRangeException>(() =>
            new CartItemEntity(invalidUserId, 1, 5));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_Should_Throw_When_OfferId_Is_Invalid(int invalidOfferId)
    {
        Should.Throw<ArgumentOutOfRangeException>(() =>
            new CartItemEntity(1, invalidOfferId, 5));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(1000)]
    public void Constructor_Should_Throw_When_Quantity_Is_Invalid(int invalidQuantity)
    {
        Should.Throw<ArgumentOutOfRangeException>(() =>
            new CartItemEntity(1, 1, invalidQuantity));
    }

    [Fact]
    public void UpdateQuantity_Should_Update_Quantity_When_Valid()
    {
        var cartItem = new CartItemEntity(1, 2, 5);

        cartItem.UpdateQuantity(10);

        cartItem.Quantity.ShouldBe(10);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(1000)]
    public void UpdateQuantity_Should_Throw_When_Quantity_Is_Invalid(int invalidQuantity)
    {
        var cartItem = new CartItemEntity(1, 2, 5);

        Should.Throw<ArgumentOutOfRangeException>(() =>
            cartItem.UpdateQuantity(invalidQuantity));
    }

    [Fact]
    public void Constructor_Should_Set_AddedAt_To_UtcNow()
    {
        var before = DateTime.UtcNow;
        var cartItem = new CartItemEntity(1, 2, 5);
        var after = DateTime.UtcNow;

        cartItem.AddedAt.ShouldBeGreaterThanOrEqualTo(before);
        cartItem.AddedAt.ShouldBeLessThanOrEqualTo(after);
    }
}
