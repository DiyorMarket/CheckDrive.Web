using CheckDrive.ApiContracts.MechanicHandover;
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
        public async Task<GetMechanicHandoverResponse> GetMechanicHandoversAsync(
            int? pageNumber,
            string? searchString,
            DateTime? date,
            int? roleId)
        {
            StringBuilder query = new("");

            if (roleId != 0)
                query.Append($"roleId={roleId}&");

            if (date is not null)
                query.Append($"date={date.Value.ToString("MM/dd/yyyy")}&");
            
            if (!string.IsNullOrWhiteSpace(searchString))
                query.Append($"searchString={searchString}&");
            
            if (pageNumber != null)
                query.Append($"pageNumber={pageNumber}");
            
            var response = await _api.GetAsync("mechanics/handovers?OrderBy=datedesc&" + query.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch handovers.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetMechanicHandoverResponse>(json);

            return result;
        }
        public async Task<GetMechanicHandoverResponse> GetMechanicHandoversAsync()
        {
            var response = await _api.GetAsync("mechanics/handovers?OrderBy=datedesc");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch handovers.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetMechanicHandoverResponse>(json);

            return result;
        }
        public async Task<MechanicHandoverDto> CreateMechanicHandoverAsync(MechanicHandoverForCreateDto mechanicHandoverForCreateDto)
        {
            var json = JsonConvert.SerializeObject(mechanicHandoverForCreateDto);
            var response = await _api.PostAsync("mechanics/handover", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating mechanicAcceptance.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<MechanicHandoverDto>(jsonResponse);
        }

        public Task DeleteMechanicHandoverAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MechanicHandoverDto> GetMechanicHandoverAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<MechanicHandoverDto> UpdateMechanicHandoverAsync(int id, MechanicHandoverForUpdateDto mechanicHandover)
        {
            throw new NotImplementedException();
        }
    }
}