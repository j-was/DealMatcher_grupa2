using Ardalis.Result;

namespace DealMatcher.Backend.UseCases.Features.Offer.Update;

public sealed record UpdateOfferCommand(
    int OfferId,
    int UserId,
    string? Title,
    string? Description,
    double? Price,
    List<string>? Images,
    List<string>? Tags,
    Dictionary<string, object>? Properties,
    int? Availability
) : IRequest<Result<object>>;
