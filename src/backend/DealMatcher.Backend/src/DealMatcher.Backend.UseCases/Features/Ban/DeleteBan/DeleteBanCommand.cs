namespace DealMatcher.Backend.UseCases.Features.Admin.DeleteBan;

public sealed record DeleteBanCommand(int AdminId, int BanId) : ICommand<Result>;
