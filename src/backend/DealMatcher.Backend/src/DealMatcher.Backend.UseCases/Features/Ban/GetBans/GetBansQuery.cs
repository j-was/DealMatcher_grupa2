namespace DealMatcher.Backend.UseCases.Features.Ban.GetBans;

public sealed record GetBansQuery(int AdminId, int? UserId, bool? Active)
    : IQuery<Result<List<BanDTO>>>;
