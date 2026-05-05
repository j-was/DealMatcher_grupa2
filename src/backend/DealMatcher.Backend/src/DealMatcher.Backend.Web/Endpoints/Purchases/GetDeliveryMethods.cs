namespace DealMatcher.Backend.Web.Endpoints.Purchases;

public class GetDeliveryMethods(IMediator mediator) : EndpointWithoutRequest<List<DeliveryMethodDTO>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Version(1);
        Get("/purchases/delivery-methods");
        Summary(s =>
        {
            s.Summary = "Get available delivery methods";
            s.Description = "Returns all available delivery options";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var request = new GetDeliveryMethodsQuery();
        var result = await mediator.Send(request, ct);

        await result.SendResult(this, ct: ct);
    }
}
