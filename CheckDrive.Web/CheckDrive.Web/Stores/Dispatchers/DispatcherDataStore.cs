using CheckDrive.ApiContracts.Dispatcher;
using CheckDrive.Web.Exceptions;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Web;

namespace CheckDrive.Web.Stores.Dispatchers;

public class DispatcherDataStore : IDispatcherDataStore
{
    private readonly ApiClient _api;

    public DispatcherDataStore(ApiClient apiClient)
    {
        _api = apiClient;
    }

    public async Task<GetDispatcherResponse> GetDispatchersAsync(string? searchString, int? pageNumber)
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

        var response = await _api.GetAsync($"dispatchers?{query}");

        return await ApiResponseHandler.HandleApiResponse<GetDispatcherResponse>(response, "Could not fetch dispatchers.");
    }

    public async Task<GetDispatcherResponse> GetDispatchersAsync()
    {
        var response = await _api.GetAsync("dispatchers");

        return await ApiResponseHandler.HandleApiResponse<GetDispatcherResponse>(response, "Could not fetch dispatchers.");
    }

    public async Task<DispatcherDto> GetDispatcherByIdAsync(int id)
    {
        var response = await _api.GetAsync($"dispatchers/{id}");

        return await ApiResponseHandler.HandleApiResponse<DispatcherDto>(response, $"Could not fetch dispatcher with id: {id}.");
    }

    public async Task<DispatcherDto> CreateDispatcherAsync(DispatcherForCreateDto driverForCreate)
    {
        var json = JsonConvert.SerializeObject(driverForCreate);
        var response = await _api.PostAsync("dispatchers", json);

        return await ApiResponseHandler.HandleApiResponse<DispatcherDto>(response, "Error creating dispatcher.");
    }
}
