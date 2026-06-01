namespace DealMatcher.Backend.UseCases.Features.Category.Properties.Get;

public sealed record GetAllPropertiesByCategoryNameQuery(string CategoryName)
    : IQuery<Result<List<CategoryPropertyDTO>>>;
