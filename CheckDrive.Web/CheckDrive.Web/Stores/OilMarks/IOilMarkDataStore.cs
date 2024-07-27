using CheckDrive.ApiContracts.OilMark;
using CheckDrive.Web.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Stores.OilMarks
{
    public interface IOilMarkDataStore
    {
        Task<GetOilMarkResponse> GetOilMarksAsync();
        Task<OilMarkDto> GetOilMarkByIdAsync(int id);
        Task<OilMarkDto> CreateOilMarkAsync(OilMarkForCreateDto review);
        Task<OilMarkDto> UpdateOilMarkAsync(int id, OilMarkForUpdateDto review);
        Task DeleteOilMarkAsync(int id);
    }
}
