using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.Accounts
{
    public interface IAccountDataStore
    {
        Task<GetAccountResponse> GetAccountsAsync();
        Task<Account> GetAccountAsync(int id);
        Task<Account> CreateAccountAsync(Account account);
        Task<Account> UpdateAccountAsync(int id, Account account);
        Task DeleteAccountAsync(int id);
    }
}
