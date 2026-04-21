namespace DealMatcher.Backend.UnitTests.UseCases.Features.User.Offers;

public class GetMyOffersQueryHandlerTests
{
    private readonly IRepository<OfferEntity> _offerRepository;
    private readonly IMapper _mapper;
    private readonly GetMyOffersQueryHandler _handler;

    public GetMyOffersQueryHandlerTests()
    {
        _offerRepository = Substitute.For<IRepository<OfferEntity>>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetMyOffersQueryHandler(_offerRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WithMappedOffers_WhenOffersFound()
    {
        var offers = new List<OfferEntity>
        {
            CreateOfferEntity(),
            CreateOfferEntity()
        };

        var expectedDtos = new List<OfferDTO>
        {
            new(1, "Offer 1", "Desc", 100, [], new SellerDTO(1, "Seller"), [],
                new CategoryDTO(1, "Cat", ""), [], 10, "DRAFT", DateTime.UtcNow, DateTime.UtcNow),
            new(2, "Offer 2", "Desc", 100, [], new SellerDTO(1, "Seller"), [],
                new CategoryDTO(1, "Cat", ""), [], 10, "DRAFT", DateTime.UtcNow, DateTime.UtcNow)
        };

        _offerRepository.ListAsync(Arg.Any<OffersBySellerIdSpec>(), Arg.Any<CancellationToken>()).Returns(offers);
        _mapper.Map<List<OfferDTO>>(offers).Returns(expectedDtos);

        var query = new GetMyOffersQuery(1);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(expectedDtos);
        await _offerRepository.Received(1).ListAsync(Arg.Any<OffersBySellerIdSpec>(), Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<List<OfferDTO>>(offers);
    }

    [Fact]
    public async Task Handle_ShouldReturnNoContent_WhenNoOffersFound()
    {
        _offerRepository.ListAsync(Arg.Any<OffersBySellerIdSpec>(), Arg.Any<CancellationToken>()).Returns([]);

        var query = new GetMyOffersQuery(1);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Status.ShouldBe(ResultStatus.NoContent);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken()
    {
        var offers = new List<OfferEntity> { CreateOfferEntity() };
        var cts = new CancellationTokenSource();

        _offerRepository.ListAsync(Arg.Any<OffersBySellerIdSpec>(), cts.Token).Returns(offers);
        _mapper.Map<List<OfferDTO>>(offers).Returns([]);

        var query = new GetMyOffersQuery(1);
        await _handler.Handle(query, cts.Token);

        await _offerRepository.Received(1).ListAsync(Arg.Any<OffersBySellerIdSpec>(), cts.Token);
    }

    [Fact]
    public async Task Handle_ShouldUse_CorrectSellerId_InSpecification()
    {
        var sellerId = 123;
        OffersBySellerIdSpec? capturedSpec = null;

        _offerRepository.ListAsync(
            Arg.Do<OffersBySellerIdSpec>(spec => capturedSpec = spec),
            Arg.Any<CancellationToken>()
        ).Returns([]);

        var query = new GetMyOffersQuery(sellerId);
        await _handler.Handle(query, CancellationToken.None);

        capturedSpec.ShouldNotBeNull();
    }

    private static OfferEntity CreateOfferEntity()
    {
        return new OfferEntity(
            "Test Offer",
            "Description",
            100m,
            ["image.jpg"],
            1,
            ["tag"],
            1,
            [new OfferProperty("Color", "Red")],
            10
        );
    }
}
