namespace DealMatcher.Backend.Core.Aggregates.Category.Specifications;

public sealed class CategoryByNameSpec : SingleResultSpecification<Category>
{
    public CategoryByNameSpec(string categoryName)
    {
        Query.Where(c => c.Name == categoryName);
    }
}
