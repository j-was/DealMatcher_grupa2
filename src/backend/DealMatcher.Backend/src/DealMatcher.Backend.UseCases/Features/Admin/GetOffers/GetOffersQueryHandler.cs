using DealMatcher.Backend.UseCases.Features.Admin.DTOs;

namespace DealMatcher.Backend.UseCases.Features.Admin.GetOffers;

public sealed class GetOffersQueryHandler(
    IReadRepository<OfferEntity> offersRepository,
    IReadRepository<CategoryEntity> categoriesRepository,
    IReadRepository<UserEntity> usersRepository,
    IMapper mapper) : IQueryHandler<GetOffersQuery, Result<AdminOffersDTO>>
{
    public async Task<Result<AdminOffersDTO>> Handle(GetOffersQuery request, CancellationToken cancellationToken)
    {
        var user = await usersRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null || user.Status != UserStatus.Admin)
        {
            return Result.Forbidden();
        }

        var page = request.Page < 1 ? 1 : request.Page;
        var limit = request.Limit < 1 ? 20 : request.Limit;

        var offers = await offersRepository.ListAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            offers =
            [
                .. offers.Where(offer => string.Equals(
                    offer.Status.Value.ToUpper(), request.Status, StringComparison.OrdinalIgnoreCase
                ))
            ];
        }

        var total = offers.Count;

        var pagedOffers = offers.OrderBy(offer => offer.Id).Skip((page - 1) * limit)
            .Take(limit).ToList();

        var items = new List<OfferDTO>();

        foreach (var offer in pagedOffers)
        {
            var seller = await usersRepository.GetByIdAsync(
                offer.SellerId,
                cancellationToken);

            var category = await categoriesRepository.GetByIdAsync(
                offer.CategoryId,
                cancellationToken);

            var offerDTO = mapper.Map<OfferDTO>(offer);

            if (seller is not null && category is not null)
            {
                offerDTO = mapper.Map<OfferDTO>(new OfferProfile.OfferInfo(offer, seller, category));
            }

            items.Add(offerDTO);
        }

        var response = new AdminOffersDTO(items, total, page, (int)Math.Ceiling(total / (double)limit));

        return Result.Success(response);
    }
}
