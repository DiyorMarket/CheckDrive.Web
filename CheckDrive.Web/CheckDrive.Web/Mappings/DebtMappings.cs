using CheckDrive.Web.Requests.Debt;
using CheckDrive.Web.ViewModels.Debt;

namespace CheckDrive.Web.Mappings;

public static class DebtMappings
{
    public static UpdateDebtRequest ToUpdateViewModel(this DebtViewModel viewModel) =>
        new()
        {
            Id = viewModel.Id,
            DriverFullName = viewModel.DriverFullName,
            CheckPointId = viewModel.CheckPointId,
            FuelAmount = viewModel.FuelAmount,
            PaidAmount = viewModel.PaidAmount,
            Status = viewModel.Status,
        };
}
