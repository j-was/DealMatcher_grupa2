namespace DealMatcher.Backend.UseCases.Features.Ban.GetActivity;

public sealed record GetOfferActivityQuery(int OfferId, int UserId) : IQuery<Result<List<ActivityRecordDTO>>>;
