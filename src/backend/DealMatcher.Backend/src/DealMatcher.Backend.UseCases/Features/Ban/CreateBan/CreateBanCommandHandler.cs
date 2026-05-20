using DealMatcher.Backend.Core.Aggregates.Ban.DTOs;

namespace DealMatcher.Backend.UseCases.Features.Ban.CreateBan;

public sealed class CreateBanCommandHandler(IRepository<BanEntity> bansRepository,
IReadRepository<UserEntity> usersRepository, IMapper mapper) : ICommandHandler<CreateBanCommand, Result<BanDTO>>
{
    public async Task<Result<BanDTO>> Handle(CreateBanCommand request, CancellationToken cancellationToken)
    {
        var user = await usersRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null || user.Status != UserStatus.Admin)
        {
            return Result.Forbidden();
        }

        var userToBan = await usersRepository.GetByIdAsync(request.Ban.UserId, cancellationToken);

        if (userToBan is null)
        {
            return Result.NotFound();
        }

        userToBan.UpdateStatus(UserStatus.Banned);

        var bans = await bansRepository.ListAsync(cancellationToken);

        if (bans.Any(b => b.UserId == request.Ban.UserId && b.IsActive))
        {
            return Result.Conflict("User is already banned.");
        }

        var ban = new BanEntity(request.Ban.UserId, request.Ban.Reason, request.UserId, request.Ban.ExpiresAt);

        await bansRepository.AddAsync(ban, cancellationToken);

        return Result.Success(mapper.Map<BanDTO>(ban));
    }
}
