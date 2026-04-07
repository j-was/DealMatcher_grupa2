using DealMatcher.Backend.UseCases.Features.Offer.Create;

namespace DealMatcher.Backend.Web.Endpoints.Offers;

public class Create(IMediator mediator) : Endpoint<CreateOfferRequest>
{
    public override void Configure()
    {
        AllowAnonymous();
        Version(1);
        Post("/offers");
    }

    public override async Task HandleAsync(CreateOfferRequest req, CancellationToken ct)
    {
        var request = new CreateNewOfferCommand(req.Title,
            req.Description,
            req.Price,
            req.Images,
            req.Tags,
            req.CategoryId,
            req.Properties,
            req.Availability);

        var result = await mediator.Send(request, ct);
        await result.SendResult(this, ct);
    }
}
