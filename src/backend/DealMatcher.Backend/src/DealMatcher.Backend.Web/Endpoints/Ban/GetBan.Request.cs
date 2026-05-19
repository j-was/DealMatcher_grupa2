namespace DealMatcher.Backend.Web.Endpoints.Bans;

public sealed class GetBanRequest
{
    [BindFrom("banId")]
    public int BanId {get; set;}
}