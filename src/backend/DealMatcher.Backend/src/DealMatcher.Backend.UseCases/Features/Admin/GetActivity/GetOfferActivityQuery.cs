namespace DealMatcher.Backend.UseCases.Features.Admin.GetActivity;

public sealed record GetOfferActivityQuery(int OfferId, int UserId) : IQuery<Result<List<ActivityRecordDTO>>>;
