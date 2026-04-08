using DealMatcher.Backend.UseCases.Features.Category.Get;

namespace DealMatcher.Backend.UnitTests.UseCases.Features.Categories.Get;

public class GetAllCategoriesQueryHandlerTests
{
    private readonly IReadRepository<CategoryEntity> _categoriesRepository;
    private readonly IMapper _mapper;
    private readonly GetAllCategoriesHandler _handler;

    public GetAllCategoriesQueryHandlerTests()
    {
        _categoriesRepository = Substitute.For<IReadRepository<CategoryEntity>>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetAllCategoriesHandler(_categoriesRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WithMappedCategories()
    {
        var categories = new List<CategoryEntity>
        {
            new("Elektronika", "Opis"),
            new("Sport", "Opis")
        };

        var expectedDtos = new List<CategoryDTO>
        {
            new(1, "Elektronika", "Opis"),
            new(2, "Sport", "Opis")
        };

        _categoriesRepository.ListAsync(Arg.Any<CancellationToken>())
            .Returns(categories);

        _mapper.Map<List<CategoryDTO>>(categories)
            .Returns(expectedDtos);

        var result = await _handler.Handle(new GetAllCategoriesQuery(), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(expectedDtos);

        await _categoriesRepository.Received(1)
            .ListAsync(Arg.Any<CancellationToken>());

        _mapper.Received(1)
            .Map<List<CategoryDTO>>(categories);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken()
    {
        var cts = new CancellationTokenSource();

        _categoriesRepository.ListAsync(cts.Token)
            .Returns([]);

        _mapper.Map<List<CategoryDTO>>(Arg.Any<List<CategoryEntity>>())
            .Returns([]);

        await _handler.Handle(new GetAllCategoriesQuery(), cts.Token);

        await _categoriesRepository.Received(1)
            .ListAsync(cts.Token);
    }
}
