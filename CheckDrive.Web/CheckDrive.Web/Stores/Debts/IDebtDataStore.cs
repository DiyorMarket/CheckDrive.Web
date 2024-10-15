using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.Debts
{
    public interface IDebtDataStore
    {
        Task<GetDebtResponse> GetDebtsAsync(string? searchString, int? pageNumber);
        Task<GetDebtResponse> GetDebtsAsync();
        Task<DebtDto> GetDebtAsync(int id);
        Task<DebtDto> CreateDebtAsync(DebtForCreateDto debtForCreate);
    }
}
