using CheckDrive.Web.Helpers;
using CheckDrive.Web.Services.CookieHandler;

namespace CheckDrive.Web.Services.CurrentUserService;

internal sealed class CurrentUserService : ICurrentUserService
{
    private readonly ICookieHandler _cookieHandler;

    public CurrentUserService(ICookieHandler cookieHandler)
    {
        _cookieHandler = cookieHandler ?? throw new ArgumentNullException(nameof(cookieHandler));
    }

    public string GetUserId()
    {
        var (accessToken, _) = _cookieHandler.GetTokens();

        if (string.IsNullOrEmpty(accessToken))
        {
            return string.Empty;
        }

        return JwtHelper.GetUserId(accessToken);
    }

    public string GetAccountId()
    {
        var (accessToken, _) = _cookieHandler.GetTokens();

        if (string.IsNullOrEmpty(accessToken))
        {
            return string.Empty;
        }

        return JwtHelper.GetAccountId(accessToken);
    }

    public string GetRole()
    {
        var (accessToken, _) = _cookieHandler.GetTokens();

        if (string.IsNullOrEmpty(accessToken))
        {
            return string.Empty;
        }

        return JwtHelper.GetUserRole(accessToken);
    }
}
