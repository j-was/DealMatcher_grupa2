using DealMatcher.Backend.Core.Aggregates.Ban.DTOs;
using DealMatcher.Backend.UseCases.Features.Ban.CreateBan;

namespace DealMatcher.Backend.Web.Endpoints.Ban;

public sealed class Create(IMediator mediator) : Endpoint<CreateBanDTO>
{
    public override void Configure()
    {
        Version(1);
        Post("/bans");
        Summary(s =>
        {
            s.Summary = "Ban a user";
            s.Description = "Creates a new ban for a user (admin only)";
        });
    }

    public override async Task HandleAsync(CreateBanDTO req, CancellationToken ct)
    {
        var userIdRaw = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdRaw, out var adminId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var command = new CreateBanCommand(adminId, req);

        var result = await mediator.Send(command, ct);

        await result.SendResult(this, ct);
    }
}
