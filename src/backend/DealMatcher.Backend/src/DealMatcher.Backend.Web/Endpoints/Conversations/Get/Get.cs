using System.Security.Claims;
using DealMatcher.Backend.Core.Aggregates.Conversation.DTOs;
using DealMatcher.Backend.UseCases.Features.Conversation.Get;

namespace DealMatcher.Backend.Web.Endpoints.Conversations.Get;

public class Get(IMediator mediator)
    : EndpointWithoutRequest<ConversationDetailsDTO>
{
    public override void Configure()
    {
        Version(1);
        Get("/conversations/{ConversationId}");
        Summary(s =>
        {
            s.Summary = "Get conversation details";
            s.Description = "Returns conversation with all messages";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userIdRaw = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdRaw, out var _))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var conversationId = Route<int>("ConversationId");

        var request = new GetConversationQuery(conversationId);
        var result = await mediator.Send(request, ct);

        await result.SendResult(this, ct: ct);
    }
}
