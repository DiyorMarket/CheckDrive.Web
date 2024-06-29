using CheckDrive.ApiContracts.Account;
using CheckDrive.ApiContracts.Doctor;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Text;

namespace CheckDrive.Web.Stores.Doctors
{
    public class DoctorDataStore : IDoctorDataStore
    {
        private readonly ApiClient _api;

        public DoctorDataStore(ApiClient apiClient)
        {
            _api = apiClient;
        }

        public async Task<GetDoctorResponse> GetDoctors(int accountId)
        {
            StringBuilder query = new("");

            if (!accountId.Equals(0))
            {
                query.Append($"accountId={accountId}");
            }

            var response = await _api.GetAsync("doctors?" + query.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch doctors.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetDoctorResponse>(json);

            return result;
        }

        public async Task<GetDoctorResponse> GetDoctors()
        {
            var response = await _api.GetAsync("doctors?");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch doctors.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetDoctorResponse>(json);

            return result;
        }

        public async Task<DoctorDto> GetDoctor(int id)
        {
            var response = await _api.GetAsync($"doctors/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch doctor with id: {id}.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<DoctorDto>(json);

            return result;
        }

        public async Task<DoctorDto> CreateDoctor(DoctorForCreateDto doctorForCreate)
        {
            var json = JsonConvert.SerializeObject(doctorForCreate);
            var response = await _api.PostAsync("doctors", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating doctors.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<DoctorDto>(jsonResponse);
        }

        public async Task<DoctorDto> UpdateDoctor(int id, AccountForUpdateDto doctorForUpdate)
        {
            var json = JsonConvert.SerializeObject(doctorForUpdate);
            var response = await _api.PutAsync($"doctors/{doctorForUpdate.Id}", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error updating doctors.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<DoctorDto>(jsonResponse);
        }

        public async Task DeleteDoctor(int id)
        {
            var response = await _api.DeleteAsync($"doctors/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not delete doctor with id: {id}.");
            }
        }
    }
}
