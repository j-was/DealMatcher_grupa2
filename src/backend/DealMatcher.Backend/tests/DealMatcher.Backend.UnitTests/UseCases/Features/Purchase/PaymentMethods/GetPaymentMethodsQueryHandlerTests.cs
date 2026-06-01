namespace DealMatcher.Backend.UnitTests.UseCases.Features.Purchase.PaymentMethods;

public class GetPaymentMethodsQueryHandlerTests
{
    private readonly IReadRepository<PaymentMethod> _methodsRepository;
    private readonly IMapper _mapper;
    private readonly GetPaymentMethodsQueryHandler _handler;

    public GetPaymentMethodsQueryHandlerTests()
    {
        _methodsRepository = Substitute.For<IReadRepository<PaymentMethod>>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetPaymentMethodsQueryHandler(_methodsRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WithMappedPaymentMethods()
    {
        var methods = new List<PaymentMethod>
        {
            new("card", "Karta", "Visa", "https://example.com/visa.png"),
            new("blik", "BLIK", "BLIK", "https://example.com/blik.png"),
        };

        var expectedDtos = new List<PaymentMethodDTO>
        {
            new("card", "Karta", "Visa", "https://example.com/visa.png"),
            new("blik", "BLIK", "BLIK", "https://example.com/blik.png"),
        };

        _methodsRepository.ListAsync(CancellationToken.None).Returns(methods);
        _mapper.Map<List<PaymentMethodDTO>>(methods).Returns(expectedDtos);

        var result = await _handler.Handle(new GetPaymentMethodsQuery(), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(expectedDtos);

        await _methodsRepository.Received(1).ListAsync(CancellationToken.None);
        _mapper.Received(1).Map<List<PaymentMethodDTO>>(methods);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken()
    {
        var cts = new CancellationTokenSource();

        _methodsRepository.ListAsync(cts.Token).Returns([]);
        _mapper.Map<List<PaymentMethodDTO>>(Arg.Any<List<PaymentMethod>>()).Returns([]);

        await _handler.Handle(new GetPaymentMethodsQuery(), cts.Token);

        await _methodsRepository.Received(1).ListAsync(cts.Token);
    }
}
