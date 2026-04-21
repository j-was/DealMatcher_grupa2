namespace DealMatcher.Backend.Core.Aggregates.Category;

public sealed class CategoryProperty : DealMatcherEntityBase, IAggregateRoot
{
    public string Name { get; set; }
    public CategoryPropertyType Type { get; set; }
    public List<string>? Options { get; set; }
    public CategoryProperty(string Name, CategoryPropertyType Type, List<string>? Options)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Name);
        if (Type == CategoryPropertyType.Select && (Options == null || Options.Count == 0))
            throw new ArgumentException("Options must be provided for SELECT type properties.");
        this.Name = Name.Trim();
        this.Type = Type;
        this.Options = Options;
    }

#pragma warning disable CS8618
    private CategoryProperty() { /* EF */ }
#pragma warning restore CS8618
}
