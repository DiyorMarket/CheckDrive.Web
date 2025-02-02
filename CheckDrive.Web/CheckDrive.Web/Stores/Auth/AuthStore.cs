using CheckDrive.Web.Requests.Auth;
using CheckDrive.Web.Responses.Auth;
using CheckDrive.Web.Services;
using CheckDrive.Web.Services.CookieHandler;

namespace CheckDrive.Web.Stores.Auth;

public sealed class AuthStore(CheckDriveApi clientApi, ICookieHandler cookieHandler) : IAuthStore
{
    public async Task<TokenResponse> LoginAsync(LoginRequest request)
    {
        var response = await clientApi.PostAsync<LoginRequest, TokenResponse>("auth/login", request);

        cookieHandler.UpdateTokens(response.AccessToken, response.RefreshToken);

        return response;
    }

    public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var response = await clientApi.PostAsync<RefreshTokenRequest, TokenResponse>("auth/refresh-token", request);

        cookieHandler.UpdateTokens(response.AccessToken, response.RefreshToken);

        return response;
    }
}
