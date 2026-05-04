using System.Security.Claims;
using DealMatcher.Backend.Core.Aggregates.Conversation.DTOs;
using DealMatcher.Backend.UseCases.Features.Conversation.Create;

namespace DealMatcher.Backend.Web.Endpoints.Conversations;

public class Create(IMediator mediator) : Endpoint<CreateConversationRequest, ConversationDTO>
{
    public override void Configure()
    {
        Version(1);
        Post("/conversations");
        Summary(s =>
        {
            s.Summary = "Create a new conversation";
            s.Description = "Creates a conversation between buyer and seller";
        });
    }

    public override async Task HandleAsync(CreateConversationRequest req, CancellationToken ct)
    {
        var userIdRaw = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdRaw, out var buyerId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var request = new CreateNewConversationCommand(req.OfferId, buyerId, req.InitialMessage);
        var result = await mediator.Send(request, ct);

        if (result.IsSuccess)
        {
            await SendAsync(result.Value, StatusCodes.Status201Created, ct);
        }
        else
        {
            await result.SendResult(this, ct);
        }
    }
}