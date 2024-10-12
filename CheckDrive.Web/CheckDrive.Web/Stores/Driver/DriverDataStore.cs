using CheckDrive.ApiContracts.Driver;
using CheckDrive.Web.Exceptions;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Web;

namespace CheckDrive.Web.Stores.Driver;

public class DriverDataStore : IDriverDataStore
{
    private readonly ApiClient _api;

    public DriverDataStore(ApiClient apiClient)
    {
        _api = apiClient;
    }

    public async Task<GetDriverResponse> GetDriversAsync(string? searchString, int? pageNumber)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            query["searchString"] = searchString;
        }
        if (pageNumber != null)
        {
            query["pageNumber"] = pageNumber.ToString();
        }

        var response = await _api.GetAsync($"drivers?{query}");

        return await ApiResponseHandler.HandleApiResponse<GetDriverResponse>(response, "Could not fetch drivers.");
    }

    public async Task<GetDriverResponse> GetDriversAsync()
    {
        var response = await _api.GetAsync("drivers");

        return await ApiResponseHandler.HandleApiResponse<GetDriverResponse>(response, "Could not fetch drivers.");
    }

    public async Task<DriverDto> GetDriverByIdAsync(int id)
    {
        var response = await _api.GetAsync($"drivers/{id}");

        return await ApiResponseHandler.HandleApiResponse<DriverDto>(response, $"Could not fetch driver with id: {id}.");
    }

    public async Task<DriverDto> CreateDriverAsync(DriverForCreateDto driverForCreate)
    {
        var json = JsonConvert.SerializeObject(driverForCreate);
        var response = await _api.PostAsync("drivers", json);

        return await ApiResponseHandler.HandleApiResponse<DriverDto>(response, "Error creating driver.");
    }
}
