using CheckDrive.ApiContracts.Account;
using CheckDrive.ApiContracts.DispatcherReview;
using CheckDrive.ApiContracts.DoctorReview;
using CheckDrive.ApiContracts.Mechanic;
using CheckDrive.ApiContracts.OperatorReview;
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
        Task<IEnumerable<DoctorReviewDto>> GetDoctorHistories(int Id);
        Task<IEnumerable<OperatorReviewDto>> GetOperatorHistories(int Id);
        Task<IEnumerable<MechanicHistororiesDto>> GetMechanicHistories(int Id);
        Task<IEnumerable<DispatcherReviewDto>> GetDispatcherHistories(int Id);
    }
}
