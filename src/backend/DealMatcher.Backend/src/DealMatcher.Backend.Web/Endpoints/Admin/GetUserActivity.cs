namespace DealMatcher.Backend.Web.Endpoints.Admin;

public class GetUserActivity(IMediator mediator) : Endpoint<GetUserActivityRequest>
{
    public override void Configure()
    {
        Version(1);
        Get("/admin/activity/user/{UserId:int}");
        Summary(s =>
        {
            s.Summary = "Get user activity (admin)";
            s.Description = "Returns activity history for a specific user";
        });
    }

    public override async Task HandleAsync(GetUserActivityRequest req, CancellationToken ct)
    {
        var userIdRaw = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                        ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdRaw, out var userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var request = new GetUserActivityQuery(req.UserId, userId);
        var result = await mediator.Send(request, ct);

        await result.SendResult(this, ct);
    }
}
