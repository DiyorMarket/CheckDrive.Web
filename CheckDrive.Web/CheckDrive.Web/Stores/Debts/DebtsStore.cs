using CheckDrive.Web.Requests.Debt;
using CheckDrive.Web.Services;
using CheckDrive.Web.ViewModels.Debt;

namespace CheckDrive.Web.Stores.Debts;

internal sealed class DebtsStore : IDebtsStore
{
    private static readonly string resourceUrl = "debts";
    private readonly CheckDriveApi _apiClient;
    public DebtsStore(CheckDriveApi apiClient)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public Task<List<DebtViewModel>> GetAsync()
        =>_apiClient.GetAsync<List<DebtViewModel>>(resourceUrl);
 
    public Task<DebtViewModel> GetByIdAsync(int id)
        => _apiClient.GetAsync<DebtViewModel>($"{resourceUrl}/{id}");

    public Task UpdateAsync(UpdateDebtRequest request)
        =>_apiClient.PutAsync($"{resourceUrl}/{request.Id}", request);

    public Task DeleteAsync(int id)
        => _apiClient.DeleteAsync($"{resourceUrl}/{id}");
}
