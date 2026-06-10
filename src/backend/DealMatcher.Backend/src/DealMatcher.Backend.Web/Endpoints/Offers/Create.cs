namespace DealMatcher.Backend.Web.Endpoints.Offers;

public class Create(IMediator mediator) : Endpoint<CreateOfferRequest>
{
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

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

        var tags = JsonSerializer.Deserialize<List<string>>(req.Tags, _jsonOptions);
        var properties = JsonSerializer.Deserialize<Dictionary<string, string>>(req.Properties, _jsonOptions);

        if (tags is null || properties is null)
        {
            await SendErrorsAsync(400, ct);
            return;
        }

        var request = new CreateNewOfferCommand(req.Title, req.Description, req.Price, req.Images, tags, req.CategoryId, properties, req.Availability, sellerId);

        var result = await mediator.Send(request, ct);
        await result.SendResult(this, ct);
    }
}
