using CheckDrive.ApiContracts.Mechanic;
using CheckDrive.Web.Exceptions;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Web;

namespace CheckDrive.Web.Stores.Mechanics;

public class MechanicDataStore : IMechanicDataStore
{
    private readonly ApiClient _api;

    public MechanicDataStore(ApiClient apiClient)
    {
        _api = apiClient;
    }

    public async Task<GetMechanicResponse> GetMechanicsAsync(string? searchString, int? pageNumber)
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

        var response = await _api.GetAsync($"mechanics?{query}");

        return await ApiResponseHandler.HandleApiResponse<GetMechanicResponse>(response, "Could not fetch mechanics.");
    }

    public async Task<GetMechanicResponse> GetMechanicsAsync()
    {
        var response = await _api.GetAsync("mechanics");

        return await ApiResponseHandler.HandleApiResponse<GetMechanicResponse>(response, "Could not fetch mechanics.");
    }

    public async Task<MechanicDto> GetMechanicByIdAsync(int id)
    {
        var response = await _api.GetAsync($"mechanics/{id}");

        return await ApiResponseHandler.HandleApiResponse<MechanicDto>(response, $"Could not fetch mechanic with id: {id}.");
    }

    public async Task<MechanicDto> CreateMechanicAsync(MechanicForCreateDto mechanicForCreate)
    {
        var json = JsonConvert.SerializeObject(mechanicForCreate);
        var response = await _api.PostAsync("mechanics", json);

        return await ApiResponseHandler.HandleApiResponse<MechanicDto>(response, "Error creating mechanic.");
    }
}
