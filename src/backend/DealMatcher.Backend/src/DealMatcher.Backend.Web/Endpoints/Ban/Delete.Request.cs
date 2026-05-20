namespace DealMatcher.Backend.Web.Endpoints.Ban;

public sealed class DeleteRequest
{
    [BindFrom("banId")]
    public int BanId { get; set; }
}
