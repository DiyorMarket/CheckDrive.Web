using CheckDrive.ApiContracts.Mechanic;
using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.ApiContracts.MechanicHandover;
using CheckDrive.Web.Service;

namespace CheckDrive.Web.Stores.Mechanics
{
    public class MechanicDataStore : IMechanicDataStore
    {
        private readonly ApiClient _apiClient;

        public MechanicDataStore(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IEnumerable<MechanicDto>> GetMechanicsAsync()
        {
            var response =_apiClient.Get("mechanics");
            return await response.Content.ReadAsAsync<IEnumerable<MechanicDto>>();
        }

        public async Task<IEnumerable<MechanicAcceptanceDto>> GetMechanicAcceptancesAsync()
        {
            var response = await _apiClient.GetAsync("mechanicacceptances");
            return await response.Content.ReadAsAsync<IEnumerable<MechanicAcceptanceDto>>();
        }

        public async Task<IEnumerable<MechanicHandoverDto>> GetMechanicHandoversAsync()
        {
            var response = await _apiClient.GetAsync("mechanichandovers");
            return await response.Content.ReadAsAsync<IEnumerable<MechanicHandoverDto>>();
        }
    }
}
