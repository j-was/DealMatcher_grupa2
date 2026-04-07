using DealMatcher.Backend.Core.Aggregates.Offer;

namespace DealMatcher.Backend.UseCases.Features.Offer.Create;

public sealed class CreateNewOfferCommandHandler(IRepository<OfferEntity> offersRepository, IReadRepository<CategoryEntity> categoriesRepository, IMapper mapper)
: ICommandHandler<CreateNewOfferCommand, Result<OfferDTO>>
{
    public async Task<Result<OfferDTO>> Handle(CreateNewOfferCommand request, CancellationToken cancellationToken)
    {
        var category = await categoriesRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            return Result.NotFound("Nie znaleziono kategorii");
        }

        var properties = request.Properties
            .Select(p => new OfferProperty(
                name: p.Key,
                value: p.Value
            ))
            .ToList();

        var offer = new OfferEntity(request.Title, request.Description, (decimal)request.Price, request.Images, 1, request.Tags, 1, properties, request.Availability);


        await offersRepository.AddAsync(offer, cancellationToken);
        await offersRepository.SaveChangesAsync(cancellationToken);

        var dto = mapper.Map<OfferDTO>(offer);

        return Result.Success(dto);
    }
}
