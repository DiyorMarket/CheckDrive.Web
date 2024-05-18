using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.Accounts
{
    public interface IAccountDataStore
    {
        Task<GetAccountResponse> GetAccounts(int roleId);
        Task<Account> GetAccount(int id);
        Task<Account> CreateAccount(Account account);
        Task<Account> UpdateAccount(int id, Account account);
        Task DeleteAccount(int id);
    }
}
