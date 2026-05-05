namespace DealMatcher.Backend.UseCases.Extensions;

public class RedirectResult
{
    public string Url { get; }
    public bool IsPermanent { get; }

    public RedirectResult(string url, bool isPermanent = false)
    {
        Url = url ?? throw new ArgumentNullException(nameof(url));
        IsPermanent = isPermanent;
    }
}

public static class RedirectResultExtensions
{
    public static RedirectResult ToRedirect(this string url, bool permanent = false)
        => new(url, permanent);
}
