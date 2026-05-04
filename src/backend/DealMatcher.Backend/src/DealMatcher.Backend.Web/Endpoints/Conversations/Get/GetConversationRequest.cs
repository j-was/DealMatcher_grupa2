namespace DealMatcher.Backend.Web.Endpoints.Conversation.Get;

public sealed class GetConversationRequest
{
    [BindFrom("ConversationId")]
    public int ConversationId { get; set; }
}
