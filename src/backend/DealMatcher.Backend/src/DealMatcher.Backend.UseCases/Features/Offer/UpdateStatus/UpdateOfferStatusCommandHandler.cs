using Ardalis.Result;
using Ardalis.SharedKernel;
using AutoMapper;
using DealMatcher.Backend.Core.Aggregates.Offer;

namespace DealMatcher.Backend.UseCases.Features.Offer.UpdateStatus;

public sealed class UpdateOfferStatusCommandHandler(
    IRepository<OfferEntity> offersRepository,
    IMapper mapper
) : IRequestHandler<UpdateOfferStatusCommand, Result<object>>
{
    public async Task<Result<object>> Handle(UpdateOfferStatusCommand request, CancellationToken cancellationToken)
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

        var newStatus = request.Status.Trim().ToUpperInvariant() switch
        {
            "ACTIVE" => OfferStatus.Active,
            "PROMOTED" => OfferStatus.Promoted,
            "SOLD" => OfferStatus.Sold,
            _ => null
        };

        if (newStatus is null)
        {
            return Result.Invalid(new ValidationError("Invalid status value"));
        }

        offer.ChangeStatus(newStatus);

        await offersRepository.UpdateAsync(offer, cancellationToken);
        await offersRepository.SaveChangesAsync(cancellationToken);

        var dto = mapper.Map<OfferDTO>(offer);
        return Result.Success(dto);
    }
}
