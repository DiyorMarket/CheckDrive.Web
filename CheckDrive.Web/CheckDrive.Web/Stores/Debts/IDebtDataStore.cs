using CheckDrive.ApiContracts.Debts;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.Debts
{
    public interface IDebtDataStore
    {
        Task<GetDebtResponse> GetDebtsAsync(int? accountId);
        Task<DebtsDto> GetDebtByIdAsync(int id);
        Task<DebtsDto> CreateDebtAsync(DebtsForCreateDto review);
        Task<DebtsDto> UpdateDebtAsync(int id, DebtsForUpdateDto review);
        Task DeleteDebtAsync(int id);
    }
}
