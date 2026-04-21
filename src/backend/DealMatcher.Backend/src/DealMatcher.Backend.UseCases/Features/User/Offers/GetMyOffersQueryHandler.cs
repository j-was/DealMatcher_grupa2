namespace DealMatcher.Backend.UseCases.Features.User.Offers;

public sealed class GetMyOffersQueryHandler(
    IRepository<OfferEntity> offerRepository,
    IMapper mapper
) : IRequestHandler<GetMyOffersQuery, List<OfferDTO>>
{
    public async Task<List<OfferDTO>> Handle(GetMyOffersQuery request, CancellationToken cancellationToken)
    {
        var spec = new OffersBySellerIdSpec(request.UserId);
        var offers = await offerRepository.ListAsync(spec, cancellationToken);
        return [.. offers.Select(mapper.Map<OfferDTO>)];
    }
}
