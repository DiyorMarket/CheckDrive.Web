using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;

namespace CheckDrive.Web.Stores.MechanicAcceptances
{
    public class MechanicAcceptanceDataStore : IMechanicAcceptanceDataStore
    {
        private readonly ApiClient _api;

        public MechanicAcceptanceDataStore(ApiClient apiClient)
        {
            _api = apiClient;
        }
        public Task<GetMechanicResponse> GetMechanicAcceptancesAsync()
        {
            throw new NotImplementedException();
        }
        public Task<MechanicAcceptance> CreateMechanicAcceptanceAsync(MechanicAcceptance mechanicAcceptance)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMechanicAcceptanceAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MechanicAcceptance> GetMechanicAcceptanceAsync(int id)
        {
            throw new NotImplementedException();
        }


        public Task<MechanicAcceptance> UpdateMechanicAcceptanceAsync(int id, MechanicAcceptance mechanicAcceptance)
        {
            throw new NotImplementedException();
        }
    }
}