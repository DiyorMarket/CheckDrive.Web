using CheckDrive.ApiContracts.MechanicAcceptance;
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
        public async Task<GetMechanicAcceptanceResponse> GetMechanicAcceptancesAsync(
            int? pageNumber, 
            string? searchString,
            DateTime? date,
            string? status,
            int? roleId)
        {
            StringBuilder query = new StringBuilder();

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
            
            var response = await _api.GetAsync("mechanics/acceptances?OrderBy=datedesc&" + query.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch mechanic acceptances.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetMechanicAcceptanceResponse>(json);

            return result;
        }

        public async Task<GetMechanicAcceptanceResponse> GetMechanicAcceptancesAsync(
            int? pageNumber,
            string? searchString,
            DateTime? date,
            int? accountId)
        {
            StringBuilder query = new StringBuilder();

            if (accountId != 0)
                query.Append($"accountId={accountId}&");

            if (date is not null)
                query.Append($"date={date.Value.ToString("MM/dd/yyyy")}&");

            if (!string.IsNullOrWhiteSpace(searchString))
                query.Append($"searchString={searchString}&");

            if (pageNumber != null)
                query.Append($"pageNumber={pageNumber}");

            var response = await _api.GetAsync("mechanics/acceptances?OrderBy=datedesc&" + query.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch mechanic acceptances.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetMechanicAcceptanceResponse>(json);

            return result;
        }

        public async Task<GetMechanicAcceptanceResponse> GetMechanicAcceptancesAsync()
        {
            var response = await _api.GetAsync("mechanics/acceptances?OrderBy=datedesc");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch drivers.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetMechanicAcceptanceResponse>(json);

            return result;
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

        public async Task<MechanicAcceptanceDto> GetMechanicAcceptanceAsync(int id)
        {
            var response = await _api.GetAsync($"mechanics/acceptance/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch account with id: {id}.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<MechanicAcceptanceDto>(json);

            if (result == null)
            {
                throw new Exception($"Deserialization of MechanicAcceptanceDto failed for id: {id}.");
            }

            return result;
        }

        public async Task<MechanicAcceptanceDto> UpdateMechanicAcceptanceAsync(int id, MechanicAcceptanceForUpdateDto mechanicAcceptanceForUpdateDto)
        {
            var json = JsonConvert.SerializeObject(mechanicAcceptanceForUpdateDto);
            var response = await _api.PutAsync($"mechanics/acceptance/{mechanicAcceptanceForUpdateDto.Id}", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error updating MechanicHandovers.");   
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<MechanicAcceptanceDto>(jsonResponse);
        }

        public async Task DeleteMechanicAcceptanceAsync(int id)
        {
            var response = await _api.DeleteAsync($"mechanics/acceptance/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not delete Mechanic acceptance with id: {id}.");
            }
        }
        public async Task<Stream> GetExportFile(int year, int month)
        {
            try
            {
                string url = $"mechanics/acceptance/export?year={year}&month={month}";
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