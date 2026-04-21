namespace DealMatcher.Backend.UseCases.Features.User.Offers;

public sealed record GetMyOffersQuery(int UserId) : IRequest<Result<List<OfferDTO>>>;
