namespace DealMatcher.Backend.Core.Aggregates.Category;

public sealed class Category :
    DealMatcherEntityBase,
    IAggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public List<CategoryProperty> Properties { get; private set; }

    public Category(
        string name,
        string description,
        List<CategoryProperty>? properties = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name.Trim();
        Description = description.Trim();
        Properties = properties ?? [];

        ValidateProperties(Properties);
    }

#pragma warning disable CS8618
    private Category() { /* EF */ }
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
}
