namespace DealMatcher.Backend.UnitTests.UseCases.Features.Categories.Properties.Get;

public class GetAllPropertiesByCategoryNameQueryHandlerTests
{
    private readonly IReadRepository<CategoryEntity> _categoriesRepository;
    private readonly IMapper _mapper;
    private readonly GetAllPropertiesByCategoryNameQueryHandler _handler;

    public GetAllPropertiesByCategoryNameQueryHandlerTests()
    {
        _categoriesRepository = Substitute.For<IReadRepository<CategoryEntity>>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetAllPropertiesByCategoryNameQueryHandler(_categoriesRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WithMappedProperties_WhenCategoryExists()
    {
        var query = new GetAllPropertiesByCategoryNameQuery("Elektronika");

        var category = new CategoryEntity(
            "Elektronika",
            "Opis",
            [
                new("Stan", CategoryPropertyType.Select, ["Nowy"]),
                new("Pamięć", CategoryPropertyType.Number, null)
            ]);

        var expectedDtos = new List<CategoryPropertyDTO>
        {
            new(1, "Stan", CategoryPropertyType.Select, ["Nowy"]),
            new(2, "Pamięć", CategoryPropertyType.Number, null)
        };

        _categoriesRepository
            .SingleOrDefaultAsync(Arg.Any<CategoryByNameSpec>(), Arg.Any<CancellationToken>())
            .Returns(category);

        _mapper.Map<List<CategoryPropertyDTO>>(category.Properties)
            .Returns(expectedDtos);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(expectedDtos);

        await _categoriesRepository.Received(1)
            .SingleOrDefaultAsync(Arg.Any<CategoryByNameSpec>(), Arg.Any<CancellationToken>());

        _mapper.Received(1)
            .Map<List<CategoryPropertyDTO>>(category.Properties);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        var query = new GetAllPropertiesByCategoryNameQuery("NieIstnieje");

        _categoriesRepository
            .SingleOrDefaultAsync(Arg.Any<CategoryByNameSpec>(), Arg.Any<CancellationToken>())
            .Returns((CategoryEntity?)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.NotFound);

        await _categoriesRepository.Received(1)
            .SingleOrDefaultAsync(Arg.Any<CategoryByNameSpec>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken()
    {
        var cts = new CancellationTokenSource();
        var query = new GetAllPropertiesByCategoryNameQuery("Elektronika");

        var category = new CategoryEntity(
            "Elektronika",
            "Opis",
            []);

        _categoriesRepository
            .SingleOrDefaultAsync(Arg.Any<CategoryByNameSpec>(), cts.Token)
            .Returns(category);

        _mapper.Map<List<CategoryPropertyDTO>>(Arg.Any<List<CategoryProperty>>())
            .Returns([]);

        await _handler.Handle(query, cts.Token);

        await _categoriesRepository.Received(1)
            .SingleOrDefaultAsync(Arg.Any<CategoryByNameSpec>(), cts.Token);
    }
}
