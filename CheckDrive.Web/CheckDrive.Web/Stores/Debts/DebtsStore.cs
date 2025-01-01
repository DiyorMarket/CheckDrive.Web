using CheckDrive.Web.ViewModels.Debt;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckDrive.Web.Stores.Debts;

public class DebtsStore : IDebtsStore
{
    public List<DebtViewModel> GetAll(string? searchText, string? status)
    {
        throw new NotImplementedException();
    }

    public List<SelectListItem> GetEnum()
    {
        throw new NotImplementedException();
    }
}
