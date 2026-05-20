using DealMatcher.Backend.UseCases.Features.Ban.DeleteBan;

namespace DealMatcher.Backend.Web.Endpoints.Ban;

public sealed class Delete(IMediator mediator) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Version(1);
        Delete("/bans/{banId}");
        Summary(s =>
        {
            s.Summary = "Remove a ban";
            s.Description = "Removes a ban from a user (admin only)";
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

        var command = new DeleteBanCommand(adminId, banId);

        var result = await mediator.Send(command, ct);

        await result.SendResult(this, ct);
    }
}
