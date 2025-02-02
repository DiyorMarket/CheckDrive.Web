namespace CheckDrive.Web.Services.CookieHandler;

public interface ICookieHandler
{
    void UpdateTokens(string accessToken, string refreshToken);
    void ClearTokens();
    (string? AccessToken, string? RefreshToken) GetTokens();
}
