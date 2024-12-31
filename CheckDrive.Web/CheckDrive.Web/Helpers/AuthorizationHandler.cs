using CheckDrive.Web.Requests.Auth;
using CheckDrive.Web.Services.CookieHandler;
using CheckDrive.Web.Stores.Auth;
using System.Net.Http.Headers;

namespace CheckDrive.Web.Helpers;

internal sealed class AuthorizationHandler : DelegatingHandler
{
    // Initializes CookieHandler & AuthStore via ServiceProvider due to circular dependency
    // AuthorizationHandler -> AuthStore -> CheckDriveApi -> HttpClient -> AuthorizationHandler,
    // which means before initialization of AuthorizationHandler we cannot resolve AuthStore.
    // Resolving CookieHandler via ServiceProvider as well to keep it consistent and easier to follow.
    private readonly IServiceProvider _serviceProvider;

    public AuthorizationHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var cookieHandler = scope.ServiceProvider.GetRequiredService<ICookieHandler>();
        var authStore = scope.ServiceProvider.GetRequiredService<IAuthStore>();

        var (accessToken, refreshToken) = cookieHandler.GetTokens();

        if (!JwtHelper.IsTokenValid(accessToken) && !string.IsNullOrEmpty(refreshToken))
        {
            var response = await authStore.RefreshTokenAsync(new RefreshTokenRequest(refreshToken));
            cookieHandler.UpdateTokens(response.AccessToken, response.RefreshToken);

            accessToken = response.AccessToken;
        }

        if (!string.IsNullOrEmpty(accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        var responseMessage = await base.SendAsync(request, cancellationToken);

        return responseMessage;
    }
}
