namespace DealMatcher.Backend.Web.Endpoints.Ban;

public sealed class GetBanRequest
{
    [BindFrom("banId")]
    public int BanId { get; set; }
}
