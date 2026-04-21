namespace DealMatcher.Backend.UnitTests.UseCases.Features.Offer.Search;

public class SearchOfferQueryHandlerTests
{
    private readonly IReadRepository<OfferEntity> _offersRepository;
    private readonly IMapper _mapper;
    private readonly SearchOfferQueryHandler _handler;

    public SearchOfferQueryHandlerTests()
    {
        _offersRepository = Substitute.For<IReadRepository<OfferEntity>>();
        _mapper = Substitute.For<IMapper>();
        _handler = new SearchOfferQueryHandler(_offersRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WithMappedOffers_WhenOffersFound()
    {
        var offers = new List<OfferEntity>
        {
            CreateOfferEntity(title: "iPhone 12", price: 999.99m, categoryId: 1, tags: ["electronics"]),
            CreateOfferEntity(title: "Samsung TV", price: 499.99m, categoryId: 1, tags: ["electronics"])
        };

        var expectedDtos = new List<OfferDTO>
        {
            new(1, "iPhone 12", "Description", 999.99, [], new SellerDTO(1, "Seller"), ["electronics"],
                new CategoryDTO(1, "Electronics", ""), [], 10, "DRAFT", DateTime.UtcNow, DateTime.UtcNow),
            new(2, "Samsung TV", "Description", 499.99, [], new SellerDTO(1, "Seller"), ["electronics"],
                new CategoryDTO(1, "Electronics", ""), [], 10, "DRAFT", DateTime.UtcNow, DateTime.UtcNow)
        };

        _offersRepository.ListAsync(Arg.Any<CancellationToken>()).Returns(offers);
        _mapper.Map<List<OfferDTO>>(Arg.Any<List<OfferEntity>>()).Returns(expectedDtos);

        var query = new SearchOfferQuery(null, null, null, null, null, null, 10);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(expectedDtos);
        await _offersRepository.Received(1).ListAsync(Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<List<OfferDTO>>(Arg.Any<List<OfferEntity>>());
    }

    [Fact]
    public async Task Handle_ShouldReturnNoContent_WhenNoOffersFound()
    {
        _offersRepository.ListAsync(Arg.Any<CancellationToken>()).Returns([]);

        var query = new SearchOfferQuery(null, null, null, null, null, "nonexistent", 10);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Status.ShouldBe(ResultStatus.NoContent);
    }

    [Fact]
    public async Task Handle_ShouldFilter_ByCategory()
    {
        var offers = new List<OfferEntity>
        {
            CreateOfferEntity(categoryId: 1),
            CreateOfferEntity(categoryId: 2)
        };

        var expectedDto = new OfferDTO(1, "Title", "Desc", 100, [], new SellerDTO(1, "Seller"), [],
            new CategoryDTO(1, "Cat", ""), [], 10, "DRAFT", DateTime.UtcNow, DateTime.UtcNow);

        _offersRepository.ListAsync(Arg.Any<CancellationToken>()).Returns(offers);
        _mapper.Map<List<OfferDTO>>(Arg.Is<List<OfferEntity>>(o => o.Count == 1)).Returns([expectedDto]);

        var query = new SearchOfferQuery(1, null, null, null, null, null, 10);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Count.ShouldBe(1);
    }

    [Fact]
    public async Task Handle_ShouldFilter_ByPriceRange()
    {
        var offers = new List<OfferEntity>
        {
            CreateOfferEntity(price: 50m),
            CreateOfferEntity(price: 100m),
            CreateOfferEntity(price: 150m)
        };

        _offersRepository.ListAsync(Arg.Any<CancellationToken>()).Returns(offers);
        _mapper.Map<List<OfferDTO>>(Arg.Is<List<OfferEntity>>(o => o.Count == 1)).Returns([new OfferDTO(
            1, "Title", "Desc", 100, [], new SellerDTO(1, "Seller"), [],
            new CategoryDTO(1, "Cat", ""), [], 10, "DRAFT", DateTime.UtcNow, DateTime.UtcNow)]);

        var query = new SearchOfferQuery(null, 75, 125, null, null, null, 10);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Count.ShouldBe(1);
    }

    [Fact]
    public async Task Handle_ShouldFilter_BySearchPhrase()
    {
        var offers = new List<OfferEntity>
        {
            CreateOfferEntity(title: "iPhone 12"),
            CreateOfferEntity(title: "Samsung TV", description: "Great iPhone accessory"),
            CreateOfferEntity(title: "MacBook Pro")
        };

        _offersRepository.ListAsync(Arg.Any<CancellationToken>()).Returns(offers);
        _mapper.Map<List<OfferDTO>>(Arg.Is<List<OfferEntity>>(o => o.Count == 2)).Returns([

            new OfferDTO(1, "iPhone 12", "Desc", 100, [], new SellerDTO(1, "Seller"), [],
                new CategoryDTO(1, "Cat", ""), [], 10, "DRAFT", DateTime.UtcNow, DateTime.UtcNow),
            new OfferDTO(2, "Samsung TV", "iPhone accessory", 100, [], new SellerDTO(1, "Seller"), [],
                new CategoryDTO(1, "Cat", ""), [], 10, "DRAFT", DateTime.UtcNow, DateTime.UtcNow)
        ]);

        var query = new SearchOfferQuery(null, null, null, null, null, "iphone", 10);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Count.ShouldBe(2);
    }

    [Fact]
    public async Task Handle_ShouldFilter_ByTags()
    {
        var offers = new List<OfferEntity>
        {
            CreateOfferEntity(tags: ["electronics", "new"]),
            CreateOfferEntity(tags: ["books", "used"]),
            CreateOfferEntity(tags: ["electronics", "used"])
        };

        _offersRepository.ListAsync(Arg.Any<CancellationToken>()).Returns(offers);
        _mapper.Map<List<OfferDTO>>(Arg.Is<List<OfferEntity>>(o => o.Count == 2)).Returns([
            new OfferDTO(1, "Title1", "Desc", 100, [], new SellerDTO(1, "Seller"), ["electronics", "new"],
                new CategoryDTO(1, "Cat", ""), [], 10, "DRAFT", DateTime.UtcNow, DateTime.UtcNow),
            new OfferDTO(2, "Title2", "Desc", 100, [], new SellerDTO(1, "Seller"), ["electronics", "used"],
                new CategoryDTO(1, "Cat", ""), [], 10, "DRAFT", DateTime.UtcNow, DateTime.UtcNow)
        ]);

        var query = new SearchOfferQuery(null, null, null, ["electronics"], null, null, 10);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Count.ShouldBe(2);
    }

    [Fact]
    public async Task Handle_ShouldRespect_Limit()
    {
        var offers = Enumerable.Range(1, 20)
            .Select(i => CreateOfferEntity(title: $"Offer {i}"))
            .ToList();

        _offersRepository.ListAsync(Arg.Any<CancellationToken>()).Returns(offers);
        _mapper.Map<List<OfferDTO>>(Arg.Is<List<OfferEntity>>(o => o.Count == 5)).Returns(
            [.. Enumerable.Range(1, 5).Select(i => new OfferDTO(
                i, $"Offer {i}", "Desc", 100, [], new SellerDTO(1, "Seller"), [],
                new CategoryDTO(1, "Cat", ""), [], 10, "DRAFT", DateTime.UtcNow, DateTime.UtcNow))]
        );

        var query = new SearchOfferQuery(null, null, null, null, null, null, 5);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Count.ShouldBe(5);
    }

    private static OfferEntity CreateOfferEntity(
        string title = "Test Offer",
        decimal price = 100m,
        int categoryId = 1,
        string description = "Description",
        List<string>? tags = null)
    {
        return new OfferEntity(
            title,
            description,
            price,
            ["image.jpg"],
            1,
            tags ?? [],
            categoryId,
            [new OfferProperty("Color", "Red")],
            10
        );
    }
}
