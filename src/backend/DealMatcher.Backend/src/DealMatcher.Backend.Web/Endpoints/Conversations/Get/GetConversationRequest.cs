namespace DealMatcher.Backend.Web.Endpoints.Conversations.Get;

public sealed class GetConversationRequest
{
    [BindFrom("ConversationId")]
    public int ConversationId { get; set; }
}
