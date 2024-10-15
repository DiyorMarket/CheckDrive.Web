using CheckDrive.ApiContracts.OilMark;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.OilMarks
{
    public interface IOilMarkDataStore
    {
        Task<GetOilMarkResponse> GetOilMarksAsync(string? searchString, int? pageNumber);
        Task<GetOilMarkResponse> GetOilMarksAsync();
        Task<OilMarkDto> GetOilMarkAsync(int id);
        Task<OilMarkDto> CreateOilMarkAsync(OilMarkForCreateDto oilmarkForCreate);
    }
}
