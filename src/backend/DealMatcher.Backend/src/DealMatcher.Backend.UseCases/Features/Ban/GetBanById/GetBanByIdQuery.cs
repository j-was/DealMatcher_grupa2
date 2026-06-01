namespace DealMatcher.Backend.UseCases.Features.Ban.GetBanById;

public sealed record GetBanByIdQuery(int AdminId, int BanId)
    : IQuery<Result<BanDTO>>;
