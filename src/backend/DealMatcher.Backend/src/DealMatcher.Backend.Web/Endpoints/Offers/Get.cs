namespace DealMatcher.Backend.Web.Endpoints.Offers;

public class Get(IMediator mediator) : Endpoint<GetOfferRequest, OfferDTO>
{
    public override void Configure()
    {
        AllowAnonymous();
        Version(1);
        Get("/offers/{OfferId:int}");
    }

    public override async Task HandleAsync(GetOfferRequest req, CancellationToken ct)
    {
        var request = new GetOfferByIdQuery(req.OfferId);
        var result = await mediator.Send(request, ct);

        await result.SendResult(this, ct: ct);
    }
}
