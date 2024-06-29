using CheckDrive.ApiContracts.Account;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.Accounts
{
    public interface IAccountDataStore
    {
        Task<GetAccountResponse> GetAccountsAsync(string? searchString, int? roleId, DateTime? birthDate, int? pageNumber);
        Task<AccountDto> GetAccountAsync(int id);
        Task<AccountDto> CreateAccountAsync(AccountForCreateDto account);
        Task<AccountDto> UpdateAccountAsync(int id, AccountForUpdateDto account);
        Task DeleteAccountAsync(int id);
    }
}
