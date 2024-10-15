using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.CheckPoint
{
    public interface ICheckPointDataStore
    {
        Task<GetCheckPointResponse> GetCheckPointsAsync(string? searchString, int? pageNumber);
        Task<GetCheckPointResponse> GetCheckPointsAsync();
        Task<CheckPointDto> GetCheckPointAsync(int id);
        Task<CheckPointDto> CreateCheckPointAsync(CheckPointForCreateDto checkpointForCreate);
    }
}
