using CheckDrive.ApiContracts.Account;
using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.Accounts
{
    public interface IAccountDataStore
    {
        Task<GetAccountResponse> GetAccounts(string searchString,int? roleId,DateTime? birthDate);
        Task<AccountDto> GetAccount(int id);
        Task<AccountDto> CreateAccount(AccountForCreateDto account);
        Task<AccountDto> UpdateAccount(int id, AccountForUpdateDto account);
        Task DeleteAccount(int id);
    }
}
