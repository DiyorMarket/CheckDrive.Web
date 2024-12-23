using CheckDrive.Web.ViewModels.CheckPoint;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace CheckDrive.Web.Stores.SplineCharts
{
    public interface ICheckPointStore
    {
        List<CheckPointViewModel> GetAll(string? search);
        CheckPointViewModel GetCheckPointById(int id);
        List<SelectListItem> GetEnumValues<TEnum>() where TEnum : Enum;
    }
}
