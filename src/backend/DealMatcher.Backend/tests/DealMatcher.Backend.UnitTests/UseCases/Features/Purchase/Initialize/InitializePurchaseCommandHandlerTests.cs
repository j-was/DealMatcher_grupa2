using DealMatcher.Backend.Core.Aggregates.Cart;
using DealMatcher.Backend.Core.Aggregates.Cart.Specifications;
using DealMatcher.Backend.Core.Aggregates.Offer;
using DealMatcher.Backend.Core.Events;
using DealMatcher.Backend.UseCases.Features.Purchase.Initialize;

namespace DealMatcher.Backend.UnitTests.UseCases.Features.Purchase.Initialize;

public class InitializePurchaseCommandHandlerTests
{
    private readonly IRepository<CartItem> _cartItemsRepository;
    private readonly IReadRepository<OfferEntity> _offersRepository;
    private readonly IConfiguration _configuration;
    private readonly InitializePurchaseCommandHandler _handler;
    private readonly IPublisher _publisher;

    public InitializePurchaseCommandHandlerTests()
    {
        _cartItemsRepository = Substitute.For<IRepository<CartItem>>();
        _offersRepository = Substitute.For<IReadRepository<OfferEntity>>();
        _configuration = Substitute.For<IConfiguration>();
        _configuration["FrontendOrigin"].Returns("");
        _publisher = Substitute.For<IPublisher>();
        _handler = new InitializePurchaseCommandHandler(_cartItemsRepository, _offersRepository, _configuration, _publisher);
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalid_WhenCartCheckoutHasNoItems()
    {
        var request = new InitializePurchaseCommand(
            UserId: 1,
            OfferId: -2137,
            PaymentMethodId: "card",
            Quantity: 1);

        _cartItemsRepository
            .ListAsync(Arg.Any<CartItemsByUserIdSpec>(), CancellationToken.None)
            .Returns([]);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();

        await _cartItemsRepository.Received(1)
            .ListAsync(Arg.Any<CartItemsByUserIdSpec>(), CancellationToken.None);
        await _offersRepository.DidNotReceiveWithAnyArgs()
            .GetByIdAsync<int>(default, default);
        await _publisher.DidNotReceive()
            .Publish(Arg.Any<OfferPurchasedEvent>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnRedirect_WhenCartCheckoutSucceeds()
    {
        var request = new InitializePurchaseCommand(
            UserId: 1,
            OfferId: -2137,
            PaymentMethodId: "card",
            Quantity: 1);

        var cartItems = new List<CartItem>
        {
            new(1, 10, 1),
            new(1, 20, 2),
        };

        var firstOffer = new OfferEntity(
            title: "First offer",
            description: "First description",
            price: 10m,
            imageUrls: ["https://example.com/1.png"],
            sellerId: 100,
            tags: ["tag1"],
            categoryId: 5,
            properties: [],
            availability: 10);

        var secondOffer = new OfferEntity(
            title: "Second offer",
            description: "Second description",
            price: 5m,
            imageUrls: ["https://example.com/2.png"],
            sellerId: 101,
            tags: ["tag2"],
            categoryId: 6,
            properties: [],
            availability: 10);

        _cartItemsRepository
            .ListAsync(Arg.Any<CartItemsByUserIdSpec>(), CancellationToken.None)
            .Returns(cartItems);

        _offersRepository.GetByIdAsync(10, CancellationToken.None).Returns(firstOffer);
        _offersRepository.GetByIdAsync(20, CancellationToken.None).Returns(secondOffer);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.RedirectUrl.ShouldBe("/payment/card/20");

        await _cartItemsRepository.Received(1)
            .ListAsync(Arg.Any<CartItemsByUserIdSpec>(), CancellationToken.None);
        await _offersRepository.Received(1).GetByIdAsync(10, CancellationToken.None);
        await _offersRepository.Received(1).GetByIdAsync(20, CancellationToken.None);
        await _publisher.Received(2)
            .Publish(Arg.Any<OfferPurchasedEvent>(), CancellationToken.None);
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalid_WhenQuantityIsLessThanOne()
    {
        var request = new InitializePurchaseCommand(
            UserId: 1,
            OfferId: 10,
            PaymentMethodId: "card",
            Quantity: 0);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();

        await _offersRepository.DidNotReceiveWithAnyArgs()
            .GetByIdAsync<int>(default, default);
        await _cartItemsRepository.DidNotReceiveWithAnyArgs()
            .ListAsync(default!, default);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenOfferDoesNotExist()
    {
        var request = new InitializePurchaseCommand(
            UserId: 1,
            OfferId: 10,
            PaymentMethodId: "card",
            Quantity: 2);

        _offersRepository.GetByIdAsync(10, CancellationToken.None).Returns((OfferEntity?)null);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();

        await _offersRepository.Received(1).GetByIdAsync(10, CancellationToken.None);
        await _cartItemsRepository.DidNotReceiveWithAnyArgs()
            .ListAsync(default!, default);
    }

    [Fact]
    public async Task Handle_ShouldReturnConflict_WhenOfferIsNotInCart()
    {
        var request = new InitializePurchaseCommand(
            UserId: 1,
            OfferId: 10,
            PaymentMethodId: "card",
            Quantity: 2);

        var offer = new OfferEntity(
            title: "Offer",
            description: "Description",
            price: 10m,
            imageUrls: ["https://example.com/offer.png"],
            sellerId: 100,
            tags: ["tag"],
            categoryId: 5,
            properties: [],
            availability: 10);

        _offersRepository.GetByIdAsync(10, CancellationToken.None).Returns(offer);
        _cartItemsRepository
            .ListAsync(Arg.Any<CartItemsByUserIdSpec>(), CancellationToken.None)
            .Returns(
            [
                new(1, 11, 1),
            ]);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();

        await _offersRepository.Received(1).GetByIdAsync(10, CancellationToken.None);
        await _cartItemsRepository.Received(1)
            .ListAsync(Arg.Any<CartItemsByUserIdSpec>(), CancellationToken.None);
    }

    [Fact]
    public async Task Handle_ShouldReturnRedirect_WhenSingleOfferCheckoutSucceeds()
    {
        var request = new InitializePurchaseCommand(
            UserId: 1,
            OfferId: 10,
            PaymentMethodId: "card",
            Quantity: 2);

        var offer = new OfferEntity(
            title: "Offer",
            description: "Description",
            price: 10m,
            imageUrls: ["https://example.com/offer.png"],
            sellerId: 100,
            tags: ["tag"],
            categoryId: 5,
            properties: [],
            availability: 10);

        _offersRepository.GetByIdAsync(10, CancellationToken.None).Returns(offer);
        _cartItemsRepository
            .ListAsync(Arg.Any<CartItemsByUserIdSpec>(), CancellationToken.None)
            .Returns(
            [
                new(1, 10, 1),
            ]);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.RedirectUrl.ShouldBe("/payment/card/20");

        await _offersRepository.Received(1).GetByIdAsync(10, CancellationToken.None);
        await _cartItemsRepository.Received(1)
            .ListAsync(Arg.Any<CartItemsByUserIdSpec>(), CancellationToken.None);
        await _publisher.Received(1)
            .Publish(Arg.Any<OfferPurchasedEvent>(), CancellationToken.None);
    }
}
