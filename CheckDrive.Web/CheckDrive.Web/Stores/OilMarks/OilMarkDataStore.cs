using CheckDrive.ApiContracts.OilMark;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Services;

namespace CheckDrive.Web.Stores.OilMarks;

public class OilMarkDataStore : IOilMarkDataStore
{
    private readonly CheckDriveApi _apiClient;

    public OilMarkDataStore(CheckDriveApi apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<OilMarkDto> CreateOilMarkAsync(OilMarkForCreateDto review)
    {
        throw new NotImplementedException();
    }

    public Task DeleteOilMarkAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<OilMarkDto> GetOilMarkByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<GetOilMarkResponse> GetOilMarksAsync()
    {
        throw new NotImplementedException();
    }

    public Task<OilMarkDto> UpdateOilMarkAsync(int id, OilMarkForUpdateDto review)
    {
        throw new NotImplementedException();
    }
}
