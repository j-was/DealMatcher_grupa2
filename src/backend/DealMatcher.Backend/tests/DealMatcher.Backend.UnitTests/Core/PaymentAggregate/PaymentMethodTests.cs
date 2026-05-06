namespace DealMatcher.Backend.UnitTests.Core.PaymentAggregate;

public class PaymentMethodTests
{
    [Fact]
    public void Constructor_Should_Create_PaymentMethod_With_Valid_Data()
    {
        var payment = new PaymentMethod("PAYPAL", "PayPal", "PayPal Inc.", "paypal-icon.png");

        payment.StringId.ShouldBe("PAYPAL");
        payment.Name.ShouldBe("PayPal");
        payment.Provider.ShouldBe("PayPal Inc.");
        payment.Icon.ShouldBe("paypal-icon.png");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_Should_Throw_When_Id_Is_Invalid(string invalidId)
    {
        Should.Throw<ArgumentException>(() =>
            new PaymentMethod(invalidId!, "Name", "Provider", "icon.png"));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_Should_Throw_When_Name_Is_Invalid(string invalidName)
    {
        Should.Throw<ArgumentException>(() =>
            new PaymentMethod("ID", invalidName!, "Provider", "icon.png"));
    }

    [Fact]
    public void Constructor_Should_Throw_When_Name_Too_Long()
    {
        var longName = new string('a', DataSchemaConstants.PaymentMethodNameMaxLength + 1);

        Should.Throw<ArgumentException>(() =>
            new PaymentMethod("ID", longName, "Provider", "icon.png"));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_Should_Throw_When_Provider_Is_Invalid(string invalidProvider)
    {
        Should.Throw<ArgumentException>(() =>
            new PaymentMethod("ID", "Name", invalidProvider!, "icon.png"));
    }

    [Fact]
    public void Constructor_Should_Throw_When_Provider_Too_Long()
    {
        var longProvider = new string('a', DataSchemaConstants.PaymentMethodProviderMaxLength + 1);

        Should.Throw<ArgumentException>(() =>
            new PaymentMethod("ID", "Name", longProvider, "icon.png"));
    }

    [Fact]
    public void Constructor_Should_Throw_When_Icon_Too_Long()
    {
        var longIcon = new string('a', DataSchemaConstants.ImageUrlMaxLength + 1);

        Should.Throw<ArgumentException>(() =>
            new PaymentMethod("ID", "Name", "Provider", longIcon));
    }

    [Fact]
    public void Constructor_Should_Allow_Empty_Icon()
    {
        var payment = new PaymentMethod("ID", "Name", "Provider", "");

        payment.Icon.ShouldBe("");
    }

    [Fact]
    public void Constructor_Should_Trim_Fields()
    {
        var payment = new PaymentMethod("  ID  ", "  PayPal  ", "  PayPal Inc.  ", "  icon.png  ");

        payment.StringId.ShouldBe("ID");
        payment.Name.ShouldBe("PayPal");
        payment.Provider.ShouldBe("PayPal Inc.");
        payment.Icon.ShouldBe("icon.png");
    }
}
