using CheckDrive.Web.Requests.Debt;
using CheckDrive.Web.ViewModels.Debt;

namespace CheckDrive.Web.Mappings;

public static class DebtMappings
{
    public static UpdateDebtRequest ToUpdateViewModel(this DebtViewModel viewModel) =>
        new()
        {
            Id = viewModel.Id,
            FirstName = viewModel.FirstName,
            LastName = viewModel.LastName,
            CheckPointId = viewModel.CheckPointId,
            FualAmount = viewModel.FualAmount,
            PaidAmount = viewModel.PaidAmount,
            Status = viewModel.Status,
        };
}
