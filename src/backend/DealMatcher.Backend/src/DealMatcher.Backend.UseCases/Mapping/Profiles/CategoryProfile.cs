using DealMatcher.Backend.Core.Aggregates.Category;

namespace DealMatcher.Backend.UseCases.Mapping.Profiles;

public sealed class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CategoryProperty, CategoryPropertyDTO>();

        CreateMap<Category, CategoryDTO>()
            .ForCtorParam(nameof(CategoryDTO.Id), opt => opt.MapFrom(src => src.Id))
            .ForCtorParam(nameof(CategoryDTO.Name), opt => opt.MapFrom(src => src.Name))
            .ForCtorParam(nameof(CategoryDTO.Description), opt => opt.MapFrom(src => src.Description));
    }
}
