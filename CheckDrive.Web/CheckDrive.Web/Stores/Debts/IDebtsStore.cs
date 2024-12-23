using CheckDrive.Web.ViewModels.Debt;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckDrive.Web.Stores.Debts;

public interface IDebtsStore
{
    List<DebtViewModel> GetAll(string? searchText, string? status );
    List<SelectListItem> GetEnum();
}
