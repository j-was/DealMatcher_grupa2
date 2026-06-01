namespace DealMatcher.Backend.UnitTests.Core.PaymentAggregate;

public class DeliveryMethodTests
{
    [Fact]
    public void Constructor_Should_Create_DeliveryMethod_With_Valid_Data()
    {
        var delivery = new DeliveryMethod("DHL", "DHL Express", "Fast delivery", 15.99m, 3);

        delivery.StringId.ShouldBe("DHL");
        delivery.Name.ShouldBe("DHL Express");
        delivery.Description.ShouldBe("Fast delivery");
        delivery.Price.ShouldBe(15.99m);
        delivery.EstimatedDays.ShouldBe(3);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_Should_Throw_When_Id_Is_Invalid(string invalidId)
    {
        Should.Throw<ArgumentException>(() =>
            new DeliveryMethod(invalidId!, "Name", "Description", 10m, 3));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_Should_Throw_When_Name_Is_Invalid(string invalidName)
    {
        Should.Throw<ArgumentException>(() =>
            new DeliveryMethod("ID", invalidName!, "Description", 10m, 3));
    }

    [Fact]
    public void Constructor_Should_Throw_When_Name_Too_Long()
    {
        var longName = new string('a', DataSchemaConstants.DeliveryMethodNameMaxLength + 1);

        Should.Throw<ArgumentException>(() =>
            new DeliveryMethod("ID", longName, "Description", 10m, 3));
    }

    [Fact]
    public void Constructor_Should_Throw_When_Description_Too_Long()
    {
        var longDesc = new string('a', DataSchemaConstants.DeliveryMethodDescriptionMaxLength + 1);

        Should.Throw<ArgumentException>(() =>
            new DeliveryMethod("ID", "Name", longDesc, 10m, 3));
    }

    [Fact]
    public void Constructor_Should_Throw_When_Price_Is_Negative()
    {
        Should.Throw<ArgumentException>(() =>
                new DeliveryMethod("ID", "Name", "Description", -1m, 3))
            .Message.ShouldContain("Price cannot be negative");
    }

    [Fact]
    public void Constructor_Should_Throw_When_EstimatedDays_Is_Negative()
    {
        Should.Throw<ArgumentException>(() =>
                new DeliveryMethod("ID", "Name", "Description", 10m, -1))
            .Message.ShouldContain("Estimated days cannot be negative");
    }

    [Fact]
    public void Constructor_Should_Allow_Zero_Price()
    {
        var delivery = new DeliveryMethod("FREE", "Free Shipping", "No cost", 0m, 7);

        delivery.Price.ShouldBe(0m);
    }

    [Fact]
    public void Constructor_Should_Trim_Name_And_Description()
    {
        var delivery = new DeliveryMethod("ID", "  Express  ", "  Fast  ", 10m, 2);

        delivery.Name.ShouldBe("Express");
        delivery.Description.ShouldBe("Fast");
    }
}
