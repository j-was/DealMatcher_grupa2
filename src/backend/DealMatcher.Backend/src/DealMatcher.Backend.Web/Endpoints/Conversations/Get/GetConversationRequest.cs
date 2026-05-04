namespace DealMatcher.Backend.Web.Endpoints.Conversations;

public sealed class GetConversationRequest
{
    [BindFrom("ConversationId")]
    public int ConversationId { get; set; }
}