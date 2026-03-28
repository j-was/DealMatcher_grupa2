namespace DealMatcher.Backend.UnitTests.UseCases.Features.Offer.Get;

public class GetOfferByIdQueryHandlerTests
{
    private readonly IReadRepository<OfferEntity> _offerRepository;
    private readonly IMapper _mapper;
    private readonly GetOfferByIdQueryHandler _handler;
    private readonly IReadRepository<UserEntity> _usersRepository;
    private readonly IReadRepository<CategoryEntity> _categoriesRepository;

    public GetOfferByIdQueryHandlerTests()
    {
        _offerRepository = Substitute.For<IReadRepository<OfferEntity>>();
        _usersRepository = Substitute.For<IReadRepository<UserEntity>>();
        _categoriesRepository = Substitute.For<IReadRepository<CategoryEntity>>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetOfferByIdQueryHandler(_offerRepository,_usersRepository, _categoriesRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WithMappedOffer_WhenOfferExists()
    {
        // Arrange
        var query = new GetOfferByIdQuery(1);

        var offer = new OfferEntity(
            title: "Test Offer",
            description: "Test Description",
            price: 99.99m,
            imageUrls: new List<string> { "https://example.com/image.jpg" },
            sellerId: 10,
            tags: new List<string> { "tag1", "tag2" },
            categoryId: 5,
            properties: new List<OfferProperty>
            {
                new("Color", "Red"),
                new("Size", "Large")
            },
            availability: 15);

        var seller = new User("John Doe");
        var category = new Category("Electronics", "Electronic devices");

        var expectedDto = new OfferDTO(
            Id: 1,
            Title: "Test Offer",
            Description: "Test Description",
            Price: 99.99,
            Images: new List<string> { "https://example.com/image.jpg" },
            Seller: new SellerDTO(10, "John Doe", 0f),
            Tags: new List<string> { "tag1", "tag2" },
            Category: new CategoryDTO(5, "Electronics", "Electronic devices"),
            Properties: new Dictionary<string, string>
            {
                { "Color", "Red" },
                { "Size", "Large" }
            },
            Availability: 15,
            Status: "DRAFT",
            CreatedAt: offer.CreatedAt,
            UpdatedAt: offer.UpdatedAt);

        _offerRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(offer);
        _categoriesRepository.GetByIdAsync(5, Arg.Any<CancellationToken>())
            .Returns(category);
        _usersRepository.GetByIdAsync(10, Arg.Any<CancellationToken>())
            .Returns(seller);
        var oi = new OfferProfile.OfferInfo(Arg.Any<OfferEntity>(), Arg.Any<UserEntity>(), Arg.Any<CategoryEntity>());
        _mapper.Map<OfferDTO>(oi)
            .Returns(expectedDto);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(expectedDto);

        await _offerRepository.Received(1).GetByIdAsync(1, Arg.Any<CancellationToken>());
        await _categoriesRepository.Received(1).GetByIdAsync(5, Arg.Any<CancellationToken>());
        await _usersRepository.Received(1).GetByIdAsync(10, Arg.Any<CancellationToken>());
        oi = new OfferProfile.OfferInfo(offer, seller, category);
        _mapper.Received(1).Map<OfferDTO>(oi);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken()
    {
        var query = new GetOfferByIdQuery(1);
        var cts = new CancellationTokenSource();

        var offer = new OfferEntity(
            title: "Test Offer",
            description: "Test Description",
            price: 99.99m,
            imageUrls: new List<string> { "https://example.com/image.jpg" },
            sellerId: 10,
            tags: new List<string>(),
            categoryId: 5,
            properties: new List<OfferProperty>(),
            availability: 15);

        var seller = new User("John Doe");
        var category = new Category("Electronics", "Description");

        _offerRepository.GetByIdAsync(1, cts.Token)
            .Returns(offer);
        _usersRepository.GetByIdAsync(10, cts.Token)
            .Returns(seller);
        _categoriesRepository.GetByIdAsync(5, cts.Token)
            .Returns(category);
        var oi = new OfferProfile.OfferInfo(Arg.Any<OfferEntity>(), Arg.Any<UserEntity>(), Arg.Any<CategoryEntity>());
        _mapper.Map<OfferDTO>(oi)
            .Returns(new OfferDTO(1, "Test", "Desc", 99.99, new List<string>(),
                new SellerDTO(10, "John", 0), new List<string>(),
                new CategoryDTO(5, "Electronics", "Desc"),
                new Dictionary<string, string>(), 15, "DRAFT", DateTime.UtcNow, DateTime.UtcNow));

        await _handler.Handle(query, cts.Token);

        await _offerRepository.Received(1).GetByIdAsync(1, cts.Token);
        await _usersRepository.Received(1).GetByIdAsync(10, cts.Token);
        await _categoriesRepository.Received(1).GetByIdAsync(5, cts.Token);
    }
}
