using DealMatcher.Backend.UseCases.Features.Ban.GetUsers;

namespace DealMatcher.Backend.Web.Endpoints.Admin;

public class GetUsers(IMediator mediator) : Endpoint<GetUsersRequest>
{
    public override void Configure()
    {
        Version(1);
        Get("/admin/users");
        Summary(s =>
        {
            s.Summary = "Get users for admin";
            s.Description = "Returns paginated users list for admin";
        });
    }

    public override async Task HandleAsync(GetUsersRequest req, CancellationToken ct)
    {
        var userIdRaw = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                        ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdRaw, out var userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var request = new GetUsersQuery(req.Page, req.Limit, req.Status, userId);
        var result = await mediator.Send(request, ct);

        await result.SendResult(this, ct);
    }
}
