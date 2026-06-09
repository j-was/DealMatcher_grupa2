using DealMatcher.Backend.Core.Aggregates.Category;
using DealMatcher.Backend.UseCases.Features.Category.Get;
using Microsoft.Extensions.Logging.Abstractions;

namespace DealMatcher.Backend.IntegrationTests.Features.Category.Get;

public class GetAllCategoriesQueryHandlerTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly IReadRepository<CategoryEntity> _categoryRepository;
    private readonly IMapper _mapper;
    private readonly GetAllCategoriesHandler _handler;

    public GetAllCategoriesQueryHandlerTests()
    {
        _context = CreateDbContext();

        _categoryRepository = new EfRepository<CategoryEntity>(_context);

        var mapperConfig = new MapperConfiguration(
            cfg =>
            {
                cfg.AddProfile(new CategoryProfile());
            },
            NullLoggerFactory.Instance);

        _mapper = mapperConfig.CreateMapper();
        _handler = new GetAllCategoriesHandler(_categoryRepository, _mapper);
    }

    // [Fact]
    // public async Task Handle_ShouldReturnSuccess_WithMappedCategories_WhenCategoriesExist()
    // {
    //     var electronics = new CategoryEntity("Electronics", "Electronic devices");
    //     var books = new CategoryEntity("Books", "Books and literature");
    //
    //     _context.AddRange(electronics, books);
    //     await _context.SaveChangesAsync();
    //
    //     var result = await _handler.Handle(new GetAllCategoriesQuery(), CancellationToken.None);
    //
    //     result.IsSuccess.ShouldBeTrue();
    //     result.Value.ShouldNotBeNull();
    //     result.Value.Count.ShouldBe(2);
    //
    //     result.Value.Select(x => x.Name).OrderBy(x => x).ToList()
    // .ShouldBe(["Books", "Electronics"]);
    //
    //     result.Value.Select(x => x.Description).OrderBy(x => x).ToList()
    //         .ShouldBe(
    //         [
    //     "Books and literature",
    //     "Electronic devices"
    //         ]);
    // }

    // [Fact]
    // public async Task Handle_ShouldReturnSuccess_WithEmptyList_WhenNoCategoriesExist()
    // {
    //     var result = await _handler.Handle(new GetAllCategoriesQuery(), CancellationToken.None);
    //
    //     result.IsSuccess.ShouldBeTrue();
    //     result.Value.ShouldNotBeNull();
    //     result.Value.ShouldBeEmpty();
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
