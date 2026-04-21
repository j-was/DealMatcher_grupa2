namespace DealMatcher.Backend.UnitTests.Core.CategoryAggregate;

public class CategoryEnityTests
{
    private static List<CategoryProperty> TestProperties =>
        [new CategoryProperty("Color", CategoryPropertyType.Text, null)];

    [Fact]
    public void Constructor_Should_Create_Category_With_Valid_Data()
    {
        var category = new CategoryEnity("Electronics", "Electronic devices", TestProperties);

        category.Name.ShouldBe("Electronics");
        category.Description.ShouldBe("Electronic devices");
        category.Properties.Count.ShouldBe(1);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_Should_Throw_When_Name_Is_Invalid(string invalidName)
    {
        Should.Throw<ArgumentException>(() =>
            new CategoryEnity(invalidName!, "Description", TestProperties));
    }

    [Fact]
    public void Constructor_Should_Throw_When_Name_Too_Short()
    {
        var shortName = new string('a', DataSchemaConstants.CategoryNameMinLength - 1);

        Should.Throw<ArgumentException>(() =>
            new CategoryEnity(shortName, "Description", TestProperties))
            .Message.ShouldContain($"Category name must be at least {DataSchemaConstants.CategoryNameMinLength} characters");
    }

    [Fact]
    public void Constructor_Should_Throw_When_Name_Too_Long()
    {
        var longName = new string('a', DataSchemaConstants.CategoryNameMaxLength + 1);

        Should.Throw<ArgumentException>(() =>
            new CategoryEnity(longName, "Description", TestProperties))
            .Message.ShouldContain($"Category name cannot exceed {DataSchemaConstants.CategoryNameMaxLength} characters");
    }

    [Fact]
    public void Constructor_Should_Throw_When_Description_Too_Long()
    {
        var longDescription = new string('a', DataSchemaConstants.CategoryDescriptionMaxLength + 1);

        Should.Throw<ArgumentException>(() =>
            new CategoryEnity("Valid Name", longDescription, TestProperties))
            .Message.ShouldContain($"Category description cannot exceed {DataSchemaConstants.CategoryDescriptionMaxLength} characters");
    }

    [Fact]
    public void Constructor_Should_Allow_Null_Properties()
    {
        var category = new CategoryEnity("Electronics", "Description", null);

        category.Properties.ShouldNotBeNull();
        category.Properties.Count.ShouldBe(0);
    }

    [Fact]
    public void Constructor_Should_Allow_Empty_Description()
    {
        var category = new CategoryEnity("Electronics", "", TestProperties);

        category.Description.ShouldBe("");
    }

    [Fact]
    public void Constructor_Should_Throw_When_Properties_Have_Empty_Name()
    {
        Should.Throw<ArgumentException>(() =>
            new CategoryProperty("", CategoryPropertyType.Text, null));
    }

    [Fact]
    public void Constructor_Should_Throw_When_Properties_Have_Duplicate_Names()
    {
        var duplicateProperties = new List<CategoryProperty>
        {
            new("Color", CategoryPropertyType.Text, null),
            new("color", CategoryPropertyType.Select, ["Red", "Blue"])
        };

        Should.Throw<ArgumentException>(() =>
            new CategoryEnity("Electronics", "Description", duplicateProperties))
            .Message.ShouldContain("Duplicate property name in category: 'Color'");
    }

    [Fact]
    public void Constructor_Should_Trim_Name_And_Description()
    {
        var category = new CategoryEnity("  Electronics  ", "  Description  ", TestProperties);

        category.Name.ShouldBe("Electronics");
        category.Description.ShouldBe("Description");
    }

    [Fact]
    public void UpdateDetails_Should_Update_Name_And_Description()
    {
        var category = new CategoryEnity("Old Name", "Old Description", TestProperties);

        category.UpdateDetails("New Name", "New Description");

        category.Name.ShouldBe("New Name");
        category.Description.ShouldBe("New Description");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void UpdateDetails_Should_Throw_When_Name_Is_Invalid(string invalidName)
    {
        var category = new CategoryEnity("Valid Name", "Description", TestProperties);

        Should.Throw<ArgumentException>(() =>
            category.UpdateDetails(invalidName!, "New Description"));
    }

    [Fact]
    public void UpdateDetails_Should_Throw_When_Name_Too_Short()
    {
        var category = new CategoryEnity("Valid Name", "Description", TestProperties);
        var shortName = new string('a', DataSchemaConstants.CategoryNameMinLength - 1);

        Should.Throw<ArgumentException>(() =>
            category.UpdateDetails(shortName, "Description"));
    }

    [Fact]
    public void UpdateDetails_Should_Throw_When_Name_Too_Long()
    {
        var category = new CategoryEnity("Valid Name", "Description", TestProperties);
        var longName = new string('a', DataSchemaConstants.CategoryNameMaxLength + 1);

        Should.Throw<ArgumentException>(() =>
            category.UpdateDetails(longName, "Description"));
    }

    [Fact]
    public void UpdateDetails_Should_Allow_Empty_Description()
    {
        var category = new CategoryEnity("Valid Name", "Old Description", TestProperties);

        category.UpdateDetails("New Name", "");

        category.Description.ShouldBe("");
    }
}
