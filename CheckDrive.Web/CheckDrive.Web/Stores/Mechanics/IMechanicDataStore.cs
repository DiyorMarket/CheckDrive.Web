using CheckDrive.ApiContracts.Mechanic;
using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.ApiContracts.MechanicHandover;

namespace CheckDrive.Web.Stores.Mechanics
{
    public interface IMechanicDataStore
    {
        Task<IEnumerable<MechanicDto>> GetMechanicsAsync();
        Task<IEnumerable<MechanicAcceptanceDto>> GetMechanicAcceptancesAsync();
        Task<IEnumerable<MechanicHandoverDto>> GetMechanicHandoversAsync();
    }
}
