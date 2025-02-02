using System.Net.Http.Headers;
using CheckDrive.Web.Constants;
using CheckDrive.Web.Extensions;
using CheckDrive.Web.Requests.Auth;
using CheckDrive.Web.Services.CookieHandler;
using CheckDrive.Web.Stores.Auth;

namespace CheckDrive.Web.Helpers;

// Initializes CookieHandler & AuthStore via ServiceProvider due to circular dependency
// AuthorizationHandler -> AuthStore -> ApiClient -> HttpClient -> AuthorizationHandler,
// which means before initialization of AuthorizationHandler we cannot resolve AuthStore.
// Resolving CookieHandler via ServiceProvider as well, to keep it consistent and easier to follow.
internal sealed class AuthorizationHandler(IServiceProvider serviceProvider) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.ShouldSkipAuthorization())
        {
            return await base.SendAsync(request, cancellationToken);
        }

        await TryAddAuthorizationHeadersAsync(request);

        return await base.SendAsync(request, cancellationToken);
    }

    private async Task TryAddAuthorizationHeadersAsync(HttpRequestMessage request)
    {
        using var scope = serviceProvider.CreateScope();
        var cookieHandler = scope.ServiceProvider.GetRequiredService<ICookieHandler>();
        var authStore = scope.ServiceProvider.GetRequiredService<IAuthStore>();

        var (accessToken, refreshToken) = cookieHandler.GetTokens();

        if (JwtHelper.IsValid(accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(HeaderConstants.AuthenticationSchema, accessToken);

            return;
        }

        if (!string.IsNullOrWhiteSpace(refreshToken))
        {
            var response = await authStore.RefreshTokenAsync(new RefreshTokenRequest(refreshToken));

            request.Headers.Authorization = new AuthenticationHeaderValue(HeaderConstants.AuthenticationSchema, response.AccessToken);
        }
    }
}
