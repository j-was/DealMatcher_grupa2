namespace DealMatcher.Backend.UnitTests.UseCases.Mapping;

public class CategoryProfileTests
{
    private readonly IMapper _mapper;

    public CategoryProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CategoryProfile>();
        }, new SerilogLoggerFactory());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Configuration_IsValid()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CategoryProfile>();
        }, new SerilogLoggerFactory());

        configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void Map_CategoryToCategoryDTO_MapsBasicPropertiesCorrectly()
    {
        var category = new CategoryEntity("Electronics", "Electronic devices");
        typeof(CategoryEntity).GetProperty("Id")!.SetValue(category, 1);

        var dto = _mapper.Map<CategoryDTO>(category);

        dto.Id.ShouldBe(1);
        dto.Name.ShouldBe("Electronics");
        dto.Description.ShouldBe("Electronic devices");
    }

    [Fact]
    public void Map_CategoryPropertyToCategoryPropertyDTO_MapsTextTypeCorrectly()
    {
        var property = new CategoryProperty("Color", CategoryPropertyType.Text, null);

        var dto = _mapper.Map<CategoryPropertyDTO>(property);

        dto.Name.ShouldBe("Color");
        dto.Type.ShouldBe("TEXT");
        dto.Options.ShouldBeEmpty();
    }

    [Fact]
    public void Map_CategoryPropertyToCategoryPropertyDTO_MapsSelectTypeWithOptions()
    {
        var options = new List<string> { "Red", "Blue", "Green" };
        var property = new CategoryProperty("Color", CategoryPropertyType.Select, options);

        var dto = _mapper.Map<CategoryPropertyDTO>(property);

        dto.Name.ShouldBe("Color");
        dto.Type.ShouldBe("SELECT");
        dto.Options.ShouldNotBeNull();
        dto.Options!.Count.ShouldBe(3);
    }

    [Fact]
    public void Map_CategoryPropertyToCategoryPropertyDTO_MapsAllTypesToUpperCase()
    {
        var types = new[]
        {
            CategoryPropertyType.Text, CategoryPropertyType.Number, CategoryPropertyType.Boolean,
            CategoryPropertyType.Select
        };
        var expectedTypes = new[] { "TEXT", "NUMBER", "BOOLEAN", "SELECT" };

        for (var i = 0; i < types.Length; i++)
        {
            var property = new CategoryProperty("Property", types[i],
                types[i] == CategoryPropertyType.Select ? ["Option1"] : null);

            var dto = _mapper.Map<CategoryPropertyDTO>(property);

            dto.Type.ShouldBe(expectedTypes[i]);
        }
    }
}
