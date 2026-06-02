namespace DealMatcher.Backend.UseCases.Features.Category.Properties.Get;

public sealed class GetAllPropertiesByCategoryNameQueryHandler(
    IReadRepository<CategoryEntity> categoriesRepository,
    IMapper mapper)
    : IQueryHandler<GetAllPropertiesByCategoryNameQuery, Result<List<CategoryPropertyDTO>>>
{
    public async Task<Result<List<CategoryPropertyDTO>>> Handle(
        GetAllPropertiesByCategoryNameQuery request,
        CancellationToken cancellationToken)
    {
        var spec = new CategoryByNameSpec(request.CategoryName);
        var category = await categoriesRepository.SingleOrDefaultAsync(spec, cancellationToken);

        if (category is null)
        {
            return Result.NotFound();
        }

        var properties = mapper.Map<List<CategoryPropertyDTO>>(category.Properties);

        return Result.Success(properties);
    }
}
