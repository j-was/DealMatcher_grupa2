namespace DealMatcher.Backend.UseCases.Features.Admin.GetUsers;

public class GetUserAdminQueryHandler(
    IRepository<UserEntity> usersRepository,
    IRepository<OfferEntity> offersRepository,
    IRepository<ActivityRecord> activityRepository) : IRequestHandler<GetUserAdminQuery, Result<UserDetailsDTO>>
{
    public async Task<Result<UserDetailsDTO>> Handle(GetUserAdminQuery request, CancellationToken ct)
    {
        var requestingUser = await usersRepository.GetByIdAsync(request.RequestingUserId, ct);

        if (requestingUser is null || requestingUser.Status != UserStatus.Admin)
        {
            return Result.Forbidden();
        }

        var user = await usersRepository.GetByIdAsync(request.UserId, ct);

        if (user is null)
        {
            return Result.NotFound("Nie znaleziono użytkownika");
        }

        var spec2 = new OffersBySellerIdSpec(request.UserId);
        var offers = await offersRepository.ListAsync(spec2, ct);

        var spec = new ActivityRecordsByUserIdSpec(request.UserId);
        var records = await activityRepository.ListAsync(spec, ct);
        var totalSales = 0;
        foreach (var offer in offers)
        {
            var spec3 = new ActivityRecordsByOfferIdSpec(offer.Id);
            var records3 = await activityRepository.ListAsync(spec3, ct);
            if (records3.Count != 0)
            {
                totalSales += records3.Where(a => a.Action == ActionType.Purchase).Count();
            }
        }

        var totalPurchases = 0;
        if (records.Count != 0)
        {
            totalPurchases = records.Where(a => a.Action == ActionType.Purchase).Count();
        }

        var lastActivityAt = records
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefault()
            ?.CreatedAt;

        var response = new UserDetailsDTO(user.Id, user.Email, user.Name, user.Surname, user.Status.Value.ToUpper(),
            user.CreatedAt, offers.Count, totalSales, totalPurchases, lastActivityAt ?? DateTime.UtcNow);

        return Result.Success(response);
    }
}
