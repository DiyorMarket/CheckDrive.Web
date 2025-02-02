using CheckDrive.Web.Constants;

namespace CheckDrive.Web.Services.CookieHandler;

public sealed class CookieHandler(IHttpContextAccessor httpContextAccessor) : ICookieHandler
{
    private static readonly CookieOptions cookieOptions = new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
    };

    public (string? AccessToken, string? RefreshToken) GetTokens()
    {
        var cookies = httpContextAccessor.HttpContext?.Request?.Cookies;

        if (cookies is null)
        {
            return (string.Empty, string.Empty);
        }

        var accessToken = cookies[HeaderConstants.AccessTokenHeader];
        var refreshToken = cookies[HeaderConstants.RefreshTokenHeader];

        return (accessToken, refreshToken);
    }

    public void UpdateTokens(string accessToken, string refreshToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        ArgumentException.ThrowIfNullOrEmpty(refreshToken);

        var cookies = httpContextAccessor.HttpContext?.Response?.Cookies;

        if (cookies is null)
        {
            return;
        }

        cookies.Append(HeaderConstants.AccessTokenHeader, accessToken, cookieOptions);
        cookies.Append(HeaderConstants.RefreshTokenHeader, refreshToken, cookieOptions);
    }

    public void ClearTokens()
    {
        var cookies = httpContextAccessor.HttpContext?.Response?.Cookies;

        if (cookies is null)
        {
            return;
        }

        cookies.Delete(HeaderConstants.AccessTokenHeader);
        cookies.Delete(HeaderConstants.RefreshTokenHeader);
    }
}
