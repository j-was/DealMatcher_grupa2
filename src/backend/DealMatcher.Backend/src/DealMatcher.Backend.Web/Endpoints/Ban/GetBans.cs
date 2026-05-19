using DealMatcher.Backend.UseCases.Features.Admin.GetBans;

namespace DealMatcher.Backend.Web.Endpoints.Ban;

public sealed class GetBans(IMediator mediator) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Version(1);
        Get("/bans");
        Summary(s =>
        {
            s.Summary = "Get bans list";
            s.Description = "Returns list of bans (admin only)";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userIdRaw = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdRaw, out var adminId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        int? userId = null;
        bool? active = null;

        var userIdQuery = HttpContext.Request.Query["userId"].FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(userIdQuery))
        {
            if (!int.TryParse(userIdQuery, out var parsedUserId))
            {
                await SendAsync(new { error = "Invalid userId query parameter." }, 400, ct);
                return;
            }

            userId = parsedUserId;
        }

        var activeQuery = HttpContext.Request.Query["active"].FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(activeQuery))
        {
            if (!bool.TryParse(activeQuery, out var parsedActive))
            {
                await SendAsync(new { error = "Invalid active query parameter." }, 400, ct);
                return;
            }

            active = parsedActive;
        }

        var query = new GetBansQuery(adminId, userId, active);

        var result = await mediator.Send(query, ct);

        await result.SendResult(this, ct);
    }
}
