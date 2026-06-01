namespace DealMatcher.Backend.Web.Endpoints.Categories.Properties;

public class Get(IMediator mediator)
    : Endpoint<GetPropertiesRequest, List<CategoryPropertyDTO>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Version(1);
        Get("/categories/{CategoryName}/properties");
        Summary(s =>
        {
            s.Summary = "Get properties for a specific category";
            s.Description = "Returns all configurable properties for the given category";
        });
    }

    public override async Task HandleAsync(GetPropertiesRequest req, CancellationToken ct)
    {
        var request = new GetAllPropertiesByCategoryNameQuery(req.CategoryName);
        var result = await mediator.Send(request, ct);

        await result.SendResult(this, ct: ct);
    }
}
