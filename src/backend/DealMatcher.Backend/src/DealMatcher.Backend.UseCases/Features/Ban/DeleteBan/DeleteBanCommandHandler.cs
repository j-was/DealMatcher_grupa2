using DealMatcher.Backend.Core.Aggregates.Ban.Specifications;

namespace DealMatcher.Backend.UseCases.Features.Admin.DeleteBan;

public sealed class DeleteBanCommandHandler(
    IRepository<BanEntity> bansRepository,
    IReadRepository<UserEntity> usersRepository
) : ICommandHandler<DeleteBanCommand, Result>
{
    public async Task<Result> Handle(DeleteBanCommand request, CancellationToken cancellationToken)
    {
        var admin = await usersRepository.GetByIdAsync(request.AdminId, cancellationToken);

        if (admin is null || admin.Status != UserStatus.Admin)
        {
            return Result.Forbidden();
        }

        var ban = await bansRepository.FirstOrDefaultAsync(new BanByIdSpec(request.BanId), cancellationToken);

        if (ban is null)
        {
            return Result.NotFound();
        }

        var bannedUser = await usersRepository.GetByIdAsync(ban.UserId, cancellationToken);

        bannedUser!.UpdateStatus(UserStatus.Active);

        await bansRepository.DeleteAsync(ban, cancellationToken);

        return Result.Success();
    }
}