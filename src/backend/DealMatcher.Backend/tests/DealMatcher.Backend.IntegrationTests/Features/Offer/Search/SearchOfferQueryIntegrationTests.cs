using DealMatcher.Backend.UseCases.Features.Offer.Search;
using Microsoft.Extensions.Logging.Abstractions;

namespace DealMatcher.Backend.IntegrationTests.Features.Offer.Search;

public class SearchOfferQueryHandlerTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly IReadRepository<OfferEntity> _offerRepository;
    private readonly IMapper _mapper;
    private readonly SearchOfferQueryHandler _handler;

    public SearchOfferQueryHandlerTests()
    {
        _context = CreateDbContext();

        _offerRepository = new EfRepository<OfferEntity>(_context);

        var mapperConfig = new MapperConfiguration(
            cfg =>
            {
                cfg.AddProfile(new OfferProfile());
            },
            NullLoggerFactory.Instance);

        _mapper = mapperConfig.CreateMapper();
        _handler = new SearchOfferQueryHandler(_offerRepository, _mapper);
    }

    // [Fact]
    // public async Task Handle_ShouldReturnSuccess_WithMatchedOffers_WhenSearchCriteriaMatch()
    // {
    //     var seller = new User("seller@mail.com", "John", "Doe");
    //     var category = new CategoryEntity("Electronics", "Electronic devices");
    //
    //     _context.AddRange(seller, category);
    //     await _context.SaveChangesAsync();
    //
    //     var matchingOffer = new OfferEntity(
    //         title: "Gaming Laptop",
    //         description: "Powerful gaming laptop",
    //         price: 3500m,
    //         imageUrls: ["https://example.com/laptop.jpg"],
    //         sellerId: seller.Id,
    //         tags: ["electronics", "gaming"],
    //         categoryId: category.Id,
    //         properties: null,
    //         availability: 3);
    //
    //     var nonMatchingOffer = new OfferEntity(
    //         title: "Office Chair",
    //         description: "Comfortable chair",
    //         price: 800m,
    //         imageUrls: ["https://example.com/chair.jpg"],
    //         sellerId: seller.Id,
    //         tags: ["furniture"],
    //         categoryId: category.Id,
    //         properties: null,
    //         availability: 10);
    //
    //     _context.AddRange(matchingOffer, nonMatchingOffer);
    //     await _context.SaveChangesAsync();
    //
    //     var result = await _handler.Handle(
    //         new SearchOfferQuery(
    //             CategoryId: category.Id,
    //             MinPrice: 1000,
    //             MaxPrice: 4000,
    //             Tags: ["electronics"],
    //             Properties: null,
    //             SearchPhrase: "laptop",
    //             Limit: 20),
    //         CancellationToken.None);
    //
    //     result.IsSuccess.ShouldBeTrue();
    //     result.Value.ShouldNotBeNull();
    //     result.Value.Count.ShouldBe(1);
    //
    //     var offer = result.Value.Single();
    //     offer.Id.ShouldBe(matchingOffer.Id);
    //     offer.Title.ShouldBe("Gaming Laptop");
    //     offer.Description.ShouldBe("Powerful gaming laptop");
    //     offer.Price.ShouldBe((double)matchingOffer.Price);
    //     offer.Tags.ShouldContain("electronics");
    //     offer.Tags.ShouldContain("gaming");
    //     offer.Availability.ShouldBe(3);
    //     offer.Seller.Id.ShouldBe(seller.Id);
    //     offer.Seller.Name.ShouldBe("User1");
    //     offer.Category.Id.ShouldBe(category.Id);
    //     offer.Category.Name.ShouldBe("");
    //     offer.Category.Description.ShouldBe("");
    // }

    // [Fact]
    // public async Task Handle_ShouldReturnNoContent_WhenNoOffersMatchSearchCriteria()
    // {
    //     var seller = new User("seller@mail.com", "John", "Doe");
    //     var category = new CategoryEntity("Electronics", "Electronic devices");
    //
    //     _context.AddRange(seller, category);
    //     await _context.SaveChangesAsync();
    //
    //     var offer = new OfferEntity(
    //         title: "Office Chair",
    //         description: "Comfortable chair",
    //         price: 800m,
    //         imageUrls: ["https://example.com/chair.jpg"],
    //         sellerId: seller.Id,
    //         tags: ["furniture"],
    //         categoryId: category.Id,
    //         properties: null,
    //         availability: 10);
    //
    //     _context.Add(offer);
    //     await _context.SaveChangesAsync();
    //
    //     var result = await _handler.Handle(
    //         new SearchOfferQuery(
    //             CategoryId: category.Id,
    //             MinPrice: 2000,
    //             MaxPrice: 3000,
    //             Tags: ["electronics"],
    //             Properties: null,
    //             SearchPhrase: "laptop",
    //             Limit: 20),
    //         CancellationToken.None);
    //
    //     result.Status.ShouldBe(ResultStatus.NoContent);
    // }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;

        return new AppDbContext(options, dispatcher: null);
    }
}
