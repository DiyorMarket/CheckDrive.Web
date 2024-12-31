using CheckDrive.Web.Requests.Auth;
using CheckDrive.Web.Responses.Auth;
using CheckDrive.Web.Services;

namespace CheckDrive.Web.Stores.Auth;

public sealed class AuthStore : IAuthStore
{
    private readonly CheckDriveApi _clientApi;

    public AuthStore(CheckDriveApi clientApi)
    {
        _clientApi = clientApi ?? throw new ArgumentNullException(nameof(clientApi));
    }

    public async Task<TokenResponse> LoginAsync(LoginRequest request)
    {
        var response = await _clientApi.PostAsync<LoginRequest, TokenResponse>("auth/login", request);

        return response;
    }

    public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var response = await _clientApi.PostAsync<RefreshTokenRequest, TokenResponse>("auth/refresh-token", request);

        return response;
    }
}
