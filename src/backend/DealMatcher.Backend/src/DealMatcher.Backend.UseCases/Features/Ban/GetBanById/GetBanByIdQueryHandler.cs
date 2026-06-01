namespace DealMatcher.Backend.UseCases.Features.Ban.GetBanById;

public sealed class GetBanByIdQueryHandler(
    IReadRepository<BanEntity> bansRepository,
    IReadRepository<UserEntity> usersRepository,
    IMapper mapper
) : IQueryHandler<GetBanByIdQuery, Result<BanDTO>>
{
    public async Task<Result<BanDTO>> Handle(GetBanByIdQuery request, CancellationToken cancellationToken)
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

        return Result.Success(mapper.Map<BanDTO>(ban));
    }
}
