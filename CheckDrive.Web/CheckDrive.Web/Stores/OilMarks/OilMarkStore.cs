using CheckDrive.Web.Requests.OilMark;
using CheckDrive.Web.Services;
using CheckDrive.Web.ViewModels.OilMark;

namespace CheckDrive.Web.Stores.OilMarks;

public class OilMarkStore(CheckDriveApi apiClient) : IOilMarkStore
{
    private static readonly string resourceUrl = "oilMarks";
    
    public Task<List<OilMarkViewModel>> GetAsync()
        => apiClient.GetAsync<List<OilMarkViewModel>>(resourceUrl);

    public Task<OilMarkViewModel> GetByIdAsync(int id)
        => apiClient.GetAsync<OilMarkViewModel>($"{resourceUrl}/{id}");

    public Task<OilMarkViewModel> CreateAsync(CreateOilMarkRequest request)
        => apiClient.PostAsync<CreateOilMarkRequest, OilMarkViewModel>(resourceUrl, request);

    public Task UpdateAsync(UpdateOilMarkRequest request)
        => apiClient.PutAsync($"{resourceUrl}/{request.Id}", request);

    public Task DeleteAsync(int id)
        => apiClient.DeleteAsync($"{resourceUrl}/{id}");
}
