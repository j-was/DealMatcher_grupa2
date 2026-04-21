using DealMatcher.Backend.Core.Aggregates.Category;

namespace DealMatcher.Backend.UseCases.Features.Offer.Get;

public sealed class GetOfferByIdQueryHandler(
    IReadRepository<OfferEntity> offersRepository,
    IReadRepository<UserEntity> usersRepository,
    IReadRepository<CategoryEnity> categoriesRepository,
    IMapper mapper) :
    IQueryHandler<GetOfferByIdQuery, Result<OfferDTO>>
{
    public async Task<Result<OfferDTO>> Handle(GetOfferByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new OfferByIdSpec(request.OfferId);
        var offer = await offersRepository.SingleOrDefaultAsync(spec, cancellationToken);
        if (offer is null)
        {
            return Result.NotFound();
        }

        var seller = await usersRepository.GetByIdAsync(offer.SellerId, cancellationToken);
        var category = await categoriesRepository.GetByIdAsync(offer.CategoryId, cancellationToken);

        var offerDTO = mapper.Map<OfferDTO>(offer);

        if (seller is not null && category is not null)
        {
            offerDTO = mapper.Map<OfferDTO>(new OfferProfile.OfferInfo(offer, seller, category));
        }

        return Result.Success(offerDTO);
    }
}
