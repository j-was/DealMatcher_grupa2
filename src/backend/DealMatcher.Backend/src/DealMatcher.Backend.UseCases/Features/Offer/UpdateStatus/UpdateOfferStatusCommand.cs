namespace DealMatcher.Backend.UseCases.Features.Offer.UpdateStatus;

public sealed record UpdateOfferStatusCommand(
    int OfferId,
    int UserId,
    string Status
) : IRequest<Result<object>>;
