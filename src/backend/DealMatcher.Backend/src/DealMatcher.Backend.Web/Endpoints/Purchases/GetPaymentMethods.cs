namespace DealMatcher.Backend.Web.Endpoints.Purchases;

public class GetPaymentMethods(IMediator mediator) : EndpointWithoutRequest<List<PaymentMethodDTO>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Version(1);
        Get("/purchases/payment-methods");
        Summary(s =>
        {
            s.Summary = "Get available payment methods";
            s.Description = "Returns all available payment options";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var request = new GetPaymentMethodsQuery();
        var result = await mediator.Send(request, ct);

        await result.SendResult(this, ct: ct);
    }
}
