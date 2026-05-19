using DealMatcher.Backend.UseCases.Features.Admin.DeleteBan;

namespace DealMatcher.Backend.Web.Endpoints.Bans;

public sealed class Delete(IMediator mediator) : Endpoint<DeleteRequest>
{
    public override void Configure()
    {
        Version(1);
        Delete("/ban/{banId}");
        Summary(s =>
        {
            s.Summary = "Remove a ban";
            s.Description = "Removes a ban from a user (admin only)";
        });
    }

    public override async Task HandleAsync(DeleteRequest req, CancellationToken ct)
    {
        var userIdRaw = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdRaw, out var adminId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var command = new DeleteBanCommand(adminId, req.BanId);

        var result = await mediator.Send(command, ct);

        await result.SendResult(this, ct);
    }
}