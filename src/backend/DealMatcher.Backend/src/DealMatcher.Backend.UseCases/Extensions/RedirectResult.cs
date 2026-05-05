namespace DealMatcher.Backend.UseCases.Extensions;

public class RedirectResult(string url, bool isPermanent = false)
{
    public string Url { get; } = url ?? throw new ArgumentNullException(nameof(url));
    public bool IsPermanent { get; } = isPermanent;
}

public static class RedirectResultExtensions
{
    public static RedirectResult ToRedirect(this string url, bool permanent = false)
        => new(url, permanent);
}
