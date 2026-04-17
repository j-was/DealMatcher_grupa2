using System.Text.Json;
using DealMatcher.Backend.UseCases.Features.Offer.Create;

namespace DealMatcher.Backend.Web.Endpoints.Offers;

public class Create(IMediator mediator) : Endpoint<CreateOfferRequest>
{
    public override void Configure()
    {
        AllowAnonymous();
        AllowFileUploads();
        Version(1);
        Post("/offers");
    }

    public override async Task HandleAsync(CreateOfferRequest req, CancellationToken ct)
    {
        var createOfferDTO = JsonSerializer.Deserialize<CreateOfferDTO>(req.Data,
        new JsonSerializerOptions {PropertyNameCaseInsensitive = true});

        if (createOfferDTO is null)
        {
            await SendErrorsAsync(400, ct);
            return;
        }

        var request = new CreateNewOfferCommand(createOfferDTO.Title,
            createOfferDTO.Description,
            createOfferDTO.Price,
            req.Images,
            createOfferDTO.Tags,
            createOfferDTO.CategoryId,
            createOfferDTO.Properties,
            createOfferDTO.Availability);

        var result = await mediator.Send(request, ct);
        await result.SendResult(this, ct);
    }
}
