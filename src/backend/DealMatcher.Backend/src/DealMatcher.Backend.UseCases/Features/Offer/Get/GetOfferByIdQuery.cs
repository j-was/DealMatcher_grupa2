namespace DealMatcher.Backend.UseCases.Features.Offer.Get;

public sealed record GetOfferByIdQuery(int OfferId) : IQuery<Result<OfferDTO>>;
