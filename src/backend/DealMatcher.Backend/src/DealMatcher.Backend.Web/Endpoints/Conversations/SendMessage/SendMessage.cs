using System.Security.Claims;
using DealMatcher.Backend.Core.Aggregates.Conversation.DTOs;
using DealMatcher.Backend.UseCases.Features.Conversation.SendMessage;
using DealMatcher.Backend.Web.Realtime;
using Microsoft.AspNetCore.SignalR;

namespace DealMatcher.Backend.Web.Endpoints.Conversations.SendMessage;

public class SendMessage(IMediator mediator, IHubContext<ConversationHub> hubContext)
    : Endpoint<SendMessageRequest, MessageDTO>
{
    public override void Configure()
    {
        Version(1);
        Post("/conversations/{ConversationId}/messages");
    }

    public override async Task HandleAsync(SendMessageRequest req, CancellationToken ct)
    {
        var userIdRaw = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdRaw, out var senderId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var conversationId = Route<int>("ConversationId");

        var request = new SendMessageInConversationCommand(
            conversationId,
            senderId,
            req.Content);

        var result = await mediator.Send(request, ct);

        if (result.IsSuccess)
        {
            await hubContext.Clients
                .Group($"conversation:{conversationId}")
                .SendAsync("message.created", result.Value, ct);

            await SendAsync(result.Value, StatusCodes.Status201Created, ct);
            return;
        }

        await result.SendResult(this, ct: ct);
    }
}
