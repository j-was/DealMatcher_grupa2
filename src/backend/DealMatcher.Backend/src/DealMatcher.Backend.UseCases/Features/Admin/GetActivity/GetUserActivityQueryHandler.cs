namespace DealMatcher.Backend.UseCases.Features.Admin.GetActivity;

public class GetUserActivityQueryHandler(IReadRepository<UserEntity> usersRepository, IReadRepository<ActivityRecord> activityRepository, IMapper mapper) : IQueryHandler<GetUserActivityQuery, Result<List<ActivityRecordDTO>>>
{

    public async Task<Result<List<ActivityRecordDTO>>> Handle(GetUserActivityQuery request, CancellationToken cancellationToken)
    {
        var user = await usersRepository.GetByIdAsync(request.RequestingUserId, cancellationToken);

        if (user is null || user.Status != UserStatus.Admin)
        {
            return Result.Forbidden();
        }

        var spec = new ActivityRecordsByUserIdSpec(request.UserId);
        var records = await activityRepository.ListAsync(spec, cancellationToken);
        if (records.Count == 0)
        {
            return Result.NotFound();
        }

        var response = mapper.Map<List<ActivityRecordDTO>>(records);

        return Result.Success(response);
    }
}
