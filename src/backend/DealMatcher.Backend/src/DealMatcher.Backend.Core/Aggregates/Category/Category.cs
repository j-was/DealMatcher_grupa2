namespace DealMatcher.Backend.Core.Aggregates.Category;

public sealed class CategoryEnity :
    DealMatcherEntityBase,
    IAggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public List<CategoryProperty> Properties { get; private set; }

    public CategoryEnity(string name, string description, List<CategoryProperty>? properties = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (name.Length < DataSchemaConstants.CategoryNameMinLength)
            throw new ArgumentException($"Category name must be at least {DataSchemaConstants.CategoryNameMinLength} characters.");

        if (name.Length > DataSchemaConstants.CategoryNameMaxLength)
            throw new ArgumentException($"Category name cannot exceed {DataSchemaConstants.CategoryNameMaxLength} characters.");

        if (description?.Length > DataSchemaConstants.CategoryDescriptionMaxLength)
            throw new ArgumentException($"Category description cannot exceed {DataSchemaConstants.CategoryDescriptionMaxLength} characters.");

        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        Properties = properties ?? [];

        ValidateProperties(Properties);
    }

#pragma warning disable CS8618
    private CategoryEnity() { /* EF */ }
#pragma warning restore CS8618

    private static void ValidateProperties(IEnumerable<CategoryProperty> properties)
    {
        var list = properties.ToList();

        if (list.Any(p => string.IsNullOrWhiteSpace(p.Name)))
            throw new ArgumentException("Category property name cannot be empty.");

        var duplicate = list
            .GroupBy(p => p.Name.Trim(), StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault(g => g.Count() > 1);

        if (duplicate is not null)
            throw new ArgumentException(
                $"Duplicate property name in category: '{duplicate.Key}'.");
    }
    public void UpdateDetails(string name, string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (name.Length < DataSchemaConstants.CategoryNameMinLength)
            throw new ArgumentException($"Category name must be at least {DataSchemaConstants.CategoryNameMinLength} characters.");

        if (name.Length > DataSchemaConstants.CategoryNameMaxLength)
            throw new ArgumentException($"Category name cannot exceed {DataSchemaConstants.CategoryNameMaxLength} characters.");

        if (description?.Length > DataSchemaConstants.CategoryDescriptionMaxLength)
            throw new ArgumentException($"Category description cannot exceed {DataSchemaConstants.CategoryDescriptionMaxLength} characters.");

        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
    }
}
