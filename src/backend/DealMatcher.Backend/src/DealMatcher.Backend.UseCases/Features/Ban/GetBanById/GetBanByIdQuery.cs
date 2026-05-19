using DealMatcher.Backend.Core.Aggregates.Ban.DTOs;

namespace DealMatcher.Backend.UseCases.Features.Admin.GetBanById;

public sealed record GetBanByIdQuery(int AdminId, int BanId)
    : IQuery<Result<BanDTO>>;
