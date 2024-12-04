using CheckDrive.Web.ViewModels.CheckPoint;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckDrive.Web.Stores.SplineCharts
{
    public interface ICheckPointStore
    {
        List<CheckPointViewModel> GetAll(string? search);
        List<SelectListItem> GetStatus();
    }
}
