using CheckDrive.ApiContracts.MechanicHandover;
using CheckDrive.Web.Exceptions;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Web;

namespace CheckDrive.Web.Stores.MechanicHandovers;

public class MechanicHandoverDataStore : IMechanicHandoverDataStore
{
    private readonly ApiClient _api;

    public MechanicHandoverDataStore(ApiClient apiClient)
    {
        _api = apiClient;
    }

    public async Task<GetMechanicHandoverResponse> GetMechanicHandoversAsync(string? searchString, int? pageNumber)
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

        var response = await _api.GetAsync($"mechanics/handovers?{query}");

        return await ApiResponseHandler.HandleApiResponse<GetMechanicHandoverResponse>(response, "Could not fetch handovers.");
    }

    public async Task<GetMechanicHandoverResponse> GetMechanicHandoversAsync()
    {
        var response = await _api.GetAsync("mechanic/handovers");

        return await ApiResponseHandler.HandleApiResponse<GetMechanicHandoverResponse>(response, "Could not fetch handovers.");
    }

    public async Task<MechanicHandoverDto> GetMechanicHandoverByIdAsync(int id)
    {
        var response = await _api.GetAsync($"mechanics/handover/{id}");

        return await ApiResponseHandler.HandleApiResponse<MechanicHandoverDto>(response, $"Could not fetch mechanic handover with id: {id}.");
    }

    public async Task<MechanicHandoverDto> CreateMechanicHandoverAsync(MechanicHandoverForCreateDto handoverForCreate)
    {
        var json = JsonConvert.SerializeObject(handoverForCreate);
        var response = await _api.PostAsync("mechanics/handover", json);

        return await ApiResponseHandler.HandleApiResponse<MechanicHandoverDto>(response, "Error creating mechanic handover.");
    }
}
