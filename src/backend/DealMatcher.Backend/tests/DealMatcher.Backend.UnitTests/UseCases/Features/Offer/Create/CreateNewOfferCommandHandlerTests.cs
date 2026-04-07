using DealMatcher.Backend.UseCases.Features.Offer.Create;

namespace DealMatcher.Backend.UnitTests.UseCases.Features.Offer.Create;

public class CreateNewOfferCommandHandlerTests
{
    private readonly IRepository<OfferEntity> _offerRepository;
    private readonly IMapper _mapper;
    private readonly CreateNewOfferCommandHandler _handler;
    private readonly IReadRepository<CategoryEntity> _categoriesRepository;

    public CreateNewOfferCommandHandlerTests()
    {
        _offerRepository = Substitute.For<IRepository<OfferEntity>>();
        _categoriesRepository = Substitute.For<IReadRepository<CategoryEntity>>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateNewOfferCommandHandler(_offerRepository, _categoriesRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WithMappedOffer_WhenOfferCreatedSuccessfully()
    {
        var command = new CreateNewOfferCommand("Test offer",
            "Test description",
            100.00,
            new List<String> { "https://example.com/image.jpg" },
            new List<String> { "Test tag" },
            5,
            new Dictionary<string, string> {
            { "Test Property", "Test Value" }},
            2);

        var category = new CategoryEntity("Electronics", "Electronic devices");

        var expectedDto = new OfferDTO(
            Id: 1,
            Title: "Test Offer",
            Description: "Test Description",
            Price: 100.00,
            Images: ["https://example.com/image.jpg"],
            Seller: new SellerDTO(1, "John"),
            Tags: ["Test tag"],
            Category: new CategoryDTO(5, "Electronics", "Electronic devices"),
            Properties: new Dictionary<string, string>
            {
                {"Test Property", "Test Value"}
            },
            Availability: 2,
            Status: "DRAFT",
            CreatedAt: DateTime.UtcNow,
            UpdatedAt: DateTime.UtcNow);

        _categoriesRepository
            .GetByIdAsync(command.CategoryId, Arg.Any<CancellationToken>())
            .Returns(category);

        _mapper
            .Map<OfferDTO>(Arg.Any<OfferEntity>())
            .Returns(expectedDto);

        var result = await _handler.Handle(command, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(expectedDto);

        await _offerRepository.Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());

        _mapper.Received(1).Map<OfferDTO>(Arg.Any<OfferEntity>());
    }
}
