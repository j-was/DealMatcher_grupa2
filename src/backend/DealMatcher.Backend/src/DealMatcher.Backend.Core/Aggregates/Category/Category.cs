namespace DealMatcher.Backend.Core.Aggregates.Category;

public sealed class Category :
  DealMatcherEntityBase,
  IAggregateRoot
{
  public string Name { get; private set; }
  public string Description { get; private set; }

  public Category(
    string name,
    string description)
  {
    Name = name;
    Description = description;
  }
  
#pragma warning disable CS8618
  private Category() { /* EF */ }
#pragma warning restore CS8618
}
