namespace DealMatcher.Backend.UseCases.Features.Ban.GetActivity;

public sealed record GetUserActivityQuery(int UserId, int RequestingUserId) : IQuery<Result<List<ActivityRecordDTO>>>;

