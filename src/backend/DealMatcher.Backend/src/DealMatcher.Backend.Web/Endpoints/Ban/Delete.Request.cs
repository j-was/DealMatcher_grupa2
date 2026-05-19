namespace DealMatcher.Backend.Web.Endpoints.Bans;

public sealed class DeleteRequest
{
    [BindFrom("banId")]
    public int BanId { get; set; }
}
