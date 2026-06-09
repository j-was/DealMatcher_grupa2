namespace DealMatcher.Backend.UseCases.Features.Offer.Create;

public sealed class CreateNewOfferCommandHandler(IRepository<OfferEntity> offersRepository, IReadRepository<CategoryEntity> categoriesRepository, IImageService imageService, IMapper mapper, IPublisher publisher)
: ICommandHandler<CreateNewOfferCommand, Result<OfferDTO>>
{
    public async Task<Result<OfferDTO>> Handle(CreateNewOfferCommand request, CancellationToken cancellationToken)
    {
        var category = await categoriesRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            return Result.Invalid(new ValidationError("Category not found"));
        }

        var properties = request.Properties
            .Select(p => new OfferProperty(
                propertyId: p.Key,
                value: p.Value
            ))
            .ToList();

        var imagesUrls = new List<string>();

        if (request.Images is not null && request.Images.Count > 0)
        {
            imagesUrls = await imageService.UploadMultipleImagesAsync(request.Images, cancellationToken);
        }

        var offer = new OfferEntity(request.Title, request.Description, (decimal)request.Price, imagesUrls, request.SellerId, request.Tags, request.CategoryId, properties, request.Availability);


        await offersRepository.AddAsync(offer, cancellationToken);
        await offersRepository.SaveChangesAsync(cancellationToken);

        var dto = mapper.Map<OfferDTO>(offer);

        await publisher.Publish(new OfferCreatedEvent(offer.SellerId, offer.Id, offer.Title, offer.Description,
            offer.Price, offer.Availability), cancellationToken);

        return Result.Created(dto);
    }
}
