namespace DealMatcher.Backend.Web.Endpoints.Conversations.SendMessage;

public class SendMessage(IMediator mediator, IHubContext<ConversationHub> hubContext, ILogger<SendMessage> logger)
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
            try
            {
                await hubContext.Clients
                    .Group($"conversation:{conversationId}")
                    .SendAsync("message.created", result.Value, ct);
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "Failed to broadcast message");
            }

            await SendAsync(result.Value, StatusCodes.Status201Created, ct);
            return;
        }

        await result.SendResult(this, ct: ct);
    }
}
