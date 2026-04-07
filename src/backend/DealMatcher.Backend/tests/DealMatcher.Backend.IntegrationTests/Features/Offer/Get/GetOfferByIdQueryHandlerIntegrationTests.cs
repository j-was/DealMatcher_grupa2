


using Microsoft.Extensions.Logging.Abstractions;

namespace DealMatcher.Backend.IntegrationTests.Features.Offer.Get;

public class GetOfferByIdQueryHandlerTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly IReadRepository<OfferEntity> _offerRepository;
    private readonly IReadRepository<UserEntity> _usersRepository;
    private readonly IReadRepository<CategoryEntity> _categoriesRepository;
    private readonly IMapper _mapper;
    private readonly GetOfferByIdQueryHandler _handler;

    public GetOfferByIdQueryHandlerTests()
    {
        _context = CreateDbContext();

        _offerRepository = new EfRepository<OfferEntity>(_context);
        _usersRepository = new EfRepository<UserEntity>(_context);
        _categoriesRepository = new EfRepository<CategoryEntity>(_context);

        var mapperConfig = new MapperConfiguration(
        cfg =>
        {
            cfg.AddProfile(new OfferProfile());
        },
        NullLoggerFactory.Instance);

        _mapper = mapperConfig.CreateMapper();
        _handler = new GetOfferByIdQueryHandler(_offerRepository, _usersRepository, _categoriesRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WithMappedOffer_WhenOfferExists()
    {
        var seller = new User("seller@mail.com", "John", "Doe");
        var category = new Category("Electronics", "Electronic devices");

        _context.AddRange(seller, category);
        await _context.SaveChangesAsync();

        var offer = new OfferEntity(
            title: "Test Offer",
            description: "Test Description",
            price: 99.99m,
            imageUrls: ["https://example.com/image.jpg"],
            sellerId: seller.Id,
            tags: ["tag1", "tag2"],
            categoryId: category.Id,
            properties: null,
            availability: 15);

        _context.Add(offer);
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(new GetOfferByIdQuery(offer.Id), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();

        result.Value.Id.ShouldBe(offer.Id);
        result.Value.Title.ShouldBe("Test Offer");
        result.Value.Description.ShouldBe("Test Description");
        result.Value.Price.ShouldBe((double)offer.Price);
        result.Value.Images.ShouldBeEquivalentTo(new List<string> { "https://example.com/image.jpg" });
        result.Value.Tags.ShouldBeEquivalentTo(new List<string> { "tag1", "tag2" });
        result.Value.Availability.ShouldBe(15);
        result.Value.Status.ShouldBe("DRAFT");
        result.Value.Seller.Id.ShouldBe(seller.Id);
        result.Value.Seller.Name.ShouldBe("John Doe");
        result.Value.Category.Id.ShouldBe(category.Id);
        result.Value.Category.Name.ShouldBe("Electronics");
        result.Value.Category.Description.ShouldBe("Electronic devices");
        result.Value.CreatedAt.ShouldBe(offer.CreatedAt);
        result.Value.UpdatedAt.ShouldBe(offer.UpdatedAt);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenOfferDoesNotExist()
    {
        var result = await _handler.Handle(new GetOfferByIdQuery(123456), CancellationToken.None);

        result.Status.ShouldBe(ResultStatus.NotFound);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;

        return new AppDbContext(options, dispatcher: null);
    }
}
