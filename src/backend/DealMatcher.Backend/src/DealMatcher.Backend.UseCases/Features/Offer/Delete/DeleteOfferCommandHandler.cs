using Ardalis.Result;
using Ardalis.SharedKernel;
using DealMatcher.Backend.Core.Aggregates.Offer;

namespace DealMatcher.Backend.UseCases.Features.Offer.Delete;

public sealed class DeleteOfferCommandHandler(
    IRepository<OfferEntity> offersRepository
) : IRequestHandler<DeleteOfferCommand, Result>
{
    public async Task<Result> Handle(DeleteOfferCommand request, CancellationToken cancellationToken)
    {
        var offer = await offersRepository.GetByIdAsync(request.OfferId, cancellationToken);

        if (offer is null)
        {
            return Result.NotFound();
        }

        if (offer.SellerId != request.UserId)
        {
            return Result.Forbidden();
        }

        offer.ChangeStatus(OfferStatus.Deleted);

        await offersRepository.UpdateAsync(offer, cancellationToken);
        await offersRepository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
