namespace DealMatcher.Backend.UseCases.Features.Offer.Search;

public sealed record SearchOfferQuery(int? CategoryId, double? MinPrice, double? MaxPrice, List<string>? Tags,
    Dictionary<string, List<string>>? Properties, string? SearchPhrase, int Limit) : IQuery<Result<List<OfferDTO>>>;
