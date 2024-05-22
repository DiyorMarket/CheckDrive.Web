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

        public async Task<GetAccountResponse> GetAccounts(string? searchString, int? roleId ,DateTime? birthDate)
        {
            StringBuilder query = new("");

            if (birthDate is not null)
            {
                query.Append($"birthDate={birthDate.Value.ToString("MM/dd/yyyy")}&");
            }
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query.Append($"searchString={searchString}&");
            }
            if(roleId != 0)
            {
                query.Append($"roleId={roleId}&");
            }

           var response = _api.Get("accounts?" + query.ToString());

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
                throw new Exception($"Could not delete account with id: {id}.");
            }
        }
    }
}
