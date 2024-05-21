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
        public Task<GetMechanicResponse> GetMechanicAcceptances()
        {
            throw new NotImplementedException();
        }
        public Task<MechanicAcceptance> CreateMechanicAcceptance(MechanicAcceptance mechanicAcceptance)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMechanicAcceptance(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MechanicAcceptance> GetMechanicAcceptance(int id)
        {
            throw new NotImplementedException();
        }


        public Task<MechanicAcceptance> UpdateMechanicAcceptance(int id, MechanicAcceptance mechanicAcceptance)
        {
            throw new NotImplementedException();
        }
    }
}