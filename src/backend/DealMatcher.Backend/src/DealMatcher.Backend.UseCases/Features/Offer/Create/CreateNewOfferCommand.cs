namespace DealMatcher.Backend.UseCases.Features.Offer.Create;

public sealed record CreateNewOfferCommand(
    string Title,
    string Description,
    double Price,
    List<string> Images,
    List<string> Tags,
    int CategoryId,
    Dictionary<string, string> Properties,
    int Availability
) : ICommand<Result<OfferDTO>>;