namespace DealMatcher.Backend.UnitTests.Core.CategoryAggregate;

public class CategoryPropertyTests
{
    [Fact]
    public void Constructor_Should_Create_Text_Property_With_Valid_Data()
    {
        var property = new CategoryProperty("Color", CategoryPropertyType.Text, null);

        property.Name.ShouldBe("Color");
        property.Type.ShouldBe(CategoryPropertyType.Text);
        property.Options.ShouldBeNull();
    }

    [Fact]
    public void Constructor_Should_Create_Select_Property_With_Options()
    {
        var options = new List<string> { "Red", "Blue", "Green" };
        var property = new CategoryProperty("Color", CategoryPropertyType.Select, options);

        property.Name.ShouldBe("Color");
        property.Type.ShouldBe(CategoryPropertyType.Select);
        property.Options.ShouldNotBeNull();
        property.Options!.Count.ShouldBe(3);
        property.Options.ShouldContain("Red");
        property.Options.ShouldContain("Blue");
        property.Options.ShouldContain("Green");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_Should_Throw_When_Name_Is_Invalid(string invalidName)
    {
        Should.Throw<ArgumentException>(() =>
            new CategoryProperty(invalidName!, CategoryPropertyType.Text, null));
    }

    [Fact]
    public void Constructor_Should_Throw_When_Name_Too_Short()
    {
        var shortName = new string('a', DataSchemaConstants.PropertyNameMinLength - 1);

        Should.Throw<ArgumentException>(() =>
            new CategoryProperty(shortName, CategoryPropertyType.Text, null));
    }

    [Fact]
    public void Constructor_Should_Throw_When_Name_Too_Long()
    {
        var longName = new string('a', DataSchemaConstants.PropertyNameMaxLength + 1);

        Should.Throw<ArgumentException>(() =>
                new CategoryProperty(longName, CategoryPropertyType.Text, null))
            .Message.ShouldContain(
                $"Property name cannot exceed {DataSchemaConstants.PropertyNameMaxLength} characters");
    }

    [Fact]
    public void Constructor_Should_Throw_When_Select_Type_Has_No_Options()
    {
        Should.Throw<ArgumentException>(() =>
                new CategoryProperty("Color", CategoryPropertyType.Select, null))
            .Message.ShouldContain("Options must be provided for SELECT type properties");

        Should.Throw<ArgumentException>(() =>
                new CategoryProperty("Color", CategoryPropertyType.Select, []))
            .Message.ShouldContain("Options must be provided for SELECT type properties");
    }

    [Fact]
    public void Constructor_Should_Throw_When_Options_Exceed_Maximum()
    {
        var tooManyOptions = Enumerable.Range(1, DataSchemaConstants.MaxPropertyOptions + 1)
            .Select(i => $"Option{i}")
            .ToList();

        Should.Throw<ArgumentException>(() =>
                new CategoryProperty("Color", CategoryPropertyType.Select, tooManyOptions))
            .Message.ShouldContain($"Property cannot have more than {DataSchemaConstants.MaxPropertyOptions} options");
    }

    [Fact]
    public void Constructor_Should_Throw_When_Option_Value_Too_Long()
    {
        var longOption = new string('a', DataSchemaConstants.PropertyOptionMaxLength + 1);
        var options = new List<string> { "Red", longOption };

        Should.Throw<ArgumentException>(() =>
                new CategoryProperty("Color", CategoryPropertyType.Select, options))
            .Message.ShouldContain(
                $"Option values cannot exceed {DataSchemaConstants.PropertyOptionMaxLength} characters");
    }

    [Fact]
    public void Constructor_Should_Trim_Name_And_Options()
    {
        var options = new List<string> { "  Red  ", " Blue ", "Green" };
        var property = new CategoryProperty("  Color  ", CategoryPropertyType.Select, options);

        property.Name.ShouldBe("Color");
        if (property.Options is null)
        {
            return;
        }

        property.Options.ShouldContain("Red");
        property.Options.ShouldContain("Blue");
        property.Options.ShouldContain("Green");
    }

    [Fact]
    public void Constructor_Should_Allow_Number_Type_Without_Options()
    {
        var property = new CategoryProperty("Price", CategoryPropertyType.Number, null);

        property.Name.ShouldBe("Price");
        property.Type.ShouldBe(CategoryPropertyType.Number);
        property.Options.ShouldBeNull();
    }

    [Fact]
    public void Constructor_Should_Allow_Boolean_Type_Without_Options()
    {
        var property = new CategoryProperty("InStock", CategoryPropertyType.Boolean, null);

        property.Name.ShouldBe("InStock");
        property.Type.ShouldBe(CategoryPropertyType.Boolean);
        property.Options.ShouldBeNull();
    }
}
