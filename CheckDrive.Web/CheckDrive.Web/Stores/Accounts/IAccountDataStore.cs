using CheckDrive.Web.Models;

namespace CheckDrive.Web.Stores.Accounts
{
    public interface IAccountDataStore
    {
        Task<List<Account>> GetAccounts();
        Task<Account> GetAccount(int id);
        Task<Account> CreateAccount(Account account);
        Task<Account> UpdateAccount(int id, Account account);
        Task DeleteAccount(int id);
    }
}
