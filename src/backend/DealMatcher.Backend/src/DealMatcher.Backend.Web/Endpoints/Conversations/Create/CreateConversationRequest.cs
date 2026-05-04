namespace DealMatcher.Backend.Web.Endpoints.Conversations;

public sealed record CreateConversationRequest(int OfferId, string InitialMessage);