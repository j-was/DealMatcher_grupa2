using DealMatcher.Backend.Core.Aggregates.Ban.DTOs;

namespace DealMatcher.Backend.UseCases.Features.Admin.GetBans;

public sealed record GetBansQuery(int AdminId, int? UserId, bool? Active)
    : IQuery<Result<List<BanDTO>>>;
