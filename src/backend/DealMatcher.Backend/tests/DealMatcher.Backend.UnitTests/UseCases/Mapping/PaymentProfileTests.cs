namespace DealMatcher.Backend.UnitTests.UseCases.Mapping;

public class PaymentProfileTests
{
    private readonly IMapper _mapper;

    public PaymentProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PaymentMethodProfile>();
        }, new SerilogLoggerFactory());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Configuration_IsValid()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PaymentMethodProfile>();
        }, new SerilogLoggerFactory());

        configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void Map_PaymentMethodToPaymentMethodDTO_MapsAllPropertiesCorrectly()
    {
        var paymentMethod = new PaymentMethod("PAYPAL", "PayPal", "PayPal Inc.", "paypal-icon.png");

        var dto = _mapper.Map<PaymentMethodDTO>(paymentMethod);

        dto.Id.ShouldBe("PAYPAL");
        dto.Name.ShouldBe("PayPal");
        dto.Provider.ShouldBe("PayPal Inc.");
        dto.Icon.ShouldBe("paypal-icon.png");
    }

    [Fact]
    public void Map_PaymentMethodToPaymentMethodDTO_WithEmptyIcon_MapsCorrectly()
    {
        var paymentMethod = new PaymentMethod("CARD", "Credit Card", "Stripe", "");

        var dto = _mapper.Map<PaymentMethodDTO>(paymentMethod);

        dto.Icon.ShouldBe("");
    }

    [Fact]
    public void Map_PaymentMethodToPaymentMethodDTO_WithDifferentProviders_MapsCorrectly()
    {
        var paymentMethods = new[]
        {
            new PaymentMethod("PAYPAL", "PayPal", "PayPal Inc.", "paypal.png"),
            new PaymentMethod("STRIPE", "Stripe", "Stripe Inc.", "stripe.png"),
            new PaymentMethod("BLIK", "BLIK", "Polish Payment Standard", "blik.png")
        };

        var dtos = _mapper.Map<List<PaymentMethodDTO>>(paymentMethods);

        dtos[0].Provider.ShouldBe("PayPal Inc.");
        dtos[1].Provider.ShouldBe("Stripe Inc.");
        dtos[2].Provider.ShouldBe("Polish Payment Standard");
    }
}
