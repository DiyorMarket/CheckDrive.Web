using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Text;

namespace CheckDrive.Web.Stores.Debts
{
    public class DebtDataStore : IDebtDataStore
    {
        private readonly ApiClient _api;

        public DebtDataStore(ApiClient apiClient)
        {
            _api = apiClient;
        }

        public async Task<GetDebtResponse> GetDebtsAsync(string? searchString, int? pageNumber)
        {
            StringBuilder query = new("");

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query.Append($"searchString={searchString}&");
            }
            if (pageNumber != null)
            {
                query.Append($"pageNumber={pageNumber}");
            }

            var response = await _api.GetAsync("debts?" + query.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch debts.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetDebtResponse>(json);

            return result;
        }

        public async Task<GetDebtResponse> GetDebtsAsync()
        {

            var response = await _api.GetAsync("debts?");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch debts.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetDebtResponse>(json);

            return result;
        }

        public async Task<DebtDto> GetDebtAsync(int id)
        {
            var response = await _api.GetAsync($"debts/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch debt with id: {id}.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<DebtDto>(json);

            return result;
        }

        public async Task<DebtDto> CreateDebtAsync(DebtForCreateDto debtForCreate)
        {
            var json = JsonConvert.SerializeObject(debtForCreate);
            var response = await _api.PostAsync("debts", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating debt.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<DebtDto>(jsonResponse);
        }
    }
}
