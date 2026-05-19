using DealMatcher.Backend.UseCases.Features.Ban.DTOs;

namespace DealMatcher.Backend.UseCases.Features.Ban.GetUsers;

public sealed class GetUsersQueryHandler(IReadRepository<UserEntity> usersRepository, IMapper mapper) : IQueryHandler<GetUsersQuery, Result<AdminUsersDTO>>
{
    public async Task<Result<AdminUsersDTO>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var user = await usersRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null || user.Status != UserStatus.Admin)
        {
            return Result.Forbidden();
        }

        var page = request.Page < 1 ? 1 : request.Page;
        var limit = request.Limit < 1 ? 20 : request.Limit;

        var users = await usersRepository.ListAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            users = [.. users.Where(user => string.Equals(
                user.Status.Value, request.Status, StringComparison.OrdinalIgnoreCase
            ))];
        }

        var total = users.Count;

        var pagedUsers = users.OrderBy(user => user.Id).Skip((page - 1) * limit)
        .Take(limit).ToList();

        var items = new List<UserDTO>();

        foreach (var u in pagedUsers)
        {
            var userDTO = mapper.Map<UserDTO>(u);

            items.Add(userDTO);
        }

        var response = new AdminUsersDTO(items, total, page, (int)Math.Ceiling(total / (double)limit));

        return Result.Success(response);
    }
}
