using CheckDrive.Web.Requests.OilMark;
using CheckDrive.Web.ViewModels.OilMark;

namespace CheckDrive.Web.Stores.OilMarks;

public interface IOilMarkStore
{
    Task<List<OilMarkViewModel>> GetAsync();
    Task<OilMarkViewModel> GetByIdAsync(int id);
    Task<OilMarkViewModel> CreateAsync(CreateOilMarkRequest request);
    Task UpdateAsync(UpdateOilMarkRequest request);
    Task DeleteAsync(int id);
}
