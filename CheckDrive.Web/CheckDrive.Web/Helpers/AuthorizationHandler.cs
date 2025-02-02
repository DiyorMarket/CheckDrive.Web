using System.Net.Http.Headers;
using CheckDrive.Web.Constants;
using CheckDrive.Web.Extensions;
using CheckDrive.Web.Requests.Auth;
using CheckDrive.Web.Services.CookieHandler;
using CheckDrive.Web.Stores.Auth;

namespace CheckDrive.Web.Helpers;

// Initializes CookieHandler & AuthStore via ServiceProvider due to circular dependency
// AuthorizationHandler -> AuthStore -> CheckDriveApi -> HttpClient -> AuthorizationHandler,
// which means before initialization of AuthorizationHandler we cannot resolve AuthStore.
// Resolving CookieHandler via ServiceProvider as well to keep it consistent and easier to follow.
internal sealed class AuthorizationHandler(IServiceProvider serviceProvider) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.ShouldSkipAuthorization())
        {
            return await base.SendAsync(request, cancellationToken);
        }

        await AddAuthorizationHeaders(request);

        return await base.SendAsync(request, cancellationToken);
    }

    private async Task AddAuthorizationHeaders(HttpRequestMessage request)
    {
        var accessToken = await GetAccessToken();

        request.Headers.Authorization = new AuthenticationHeaderValue(HeaderConstants.AuthenticationSchema, accessToken);
    }

    private async Task<string> GetAccessToken()
    {
        using var scope = serviceProvider.CreateScope();
        var cookieHandler = scope.ServiceProvider.GetRequiredService<ICookieHandler>();
        var authStore = scope.ServiceProvider.GetRequiredService<IAuthStore>();

        var (accessToken, refreshToken) = cookieHandler.GetTokens();

        if (!JwtHelper.IsTokenValid(accessToken) && !string.IsNullOrWhiteSpace(refreshToken))
        {
            var response = await authStore.RefreshTokenAsync(new RefreshTokenRequest(refreshToken));

            return response.AccessToken;
        }

        return accessToken ?? string.Empty;
    }
}
