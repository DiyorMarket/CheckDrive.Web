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
            string? status,
            int? roleId)
        {
            StringBuilder query = new("");

            if (!string.IsNullOrWhiteSpace(status))
                query.Append($"status={status}&");

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
        public async Task<GetMechanicHandoverResponse> GetMechanicHandoversAsync(
            int? pageNumber,
            string? searchString,
            DateTime? date,
            int? accountId)
        {
            StringBuilder query = new("");

            if (accountId != 0)
                query.Append($"accountId={accountId}&");

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

        public async Task<MechanicHandoverDto> GetMechanicHandoverAsync(int id)
        {
            var response = await _api.GetAsync($"mechanics/handover/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch account with id: {id}.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<MechanicHandoverDto>(json);

            return result;
        }
        public async Task<MechanicHandoverDto> UpdateMechanicHandoverAsync(int id, MechanicHandoverForUpdateDto mechanicHandover)
        {
            var json = JsonConvert.SerializeObject(mechanicHandover);
            var response = await _api.PutAsync($"mechanics/handover/{mechanicHandover.Id}", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error updating MechanicHandovers.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<MechanicHandoverDto>(jsonResponse);
        }
        public async Task DeleteMechanicHandoverAsync(int id)
        {
            var response = await _api.DeleteAsync($"mechanics/handover/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not delete Mechanic acceptance with id: {id}.");
            }
        }

        public async Task<Stream> GetExportFile(int year, int month)
        {
            try
            {
                string url = $"mechanics/handover/export?year={year}&month={month}";
                var response = await _api.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to retrieve the file.");
                }

                var stream = await response.Content.ReadAsStreamAsync();
                if (stream == null || stream.Length == 0)
                {
                    Console.WriteLine("The file is empty or could not be retrieved.");
                    return null; // or throw an exception based on your needs
                }

                return stream;
            }
            catch (InvalidOperationException ex)
            {
                // Log or handle the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }
    }
}