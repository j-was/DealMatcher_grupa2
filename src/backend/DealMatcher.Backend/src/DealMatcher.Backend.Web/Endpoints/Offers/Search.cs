using DealMatcher.Backend.UseCases.Features.Offer.Search;

namespace DealMatcher.Backend.Web.Endpoints.Offers;

public class Search(IMediator mediator) : Endpoint<SearchOffersRequest, List<OfferDTO>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Version(1);
        Post("/offers/search");
        Summary(s =>
        {
            s.Summary = "Search offers";
            s.Description = "Creates a search query and returns matching offers";
        });
    }

    public override async Task HandleAsync(SearchOffersRequest req, CancellationToken ct)
    {
        var request = new SearchOfferQuery(req.CategoryId, req.MinPrice, req.MaxPrice, req.Tags, req.Properties, req.SearchPhrase, req.Limit);
        var result = await mediator.Send(request, ct);

        await result.SendResult(this, ct: ct);
    }
}
