using CheckDrive.ApiContracts.Doctor;
using CheckDrive.Web.Exceptions;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Web;

namespace CheckDrive.Web.Stores.Doctors;

public class DoctorDataStore : IDoctorDateStore
{
    private readonly ApiClient _api;

    public DoctorDataStore(ApiClient apiClient)
    {
        _api = apiClient;
    }

    public async Task<GetDoctorResponse> GetDoctorsAsync()
    {
        var response = await _api.GetAsync("doctors");

        return await ApiResponseHandler.HandleApiResponse<GetDoctorResponse>(response, "Could not fetch doctors.");
    }

    public async Task<DoctorDto> GetDoctorByIdAsync(int id)
    {
        var response = await _api.GetAsync($"doctors/{id}");

        return await ApiResponseHandler.HandleApiResponse<DoctorDto>(response, $"Could not fetch doctor with id: {id}.");
    }

    public async Task<DoctorDto> CreateDoctorAsync(DoctorForCreateDto doctorForCreate)
    {
        var json = JsonConvert.SerializeObject(doctorForCreate);
        var response = await _api.PostAsync("doctors", json);

        return await ApiResponseHandler.HandleApiResponse<DoctorDto>(response, "Error creating doctor.");
    }
}
