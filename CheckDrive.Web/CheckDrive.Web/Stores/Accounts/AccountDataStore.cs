using CheckDrive.ApiContracts.Account;
using CheckDrive.ApiContracts.DispatcherReview;
using CheckDrive.ApiContracts.DoctorReview;
using CheckDrive.ApiContracts.Mechanic;
using CheckDrive.ApiContracts.OperatorReview;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Services;

namespace CheckDrive.Web.Stores.Accounts;

public class AccountDataStore : IAccountDataStore
{
    private readonly CheckDriveApi _api;

    public AccountDataStore(CheckDriveApi apiClient)
    {
        _api = apiClient;
    }

    public Task<AccountDto> CreateAccountAsync(AccountForCreateDto account)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAccountAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<AccountDto> GetAccountAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<GetAccountResponse> GetAccountsAsync(string? searchString, int? roleId, DateTime? birthDate, int? pageNumber)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DispatcherReviewDto>> GetDispatcherHistories(int Id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DoctorReviewDto>> GetDoctorHistories(int Id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<MechanicHistororiesDto>> GetMechanicHistories(int Id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<OperatorReviewDto>> GetOperatorHistories(int Id)
    {
        throw new NotImplementedException();
    }

    public Task<AccountDto> UpdateAccountAsync(int id, AccountForUpdateDto account)
    {
        throw new NotImplementedException();
    }
}
