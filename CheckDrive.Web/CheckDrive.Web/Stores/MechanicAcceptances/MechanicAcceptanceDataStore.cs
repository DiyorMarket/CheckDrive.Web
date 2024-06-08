using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Text;

namespace CheckDrive.Web.Stores.MechanicAcceptances
{
    public class MechanicAcceptanceDataStore : IMechanicAcceptanceDataStore
    {
        private readonly ApiClient _api;

        public MechanicAcceptanceDataStore(ApiClient apiClient)
        {
            _api = apiClient;
        }
        public async Task<GetMechanicAcceptanceResponse> GetMechanicAcceptancesAsync(int? pageNumber)
        {
            StringBuilder query = new("");

            if (pageNumber != null)
            {
                query.Append($"pageNumber={pageNumber}");
            }
            var response = await _api.GetAsync("mechanics/acceptances?" + query.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch drivers.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetMechanicAcceptanceResponse>(json);

            return result;
        }
        public async Task<GetMechanicAcceptanceResponse> GetMechanicAcceptancesAsync()
        {
            var response = await _api.GetAsync("mechanics/acceptances?");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch drivers.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetMechanicAcceptanceResponse>(json);

            return result;
        }
        public Task DeleteMechanicAcceptanceAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MechanicAcceptance> GetMechanicAcceptanceAsync(int id)
        {
            throw new NotImplementedException();
        }
        
        public async  Task<MechanicAcceptanceDto> CreateMechanicAcceptanceAsync(MechanicAcceptanceForCreateDto acceptanceForCreateDto)
        {
            var json = JsonConvert.SerializeObject(acceptanceForCreateDto);
            var response = await _api.PostAsync("mechanics/acceptance", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating mechanicAcceptance.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<MechanicAcceptanceDto>(jsonResponse);
        }
        Task<MechanicAcceptanceDto> IMechanicAcceptanceDataStore.GetMechanicAcceptanceAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<MechanicAcceptanceDto> IMechanicAcceptanceDataStore.UpdateMechanicAcceptanceAsync(int id, MechanicAcceptanceForUpdateDto mechanicAcceptanceForUpdateDto)
        {
            throw new NotImplementedException();
        }
    }
}