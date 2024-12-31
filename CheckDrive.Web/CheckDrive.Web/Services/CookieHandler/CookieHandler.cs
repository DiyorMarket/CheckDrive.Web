namespace CheckDrive.Web.Services.CookieHandler;

public sealed class CookieHandler : ICookieHandler
{
    private const string AccessTokenHeader = "access_token";
    private const string RefreshTokenHeader = "refresh_token";

    private static readonly CookieOptions cookieOptions = new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
    };

    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public void UpdateTokens(string accessToken, string refreshToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        ArgumentException.ThrowIfNullOrEmpty(refreshToken);

        var cookies = _httpContextAccessor.HttpContext?.Response?.Cookies;

        if (cookies is null)
        {
            return;
        }

        cookies.Append(AccessTokenHeader, accessToken, cookieOptions);
        cookies.Append(RefreshTokenHeader, refreshToken, cookieOptions);
    }

    public void ClearTokens()
    {
        var cookies = _httpContextAccessor.HttpContext?.Response?.Cookies;

        if (cookies is null)
        {
            return;
        }

        cookies.Delete(AccessTokenHeader);
        cookies.Delete(RefreshTokenHeader);
    }

    public (string? AccessToken, string? RefreshToken) GetTokens()
    {
        var cookies = _httpContextAccessor.HttpContext?.Request?.Cookies;

        if (cookies is null)
        {
            return (string.Empty, string.Empty);
        }

        var accessToken = cookies[AccessTokenHeader];
        var refreshToken = cookies[RefreshTokenHeader];

        return (accessToken, refreshToken);
    }
}
