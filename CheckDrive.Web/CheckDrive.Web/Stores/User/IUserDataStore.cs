using CheckDrive.Web.ViewModels;

namespace CheckDrive.Web.Stores.User
{
    public interface IUserDataStore
    {
        public Task<(bool Success, string Token)> AuthenticateLoginAsync(LoginViewModel loginViewModel);
    }
}
