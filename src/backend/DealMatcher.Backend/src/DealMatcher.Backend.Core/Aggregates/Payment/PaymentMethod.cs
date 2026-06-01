namespace DealMatcher.Backend.Core.Aggregates.Payment;

public sealed class PaymentMethod :
    DealMatcherEntityBase,
    IAggregateRoot
{
    public string StringId { get; private set; }
    public string Name { get; private set; }
    public string Provider { get; private set; }
    public string Icon { get; private set; }


    public PaymentMethod(string id, string name, string provider, string icon)
    {
        ValidateName(name);
        ValidateProvider(provider);
        ValidateIcon(icon);

        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        StringId = id.Trim();
        Name = name.Trim();
        Provider = provider.Trim();
        Icon = icon.Trim();
    }

#pragma warning disable CS8618
    private PaymentMethod()
    {
        /* EF */
    }
#pragma warning restore CS8618
    private static void ValidateName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (name.Length <= 0 || name.Length > DataSchemaConstants.PaymentMethodNameMaxLength)
            throw new ArgumentException(
                $"Name cannot exceed {DataSchemaConstants.PaymentMethodNameMaxLength} characters.");
    }

    private static void ValidateProvider(string provider)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(provider);

        if (provider.Length <= 0 || provider.Length > DataSchemaConstants.PaymentMethodProviderMaxLength)
            throw new ArgumentException(
                $"Surname cannot exceed {DataSchemaConstants.PaymentMethodProviderMaxLength} characters.");
    }

    private static void ValidateIcon(string icon)
    {
        if (string.IsNullOrWhiteSpace(icon)) return;

        if (icon.Length > DataSchemaConstants.ImageUrlMaxLength)
            throw new ArgumentException($"Surname cannot exceed {DataSchemaConstants.ImageUrlMaxLength} characters.");
    }
}
