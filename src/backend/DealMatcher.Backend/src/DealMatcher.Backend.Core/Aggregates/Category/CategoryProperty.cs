namespace DealMatcher.Backend.Core.Aggregates.Category;

public sealed class CategoryProperty : DealMatcherEntityBase, IAggregateRoot
{
    public string Name { get; set; }
    public CategoryPropertyType Type { get; set; }
    public List<string>? Options { get; set; }
    public CategoryProperty(string name, CategoryPropertyType type, List<string>? options)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (name.Length < DataSchemaConstants.PropertyNameMinLength)
            throw new ArgumentException($"Property name must be at least {DataSchemaConstants.PropertyNameMinLength} characters.");

        if (name.Length > DataSchemaConstants.PropertyNameMaxLength)
            throw new ArgumentException($"Property name cannot exceed {DataSchemaConstants.PropertyNameMaxLength} characters.");

        if (type == CategoryPropertyType.Select && (options == null || options.Count == 0))
            throw new ArgumentException("Options must be provided for SELECT type properties.");

        if (options != null && options.Count > DataSchemaConstants.MaxPropertyOptions)
            throw new ArgumentException($"Property cannot have more than {DataSchemaConstants.MaxPropertyOptions} options.");

        if (options != null && options.Any(o => o.Length > DataSchemaConstants.PropertyOptionMaxLength))
            throw new ArgumentException($"Option values cannot exceed {DataSchemaConstants.PropertyOptionMaxLength} characters.");

        Name = name.Trim();
        Type = type;
        Options = options?.Select(o => o.Trim()).ToList();
    }

#pragma warning disable CS8618
    private CategoryProperty() { /* EF */ }
#pragma warning restore CS8618
}
