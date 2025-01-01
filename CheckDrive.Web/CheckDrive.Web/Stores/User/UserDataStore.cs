using CheckDrive.ApiContracts.Account;
using CheckDrive.Web.Services;

namespace CheckDrive.Web.Stores.User;

public class UserDataStore : IUserDataStore
{
    private readonly CheckDriveApi _apiClient;

    public UserDataStore(CheckDriveApi apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<(bool Success, string Token)> AuthenticateLoginAsync(AccountForLoginDto loginViewModel)
    {
        var response = await _apiClient.PostAsync<AccountForLoginDto, string>("login/login", loginViewModel);

        return (true, response);
    }
}

