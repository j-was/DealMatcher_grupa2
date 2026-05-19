namespace DealMatcher.Backend.UseCases.Mapping.Profiles;

public sealed class ActivityRecordProfile : Profile
{
    public ActivityRecordProfile()
    {
        CreateMap<ActivityRecord, ActivityRecordDTO>()
            .ForCtorParam(nameof(ActivityRecordDTO.Action),
                opt => opt.MapFrom(src => src.Action.Value.ToUpper()))
            .ForCtorParam(nameof(ActivityRecordDTO.Details),
                opt => opt.MapFrom(src => src.Details.ToDictionary(d => d.Name, d => d.Value)));
    }
}
