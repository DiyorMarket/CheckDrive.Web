using CheckDrive.ApiContracts.Account;
using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Text;

namespace CheckDrive.Web.Stores.Accounts
{
    public class AccountDataStore : IAccountDataStore
    {
        private readonly ApiClient _api;

        public AccountDataStore(ApiClient apiClient)
        {
            _api = apiClient;
        }

        public async Task<GetAccountResponse> GetAccounts()
        {

            var response = _api.Get("accounts");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch accounts.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetAccountResponse>(json);

            return result;
        }

        public async Task<AccountDto> GetAccount(int id)
        {
            var response = _api.Get($"accounts/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch accounts with id: {id}.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<AccountDto>(json);

            return result;
        }

        public async Task<AccountDto> CreateAccount(AccountForCreateDto account)
        {
            var json = JsonConvert.SerializeObject(account);
            var response = _api.Post("accounts", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating accounts.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<AccountDto>(jsonResponse);
        }

        public async Task<AccountDto> UpdateAccount(int id, AccountForUpdateDto account)
        {
            var json = JsonConvert.SerializeObject(account);
            var response = _api.Put($"accounts/{account.Id}", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error updating accounts.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<AccountDto>(jsonResponse);
        }

        public async Task DeleteAccount(int id)
        {
            var response = _api.Delete($"accounts/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not delete accounts with id: {id}.");
            }
        }
    }
}
