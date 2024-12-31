using CheckDrive.Web.Requests.Auth;
using CheckDrive.Web.Responses.Auth;

namespace CheckDrive.Web.Stores.Auth;

public interface IAuthStore
{
    Task<TokenResponse> LoginAsync(LoginRequest request);
    Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request);
}
