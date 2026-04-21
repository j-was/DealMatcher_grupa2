namespace DealMatcher.Backend.Core.Aggregates.Category.Specifications;

public sealed class CategoryByNameSpec : SingleResultSpecification<CategoryEnity>
{
    public CategoryByNameSpec(string categoryName)
    {
        Query.Where(c => c.Name == categoryName);
    }
}
