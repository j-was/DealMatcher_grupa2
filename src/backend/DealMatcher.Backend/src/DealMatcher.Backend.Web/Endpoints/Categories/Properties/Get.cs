using DealMatcher.Backend.Core.Aggregates.Category.DTOs;
using DealMatcher.Backend.UseCases.Features.Category.Properties.Get;

namespace DealMatcher.Backend.Web.Endpoints.Categories.Properties;

public class Get(IMediator mediator)
    : Endpoint<GetPropertiesRequest, List<CategoryPropertyDTO>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Version(1);
        Get("/categories/{CategoryName}/properties");
    }

    public override async Task HandleAsync(GetPropertiesRequest req, CancellationToken ct)
    {
        var request = new GetAllPropertiesByCategoryNameQuery(req.CategoryName);
        var result = await mediator.Send(request, ct);

        await result.SendResult(this, ct: ct);
    }
}
