using CheckDrive.ApiContracts.Debts;
using CheckDrive.ApiContracts.OilMark;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CheckDrive.Web.Stores.Debts
{
    public class DebtDataStore(ApiClient api) : IDebtDataStore
    {
        private readonly ApiClient _api = api;

        public async Task<GetDebtResponse> GetDebtsAsync(string? searchString, int? pageNumber, DateTime? date) 
        {
            StringBuilder query = new("");

            if (date is not null)
                query.Append($"date={date.Value.ToString("MM/dd/yyyy")}&");

            if (pageNumber != null)
            {
                query.Append($"pageNumber={pageNumber}");
            }

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query.Append($"searchString={searchString}&");
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
        public async Task<DebtsDto> GetDebtByIdAsync(int id)
        {
            var response = await _api.GetAsync($"debts/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch debt with id: {id}.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<DebtsDto>(json);

            return result;
        }
        public async Task<DebtsDto> CreateDebtAsync(DebtsForCreateDto review)
        {
            var json = JsonConvert.SerializeObject(review);
            var response = await _api.PostAsync("debts", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating debts.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<DebtsDto>(jsonResponse);
        }

        public async Task<DebtsDto> UpdateDebtAsync(int id, DebtsForUpdateDto review)
        {
            var json = JsonConvert.SerializeObject(review);
            var response = await _api.PutAsync($"debts/{review.Id}", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error updating debt.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<DebtsDto>(jsonResponse);
        }
        public async Task DeleteDebtAsync(int id)
        {

            var response = await _api.DeleteAsync($"debts/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not delete debt with id: {id}.");
            }
        }
    }
}
