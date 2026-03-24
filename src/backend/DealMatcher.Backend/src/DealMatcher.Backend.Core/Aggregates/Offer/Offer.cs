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
      List<string> tags,
      int categoryId,
      List<OfferProperty> properties,
      int availability)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);
        ArgumentOutOfRangeException.ThrowIfNegative(sellerId);
        ArgumentOutOfRangeException.ThrowIfNegative(categoryId);
        ArgumentOutOfRangeException.ThrowIfNegative(availability);

        if (imageUrls is null || imageUrls.Count < OfferConstants.MinImagesCount)
            throw new ArgumentException(
                $"Offer must have at least {OfferConstants.MinImagesCount} image.",
                nameof(imageUrls));

        if (imageUrls.Count > OfferConstants.MaxImagesCount)
            throw new ArgumentException(
                $"Offer cannot have more than {OfferConstants.MaxImagesCount} images.",
                nameof(imageUrls));

        if (tags is not null && tags.Count > OfferConstants.MaxTagsCount)
            throw new ArgumentException(
                $"Offer cannot have more than {OfferConstants.MaxTagsCount} tags.",
                nameof(tags));

        Title = title;
        Description = description;
        Price = price;
        ImageUrls = imageUrls;
        SellerId = sellerId;
        Tags = tags ?? [];
        CategoryId = categoryId;
        Availability = availability;
        Status = OfferStatus.Draft;
        UpdatedAt = DateTime.UtcNow;
        Properties = properties ?? [];
    }

#pragma warning disable CS8618
    private Offer() { /* EF */ }
#pragma warning restore CS8618

    public void ChangeStatus(OfferStatus offerStatus)
    {
        Status = offerStatus;
        UpdatedAt = DateTime.UtcNow;
    }
}

public sealed record OfferProperty(string Name, string Value);
