using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;

namespace CheckDrive.Web.Stores.Accounts
{
    public class AccountDataStore : IAccountDataStore
    {
        private readonly ApiClient _api;

        public AccountDataStore(ApiClient apiClient)
        {
            _api = apiClient;
        }

        public async Task<GetAccountResponse> GetAccountsAsync()
        {
            var response = await _api.GetAsync("accounts");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch accounts.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetAccountResponse>(json);

            return result;
        }

        public async Task<Account> GetAccountAsync(int id)
        {
            var response = await _api.GetAsync($"accounts/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch account with id: {id}.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Account>(json);

            return result;
        }

        public async Task<Account> CreateAccountAsync(Account account)
        {
            var json = JsonConvert.SerializeObject(account);
            var response = await _api.PostAsync("accounts", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating account.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Account>(jsonResponse);
        }

        public async Task<Account> UpdateAccountAsync(int id, Account account)
        {
            var json = JsonConvert.SerializeObject(account);
            var response = await _api.PutAsync($"accounts/{account.Id}", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error updating account.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Account>(jsonResponse);
        }

        public async Task DeleteAccountAsync(int id)
        {
            var response = await _api.DeleteAsync($"accounts/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not delete account with id: {id}.");
            }
        }
    }
}
