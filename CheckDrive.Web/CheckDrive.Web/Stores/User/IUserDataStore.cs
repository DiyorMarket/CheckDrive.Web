using CheckDrive.ApiContracts.Account;
using CheckDrive.Web.ViewModels;

namespace CheckDrive.Web.Stores.User
{
    public interface IUserDataStore
    {
        public Task<(bool Success, string Token)> AuthenticateLoginAsync(AccountForLoginDto loginViewModel);
    }
}
