using Ardalis.Result;

namespace DealMatcher.Backend.UseCases.Features.Offer.Delete;

public sealed record DeleteOfferCommand(
    int OfferId,
    int UserId
) : IRequest<Result>;
