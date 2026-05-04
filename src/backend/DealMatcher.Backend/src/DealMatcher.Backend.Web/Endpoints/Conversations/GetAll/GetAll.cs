using System.Security.Claims;
using DealMatcher.Backend.Core.Aggregates.Conversation.DTOs;
using DealMatcher.Backend.UseCases.Features.Conversation.GetAll;

namespace DealMatcher.Backend.Web.Endpoints.Conversations;

public class GetAll(IMediator mediator) : EndpointWithoutRequest<List<ConversationDTO>>
{
    public override void Configure()
    {
        Version(1);
        Get("/conversations");
        Summary(s =>
        {
            s.Summary = "Get user conversations";
            s.Description = "Returns all conversations for the authenticated user";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userIdRaw = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdRaw, out var userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var request = new GetAllConversationsQuery(userId);
        var result = await mediator.Send(request, ct);

        await result.SendResult(this, ct);
    }
}