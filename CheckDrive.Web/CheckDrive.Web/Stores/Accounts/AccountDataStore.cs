using CheckDrive.ApiContracts.Account;
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


        public async Task<GetAccountResponse> GetAccountsAsync(string? searchString, int? roleId, DateTime? birthDate, int? pageNumber)
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
            if (roleId is not null)
            {
                query.Append($"roleId={roleId}&");
            }
            if (pageNumber != null)
            {
                query.Append($"pageNumber={pageNumber}");
            }

            var response = await _api.GetAsync("accounts?" + query.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch accounts.");
            }


            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetAccountResponse>(json);

            return result;
        }

        public async Task<AccountDto> GetAccountAsync(int id)

        {
            var response = await _api.GetAsync($"accounts/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch account with id: {id}.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AccountDto>(json);

            return result;
        }

        public async Task<AccountDto> CreateAccountAsync(AccountForCreateDto account)
        {
            var json = JsonConvert.SerializeObject(account);
            var response = await _api.PostAsync("accounts", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating account.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AccountDto>(jsonResponse);
        }



        public async Task<AccountDto> UpdateAccountAsync(int id, AccountForUpdateDto account)
        {
            var json = JsonConvert.SerializeObject(account);
            var response = await _api.PutAsync($"accounts/{account.Id}", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error updating account.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<AccountDto>(jsonResponse);
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
