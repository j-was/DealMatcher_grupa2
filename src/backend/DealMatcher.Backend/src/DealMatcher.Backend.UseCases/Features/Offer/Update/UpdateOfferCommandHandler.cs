using Ardalis.Result;
using Ardalis.SharedKernel;
using AutoMapper;
using DealMatcher.Backend.Core.Aggregates.Offer;

namespace DealMatcher.Backend.UseCases.Features.Offer.Update;

public sealed class UpdateOfferCommandHandler(
    IRepository<OfferEntity> offersRepository,
    IMapper mapper
) : IRequestHandler<UpdateOfferCommand, Result<object>>
{
    public async Task<Result<object>> Handle(UpdateOfferCommand request, CancellationToken cancellationToken)
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

        if (request.Title is null
            && request.Description is null
            && request.Price is null
            && request.Images is null
            && request.Tags is null
            && request.Properties is null
            && request.Availability is null)
        {
            return Result.Invalid(new ValidationError("No update data provided"));
        }

        var properties = request.Properties?
            .Select(p => new OfferProperty(p.Key, p.ToString()))
            .ToList();

        offer.Update(
            title: request.Title,
            description: request.Description,
            price: request.Price is null ? null : (decimal?)request.Price.Value,
            images: request.Images,
            tags: request.Tags,
            properties: properties,
            availability: request.Availability);

        await offersRepository.UpdateAsync(offer, cancellationToken);
        await offersRepository.SaveChangesAsync(cancellationToken);

        var dto = mapper.Map<OfferDTO>(offer);
        return Result.Success(dto);
    }
}
