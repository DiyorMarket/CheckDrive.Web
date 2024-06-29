using CheckDrive.ApiContracts.Account;
using CheckDrive.Web.Service;
using CheckDrive.Web.ViewModels;
using Newtonsoft.Json;
using System.Text;

namespace CheckDrive.Web.Stores.User;

public class UserDataStore : IUserDataStore
{
    private readonly ApiClient _apiClient;
    public UserDataStore(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }
    public async Task<(bool Success, string Token)> AuthenticateLoginAsync(AccountForLoginDto loginViewModel)
    {
        var json = JsonConvert.SerializeObject(loginViewModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _apiClient.PostAsync("login/login", json);

        if (!response.IsSuccessStatusCode)
        {
            return (false, string.Empty);
        }

        var tokenJson = await response.Content.ReadAsStringAsync();
        var token = JsonConvert.DeserializeObject<string>(tokenJson);

        return (true, token);
    }
}

