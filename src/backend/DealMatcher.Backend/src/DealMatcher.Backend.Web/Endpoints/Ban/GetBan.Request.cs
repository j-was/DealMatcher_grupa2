namespace DealMatcher.Backend.Web.Endpoints.Ban;

public sealed class GetBanRequest
{
    [RouteParam]
    public int BanId { get; set; }
}
