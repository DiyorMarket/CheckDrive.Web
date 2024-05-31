using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Text;

namespace CheckDrive.Web.Stores.MechanicHandovers
{
    public class MechanicHandoverDataStore : IMechanicHandoverDataStore
    {
        private readonly ApiClient _api;

        public MechanicHandoverDataStore(ApiClient apiClient)
        {
            _api = apiClient;
        }
        public async Task<GetMechanicHandoverResponse> GetMechanicHandoversAsync(int? pageNumber)
        {
            StringBuilder query = new("");

            if (pageNumber != null)
            {
                query.Append($"pageNumber={pageNumber}");
            }
            var response = await _api.GetAsync("mechanics/handovers?" + query.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch handovers.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetMechanicHandoverResponse>(json);

            return result;
        }
        public Task<MechanicHandover> CreateMechanicHandoverAsync(MechanicHandover mechanicHandover)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMechanicHandoverAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MechanicHandover> GetMechanicHandoverAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<MechanicHandover> UpdateMechanicHandoverAsync(int id, MechanicHandover mechanicHandover)
        {
            throw new NotImplementedException();
        }
    }
}