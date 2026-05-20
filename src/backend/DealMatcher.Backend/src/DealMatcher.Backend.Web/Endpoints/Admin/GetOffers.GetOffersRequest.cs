namespace DealMatcher.Backend.Web.Endpoints.Admin;

public sealed class GetOffersRequest
{
    [QueryParam]
    public int Page { get; set; } = 1;
    [QueryParam]
    public int Limit { get; set; } = 20;
    [QueryParam]
    public string? Status { get; set; }
}
