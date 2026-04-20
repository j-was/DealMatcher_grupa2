namespace DealMatcher.Backend.Web.Endpoints.Offers;

public sealed record SearchOffersRequest(int? CategoryId, double? MinPrice, double? MaxPrice, List<string>? Tags,
    Dictionary<string, List<string>>? Properties, string? SearchPhrase, int Limit);
