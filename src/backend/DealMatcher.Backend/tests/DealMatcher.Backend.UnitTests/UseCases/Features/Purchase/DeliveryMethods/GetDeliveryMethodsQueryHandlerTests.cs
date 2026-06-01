namespace DealMatcher.Backend.UnitTests.UseCases.Features.Purchase.DeliveryMethods;

public class GetDeliveryMethodsQueryHandlerTests
{
    private readonly IReadRepository<DeliveryMethod> _methodsRepository;
    private readonly IMapper _mapper;
    private readonly GetDeliveryMethodsQueryHandler _handler;

    public GetDeliveryMethodsQueryHandlerTests()
    {
        _methodsRepository = Substitute.For<IReadRepository<DeliveryMethod>>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetDeliveryMethodsQueryHandler(_methodsRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WithMappedDeliveryMethods()
    {
        var methods = new List<DeliveryMethod>
        {
            new("dhl", "DHL", "Kurier", 19.99m, 2), new("pickup", "Odbiór", "Punkt odbioru", 0m, 1),
        };

        var expectedDtos = new List<DeliveryMethodDTO>
        {
            new("dhl", "DHL", "Kurier", 19.99, 2), new("pickup", "Odbiór", "Punkt odbioru", 0, 1),
        };

        _methodsRepository.ListAsync(CancellationToken.None).Returns(methods);
        _mapper.Map<List<DeliveryMethodDTO>>(methods).Returns(expectedDtos);

        var result = await _handler.Handle(new GetDeliveryMethodsQuery(), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(expectedDtos);

        await _methodsRepository.Received(1).ListAsync(CancellationToken.None);
        _mapper.Received(1).Map<List<DeliveryMethodDTO>>(methods);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken()
    {
        var cts = new CancellationTokenSource();

        _methodsRepository.ListAsync(cts.Token).Returns([]);
        _mapper.Map<List<DeliveryMethodDTO>>(Arg.Any<List<DeliveryMethod>>()).Returns([]);

        await _handler.Handle(new GetDeliveryMethodsQuery(), cts.Token);

        await _methodsRepository.Received(1).ListAsync(cts.Token);
    }
}
