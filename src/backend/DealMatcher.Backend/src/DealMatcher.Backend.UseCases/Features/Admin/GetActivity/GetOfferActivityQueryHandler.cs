namespace DealMatcher.Backend.UseCases.Features.Admin.GetActivity;

public sealed class GetOfferActivityQueryHandler(
    IReadRepository<ActivityRecord> activityRepository,
    IReadRepository<UserEntity> usersRepository,
    IMapper mapper) : IQueryHandler<GetOfferActivityQuery, Result<List<ActivityRecordDTO>>>
{
    public async Task<Result<List<ActivityRecordDTO>>> Handle(GetOfferActivityQuery request,
        CancellationToken cancellationToken)
    {
        var user = await usersRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null || user.Status != UserStatus.Admin)
        {
            return Result.Forbidden();
        }

        var spec = new ActivityRecordsByOfferIdSpec(request.OfferId);
        var records = await activityRepository.ListAsync(spec, cancellationToken);
        if (records.Count == 0)
        {
            return Result.NotFound();
        }

        var response = mapper.Map<List<ActivityRecordDTO>>(records);

        return Result.Success(response);
    }
}
