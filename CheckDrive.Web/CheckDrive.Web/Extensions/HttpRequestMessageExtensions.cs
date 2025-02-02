namespace CheckDrive.Web.Extensions;

public static class HttpRequestMessageExtensions
{
    private static readonly string[] excludedPaths = ["auth", "login", "register"];

    public static bool ShouldSkipAuthorization(this HttpRequestMessage request)
    {
        var path = request.RequestUri?.AbsolutePath;

        if (string.IsNullOrWhiteSpace(path))
        {
            return true;
        }

        return excludedPaths.Any(x => path.Contains(x, StringComparison.InvariantCultureIgnoreCase));
    }
}
