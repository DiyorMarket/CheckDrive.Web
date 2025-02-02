using CheckDrive.Web.Requests.Debt;
using CheckDrive.Web.ViewModels.Debt;

namespace CheckDrive.Web.Stores.Debts;

public interface IDebtsStore
{
    Task<List<DebtViewModel>> GetAsync();
    Task<DebtViewModel> GetByIdAsync(int id);
    Task UpdateAsync(UpdateDebtRequest viewModel);
    Task DeleteAsync(int id);
}
