namespace DealMatcher.Backend.UseCases.Features.User.Offers;

public sealed class GetMyOffersQueryHandler(
    IRepository<OfferEntity> offerRepository,
    IMapper mapper
) : IRequestHandler<GetMyOffersQuery, Result<List<OfferDTO>>>
{
    public async Task<Result<List<OfferDTO>>> Handle(GetMyOffersQuery request, CancellationToken cancellationToken)
    {
        var spec = new OffersBySellerIdSpec(request.UserId);
        var offers = await offerRepository.ListAsync(spec, cancellationToken);
        if (offers.Count == 0)
        {
            return Result.NoContent();
        }

        var offersDto = mapper.Map<List<OfferDTO>>(offers);

        return Result.Success(offersDto);
    }
}
