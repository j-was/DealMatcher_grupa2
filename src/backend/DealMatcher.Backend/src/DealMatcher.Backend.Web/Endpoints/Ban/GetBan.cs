using DealMatcher.Backend.UseCases.Features.Admin.GetBanById;

namespace DealMatcher.Backend.Web.Endpoints.Bans;

public sealed class GetBan(IMediator mediator) : Endpoint<GetBanRequest>
{
    public override void Configure()
    {
        Version(1);
        Get("/ban/{banId}");
        Summary(s =>
        {
             s.Summary = "Get ban details";
            s.Description = "Returns detailed information about a specific ban (admin only)";
        });
    }

    public override async Task HandleAsync(GetBanRequest req, CancellationToken ct)
    {
        var userIdRaw = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdRaw, out var adminId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var query = new GetBanByIdQuery(adminId, req.BanId);

        var result = await mediator.Send(query, ct);

        await result.SendResult(this, ct);
    }
}