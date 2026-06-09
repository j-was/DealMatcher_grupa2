namespace DealMatcher.Backend.Core.Aggregates.Offer;

public sealed class Offer :
    DealMatcherEntityBase,
    IAggregateRoot
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public List<string> ImageUrls { get; private set; }
    public int SellerId { get; private set; }
    public List<string> Tags { get; private set; }
    public int CategoryId { get; private set; }
    public List<OfferProperty> Properties { get; private set; }
    public int Availability { get; private set; }
    public OfferStatus Status { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Offer(
        string title,
        string description,
        decimal price,
        List<string> imageUrls,
        int sellerId,
        List<string>? tags,
        int categoryId,
        List<OfferProperty>? properties,
        int availability)
    {
        ValidateTitle(title);
        ValidateDescription(description);
        ValidatePrice(price);
        ValidateImageUrls(imageUrls);
        ValidateTags(tags);
        ValidateAvailability(availability);

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sellerId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(categoryId);

        Title = title.Trim();
        Description = description.Trim();
        Price = price;
        ImageUrls = imageUrls;
        SellerId = sellerId;
        Tags = tags ?? [];
        CategoryId = categoryId;
        Availability = availability;
        Status = OfferStatus.Active;
        UpdatedAt = DateTime.UtcNow;
        Properties = properties ?? [];

        ValidateProperties(Properties);
    }

#pragma warning disable CS8618
    private Offer()
    {
        /* EF */
    }
#pragma warning restore CS8618

    private static void ValidateTitle(string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        if (title.Length < DataSchemaConstants.TitleMinLength)
            throw new ArgumentException($"Title must be at least {DataSchemaConstants.TitleMinLength} characters.");

        if (title.Length > DataSchemaConstants.TitleMaxLength)
            throw new ArgumentException($"Title cannot exceed {DataSchemaConstants.TitleMaxLength} characters.");
    }

    private static void ValidateDescription(string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        if (description.Length < DataSchemaConstants.DescriptionMinLength)
            throw new ArgumentException(
                $"Description must be at least {DataSchemaConstants.DescriptionMinLength} characters.");

        if (description.Length > DataSchemaConstants.DescriptionMaxLength)
            throw new ArgumentException(
                $"Description cannot exceed {DataSchemaConstants.DescriptionMaxLength} characters.");
    }

    private static void ValidatePrice(decimal price)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(price, DataSchemaConstants.PriceMinValue);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(price, DataSchemaConstants.PriceMaxValue);
    }

    private static void ValidateImageUrls(List<string> imageUrls)
    {
        if (imageUrls.Count < DataSchemaConstants.MinImagesCount)
            throw new ArgumentException($"Offer must have at least {DataSchemaConstants.MinImagesCount} images.");

        if (imageUrls.Count > DataSchemaConstants.MaxImagesCount)
            throw new ArgumentException($"Offer cannot have more than {DataSchemaConstants.MaxImagesCount} images.");

        if (imageUrls.Any(url => url.Length > DataSchemaConstants.ImageUrlMaxLength))
            throw new ArgumentException(
                $"Image URLs cannot exceed {DataSchemaConstants.ImageUrlMaxLength} characters.");
    }

    private static void ValidateTags(List<string>? tags)
    {
        if (tags == null) return;

        if (tags.Count > DataSchemaConstants.MaxTagsCount)
            throw new ArgumentException($"Offer cannot have more than {DataSchemaConstants.MaxTagsCount} tags.");

        if (tags.Any(tag => tag.Length > DataSchemaConstants.TagMaxLength))
            throw new ArgumentException($"Tags cannot exceed {DataSchemaConstants.TagMaxLength} characters.");
    }

    private static void ValidateAvailability(int availability)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(availability, DataSchemaConstants.AvailabilityMinValue);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(availability, DataSchemaConstants.AvailabilityMaxValue);
    }

    private static void ValidateProperties(List<OfferProperty> properties)
    {
        var duplicate = properties
            .GroupBy(p => p.PropertyId, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault(g => g.Count() > 1);

        if (duplicate != null)
            throw new ArgumentException($"Duplicate property id: '{duplicate.Key}'.");
    }

    public void ChangeStatus(OfferStatus offerStatus)
    {
        Status = offerStatus;
        UpdatedAt = DateTime.UtcNow;
    }

    public void DecreaseAvailability(int availabilityDecrease)
    {
        Availability -= availabilityDecrease;
        if (Availability <= 0)
        {
            ChangeStatus(OfferStatus.Sold);
        }
    }

    public void Update(
        string? title = null,
        string? description = null,
        decimal? price = null,
        List<string>? images = null,
        List<string>? tags = null,
        List<OfferProperty>? properties = null,
        int? availability = null)
    {
        if (title is not null)
        {
            ValidateTitle(title);
            Title = title.Trim();
        }

        if (description is not null)
        {
            ValidateDescription(description);
            Description = description.Trim();
        }

        if (price is not null)
        {
            ValidatePrice(price.Value);
            Price = price.Value;
        }

        if (images is not null)
        {
            var unknownImages = images
               .Where(url => !ImageUrls.Contains(url))
               .ToList();

            if (unknownImages.Count > 0)
                throw new ArgumentException("Some images are not assigned to this offer.");

            var updatedImages = ImageUrls
                .Where(url => !images.Contains(url))
                .ToList();

            ValidateImageUrls(updatedImages);
            ImageUrls = updatedImages;
        }

        if (tags is not null)
        {
            ValidateTags(tags);
            Tags = tags;
        }

        if (properties is not null)
        {
            ValidateProperties(properties);
            Properties = properties;
        }

        if (availability is not null)
        {
            ValidateAvailability(availability.Value);
            Availability = availability.Value;
        }

        ChangeStatus(OfferStatus.Active);
    }
}
