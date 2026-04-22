using DealMatcher.Backend.Core.Aggregates.Category;

namespace DealMatcher.Backend.UseCases.Features.Category.Get;

public sealed class GetAllCategoriesHandler(
    IReadRepository<CategoryEntity> categoriesRepository,
    IMapper mapper) :
    IQueryHandler<GetAllCategoriesQuery, Result<List<CategoryDTO>>>
{
    public async Task<Result<List<CategoryDTO>>> Handle(
        GetAllCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var categories = await categoriesRepository.ListAsync(cancellationToken);

        var categoryDtos = mapper.Map<List<CategoryDTO>>(categories);

        return Result.Success(categoryDtos);
    }
}
