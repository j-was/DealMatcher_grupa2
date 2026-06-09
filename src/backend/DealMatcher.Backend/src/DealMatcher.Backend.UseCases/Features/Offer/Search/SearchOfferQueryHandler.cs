namespace DealMatcher.Backend.UseCases.Features.Offer.Search;

public sealed class SearchOfferQueryHandler(IReadRepository<OfferEntity> offersRepository, IMapper mapper)
    : IQueryHandler<SearchOfferQuery, Result<List<OfferDTO>>>
{
    public async Task<Result<List<OfferDTO>>> Handle(SearchOfferQuery request, CancellationToken cancellationToken)
    {
        var offers = await offersRepository.ListAsync(cancellationToken);
        var searchedOffers = offers.Where(o => ValidOffer(o, request)).Take(request.Limit).ToList();
        if (searchedOffers.Count == 0)
        {
            return Result.NoContent();
        }

        var response = mapper.Map<List<OfferDTO>>(searchedOffers);

        if (response.Count == 0)
        {
            return Result.NoContent();
        }

        return Result.Success(response);
    }

    private static bool ValidOffer(OfferEntity offer, SearchOfferQuery request)
    {
        return ValidCategory(offer, request)
               && ValidPrice(offer, request)
               && ValidSearchPhrase(offer, request)
               && ValidTags(offer, request)
               && ValidProperties(offer, request)
               && offer.Status == OfferStatus.Active;
    }

    private static bool ValidCategory(OfferEntity offer, SearchOfferQuery request)
        => request.CategoryId is null || offer.CategoryId == request.CategoryId;

    private static bool ValidPrice(OfferEntity offer, SearchOfferQuery request)
        => (request.MinPrice is null || offer.Price >= (decimal)request.MinPrice)
           && (request.MaxPrice is null || offer.Price <= (decimal)request.MaxPrice);

    private static bool ValidSearchPhrase(OfferEntity offer, SearchOfferQuery request)
    {
        if (string.IsNullOrWhiteSpace(request.SearchPhrase))
        {
            return true;
        }

        var phrase = request.SearchPhrase.Trim();

        return offer.Title.Contains(phrase, StringComparison.OrdinalIgnoreCase) ||
               offer.Description.Contains(phrase, StringComparison.OrdinalIgnoreCase);
    }

    private static bool ValidTags(OfferEntity offer, SearchOfferQuery request)
    {
        if (request.Tags is null || request.Tags.Count == 0)
        {
            return true;
        }

        var offerTags = offer.Tags.Select(tag => tag.Trim()).Where(t => !string.IsNullOrWhiteSpace(t))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        return request.Tags.Any(tag => offerTags.Contains(tag.Trim()));
    }

    private static bool ValidProperty(string offerProperty, List<string> requestedValues)
    {
        if (requestedValues.Count == 2
            && decimal.TryParse(requestedValues[0], out var min)
            && decimal.TryParse(requestedValues[1], out var max)
            && decimal.TryParse(offerProperty, out var offerVal))
        {
            if (min < max)
            {
                return offerVal >= min && offerVal <= max;
            }

            return false;
        }

        return requestedValues.Any(v =>
            string.Equals(v?.Trim(), offerProperty?.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    private static bool ValidProperties(OfferEntity offer, SearchOfferQuery request)
    {
        if (request.Properties is null || request.Properties.Count == 0)
            return true;

        if (offer.Properties is null || offer.Properties.Count == 0)
            return false;

        foreach (var reqProperty in request.Properties)
        {
            var propertyName = reqProperty.Key;
            var reqValues = reqProperty.Value;

            if (string.IsNullOrWhiteSpace(propertyName))
                continue;

            if (reqValues is null || reqValues.Count == 0)
                continue;

            var offerProperty =
                offer.Properties.FirstOrDefault(pr => pr.PropertyId.Equals(propertyName, StringComparison.OrdinalIgnoreCase)
                );

            if (offerProperty is null)
            {
                return false;
            }

            if (!ValidProperty(offerProperty.Value, reqValues))
            {
                return false;
            }
        }

        return true;
    }
}
