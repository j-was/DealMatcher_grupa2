using DealMatcher.Backend.UseCases.Features.Ban.GetBanById;

namespace DealMatcher.Backend.Web.Endpoints.Ban;

public sealed class GetBan(IMediator mediator) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Version(1);
        Get("/bans/{banId}");
        Summary(s =>
        {
            s.Summary = "Get ban details";
            s.Description = "Returns detailed information about a specific ban (admin only)";
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

        var banId = Route<int>("banId");

        var query = new GetBanByIdQuery(adminId, banId);

        var result = await mediator.Send(query, ct);

        await result.SendResult(this, ct);
    }
}
