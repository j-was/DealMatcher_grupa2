using Microsoft.AspNetCore.Http;

namespace DealMatcher.Backend.UseCases.Features.Offer.Create;

public sealed record CreateNewOfferCommand(
    string Title,
    string Description,
    double Price,
    List<IFormFile> Images,
    List<string> Tags,
    int CategoryId,
    Dictionary<string, string> Properties,
    int Availability,
    int SellerId
) : ICommand<Result<OfferDTO>>;
