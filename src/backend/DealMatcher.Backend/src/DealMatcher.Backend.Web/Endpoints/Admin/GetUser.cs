using DealMatcher.Backend.UseCases.Features.User.Get;
using GetUserQuery = DealMatcher.Backend.UseCases.Features.User.Get.GetUserQuery;

namespace DealMatcher.Backend.Web.Endpoints.Admin;

public class GetUser(IMediator mediator) : Endpoint<GetUserRequest>
{
    public override void Configure()
    {
        Version(1);
        Get("/admin/users/{UserId:int}");
        Summary(s =>
        {
            s.Summary = "Get users for admin";
            s.Description = "Returns paginated users list for admin";
        });
    }

    public override async Task HandleAsync(GetUserRequest req, CancellationToken ct)
    {
        var userIdRaw = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                        ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdRaw, out var userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var request = new GetUserAdminQuery(req.UserId, userId);
        var result = await mediator.Send(request, ct);

        await result.SendResult(this, ct);
    }
}
