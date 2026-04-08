namespace DealMatcher.Backend.UseCases.Features.Category.Get;

public sealed record GetAllCategoriesQuery : IQuery<Result<List<CategoryDTO>>>;
