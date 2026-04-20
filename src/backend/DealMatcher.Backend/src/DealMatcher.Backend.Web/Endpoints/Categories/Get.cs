using DealMatcher.Backend.Core.Aggregates.Category.DTOs;
using DealMatcher.Backend.UseCases.Features.Category.Get;

namespace DealMatcher.Backend.Web.Endpoints.Categories;

public class Get(IMediator mediator) : EndpointWithoutRequest<List<CategoryDTO>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Version(1);
        Get("/categories");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var request = new GetAllCategoriesQuery();
        var result = await mediator.Send(request, ct);

        await result.SendResult(this, ct: ct);
    }
}
