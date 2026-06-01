namespace DealMatcher.Backend.Web.Realtime;

public sealed class ConversationHub : Hub
{
    public async Task JoinConversation(int conversationId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"conversation:{conversationId}");
    }

    public async Task LeaveConversation(int conversationId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"conversation:{conversationId}");
    }

    public async Task Typing(int conversationId)
    {
        await Clients.OthersInGroup($"conversation:{conversationId}").SendAsync("typing", conversationId);
    }
}
