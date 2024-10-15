using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Text;

namespace CheckDrive.Web.Stores.Employees
{
    public class EmployeeDataStore : IEmployeeDataStore
    {
        private readonly ApiClient _api;

        public EmployeeDataStore(ApiClient apiClient)
        {
            _api = apiClient;
        }

        public async Task<GetEmployeeResponse> GetEmployeesAsync(string? searchString, int? pageNumber)
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

            var response = await _api.GetAsync("employees?" + query.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch employees.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetEmployeeResponse>(json);

            return result;
        }

        public async Task<GetEmployeeResponse> GetEmployeesAsync()
        {

            var response = await _api.GetAsync("employees?");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch employees.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetEmployeeResponse>(json);

            return result;
        }

        public async Task<EmployeeDto> GetEmployeeAsync(int id)
        {
            var response = await _api.GetAsync($"employees/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch employee with id: {id}.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<EmployeeDto>(json);

            return result;
        }

        public async Task<EmployeeDto> CreateEmployeeAsync(EmployeeForCreateDto employeeForCreate)
        {
            var json = JsonConvert.SerializeObject(employeeForCreate);
            var response = await _api.PostAsync("employees", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating employees.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<EmployeeDto>(jsonResponse);
        }
    }
}
