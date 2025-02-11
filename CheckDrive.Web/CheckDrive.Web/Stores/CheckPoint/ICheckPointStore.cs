using CheckDrive.Web.ViewModels.CheckPoint;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckDrive.Web.Stores.CheckPoint;

public interface ICheckPointStore
{
    Task<List<CheckPointViewModel>> GetAllAsync();
    Task<CheckPointViewModel> GetByIdAsync(int id);
    List<SelectListItem> GetEnumValues<TEnum>() where TEnum : Enum;
}
