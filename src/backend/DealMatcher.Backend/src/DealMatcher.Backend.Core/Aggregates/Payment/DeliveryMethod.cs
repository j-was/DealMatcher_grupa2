namespace DealMatcher.Backend.Core.Aggregates.Payment;

public sealed class DeliveryMethod :
  DealMatcherEntityBase,
  IAggregateRoot
{
    public string StringId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public int EstimatedDays { get; private set; }

    public DeliveryMethod(string id, string name, string description, decimal price, int estimatedDays)
    {
        ValidateName(name);
        ValidateDescription(description);
        ValidatePrice(price);
        ValidateEstimatedDays(estimatedDays);

        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        StringId = id;
        Name = name.Trim();
        Description = description.Trim();
        Price = price;
        EstimatedDays = estimatedDays;
    }

#pragma warning disable CS8618
    private DeliveryMethod()
    {
        /* EF */
    }
#pragma warning restore CS8618

    private static void ValidateName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (name.Length <= 0 || name.Length > DataSchemaConstants.DeliveryMethodNameMaxLength)
            throw new ArgumentException($"Name cannot exceed {DataSchemaConstants.DeliveryMethodNameMaxLength} characters.");
    }

    private static void ValidateDescription(string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        if (description.Length <= 0 || description.Length > DataSchemaConstants.DeliveryMethodDescriptionMaxLength)
            throw new ArgumentException($"Description cannot exceed {DataSchemaConstants.DeliveryMethodDescriptionMaxLength} characters.");
    }

    private static void ValidatePrice(decimal price)
    {
        if (price < 0)
            throw new ArgumentException("Price cannot be negative.");
    }

    private static void ValidateEstimatedDays(int days)
    {
        if (days < 0)
            throw new ArgumentException("Estimated days cannot be negative.");
    }
}