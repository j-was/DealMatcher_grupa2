namespace DealMatcher.Backend.Web.Endpoints.Ban;

public sealed class DeleteRequest
{
    [RouteParam]
    public int BanId { get; set; }
}
