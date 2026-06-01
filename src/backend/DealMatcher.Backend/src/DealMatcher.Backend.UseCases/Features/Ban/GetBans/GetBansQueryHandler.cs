namespace DealMatcher.Backend.UseCases.Features.Ban.GetBans;

public sealed class GetBansQueryHandler(
    IReadRepository<BanEntity> bansRepository,
    IReadRepository<UserEntity> usersRepository,
    IMapper mapper
) : IQueryHandler<GetBansQuery, Result<List<BanDTO>>>
{
    public async Task<Result<List<BanDTO>>> Handle(GetBansQuery request, CancellationToken cancellationToken)
    {
        var admin = await usersRepository.GetByIdAsync(request.AdminId, cancellationToken);

        if (admin is null || admin.Status != UserStatus.Admin)
        {
            return Result.Forbidden();
        }

        var bans = await bansRepository.ListAsync(cancellationToken);

        if (request.UserId.HasValue)
        {
            bans = [.. bans.Where(b => b.UserId == request.UserId.Value)];
        }

        if (request.Active.HasValue)
        {
            bans = [.. bans.Where(b => b.IsActive == request.Active.Value)];
        }

        var items = bans
            .OrderByDescending(b => b.Id)
            .Select(mapper.Map<BanDTO>)
            .ToList();

        return Result.Success(items);
    }
}
