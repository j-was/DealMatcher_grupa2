namespace DealMatcher.Backend.UseCases.Features.Admin.GetActivity;

public sealed record GetUserActivityQuery(int UserId, int RequestingUserId) : IQuery<Result<List<ActivityRecordDTO>>>;

