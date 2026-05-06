namespace DealMatcher.Backend.UnitTests.UseCases.Mapping;

public class DeliveryProfileTests
{
    private readonly IMapper _mapper;

    public DeliveryProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<DeliveryMethodProfile>();
        }, new SerilogLoggerFactory());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Configuration_IsValid()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<DeliveryMethodProfile>();
        }, new SerilogLoggerFactory());

        configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void Map_DeliveryMethodToDeliveryMethodDTO_MapsAllPropertiesCorrectly()
    {
        var deliveryMethod = new DeliveryMethod("DHL", "DHL Express", "Fast delivery", 15.99m, 3);

        var dto = _mapper.Map<DeliveryMethodDTO>(deliveryMethod);

        dto.Id.ShouldBe("DHL");
        dto.Name.ShouldBe("DHL Express");
        dto.Description.ShouldBe("Fast delivery");
        dto.Price.ShouldBe(15.99d);
        dto.EstimatedDays.ShouldBe(3);
    }

    [Fact]
    public void Map_DeliveryMethodToDeliveryMethodDTO_MapsPriceAsDouble()
    {
        var deliveryMethod = new DeliveryMethod("FREE", "Free Shipping", "No cost", 0m, 7);

        var dto = _mapper.Map<DeliveryMethodDTO>(deliveryMethod);

        dto.Price.ShouldBe(0d);
        dto.Price.GetType().ShouldBe(typeof(double));
    }

    [Fact]
    public void Map_DeliveryMethodToDeliveryMethodDTO_WithLongerEstimatedDays_MapsCorrectly()
    {
        var deliveryMethod = new DeliveryMethod("STANDARD", "Standard", "Standard shipping", 5.99m, 14);

        var dto = _mapper.Map<DeliveryMethodDTO>(deliveryMethod);

        dto.EstimatedDays.ShouldBe(14);
    }

    [Fact]
    public void Map_DeliveryMethodToDeliveryMethodDTO_WithDifferentIds_MapsCorrectly()
    {
        var deliveryMethods = new[]
        {
            new DeliveryMethod("DHL", "DHL Express", "Fast", 15.99m, 3),
            new DeliveryMethod("UPS", "UPS Standard", "Reliable", 12.50m, 5),
            new DeliveryMethod("FEDEX", "FedEx Priority", "Overnight", 25.00m, 1)
        };

        var dtos = _mapper.Map<List<DeliveryMethodDTO>>(deliveryMethods);

        dtos[0].Id.ShouldBe("DHL");
        dtos[1].Id.ShouldBe("UPS");
        dtos[2].Id.ShouldBe("FEDEX");
    }
}
