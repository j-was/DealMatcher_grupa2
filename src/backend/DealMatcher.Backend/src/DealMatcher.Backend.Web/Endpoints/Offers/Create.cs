using System.Security.Claims;
using System.Text.Json;
using DealMatcher.Backend.UseCases.Features.Offer.Create;

namespace DealMatcher.Backend.Web.Endpoints.Offers;

public class Create(IMediator mediator) : Endpoint<CreateOfferRequest>
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
    public override void Configure()
    {
        AllowFileUploads();
        Version(1);
        Post("/offers");
        Summary(s =>
        {
            s.Summary = "Create a new offer";
        });
    }

    public override async Task HandleAsync(CreateOfferRequest req, CancellationToken ct)
    {
        var userIdRaw = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdRaw, out var sellerId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var createOfferDTO = JsonSerializer.Deserialize<CreateOfferDTO>(req.Data, _jsonOptions);

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
            createOfferDTO.Availability,
            sellerId);

        var result = await mediator.Send(request, ct);
        await result.SendResult(this, ct);
    }
}
