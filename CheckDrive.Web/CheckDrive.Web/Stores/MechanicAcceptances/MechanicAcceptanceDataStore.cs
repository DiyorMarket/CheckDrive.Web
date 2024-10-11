using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.Web.Exceptions;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Web;

namespace CheckDrive.Web.Stores.MechanicAcceptances;

public class MechanicAcceptanceDataStore : IMechanicAcceptanceDataStore
{
    private readonly ApiClient _api;

    public MechanicAcceptanceDataStore(ApiClient apiClient)
    {
        _api = apiClient;
    }

    public async Task<GetMechanicAcceptanceResponse> GetMechanicAcceptancesAsync(string? searchString, int? pageNumber)
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

        var response = await _api.GetAsync($"mechanics/acceptances?{query}");

        return await ApiResponseHandler.HandleApiResponse<GetMechanicAcceptanceResponse>(response, "Could not fetch acceptances.");
    }

    public async Task<GetMechanicAcceptanceResponse> GetMechanicAcceptancesAsync()
    {
        var response = await _api.GetAsync("mechanic/acceptances");

        return await ApiResponseHandler.HandleApiResponse<GetMechanicAcceptanceResponse>(response, "Could not fetch acceptances.");
    }

    public async Task<MechanicAcceptanceDto> GetMechanicAcceptanceByIdAsync(int id)
    {
        var response = await _api.GetAsync($"mechanics/acceptance/{id}");

        return await ApiResponseHandler.HandleApiResponse<MechanicAcceptanceDto>(response, $"Could not fetch mechanic acceptance with id: {id}.");
    }

    public async Task<MechanicAcceptanceDto> CreateMechanicAcceptanceAsync(MechanicAcceptanceForCreateDto acceptanceForCreate)
    {
        var json = JsonConvert.SerializeObject(acceptanceForCreate);
        var response = await _api.PostAsync("mechanics/acceptance", json);

        return await ApiResponseHandler.HandleApiResponse<MechanicAcceptanceDto>(response, "Error creating mechanic acceptance.");
    }
}
