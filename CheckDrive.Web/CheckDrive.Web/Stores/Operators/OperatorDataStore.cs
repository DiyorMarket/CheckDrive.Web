using CheckDrive.ApiContracts.Operator;
using CheckDrive.Web.Exceptions;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Web;

namespace CheckDrive.Web.Stores.Operators;

public class OperatorDataStore : IOperatorDataStore
{
    private readonly ApiClient _api;

    public OperatorDataStore(ApiClient apiClient)
    {
        _api = apiClient;
    }

    public async Task<GetOperatorResponse> GetOperatorsAsync()
    {
        var response = await _api.GetAsync("operators");

        return await ApiResponseHandler.HandleApiResponse<GetOperatorResponse>(response, "Could not fetch operators.");
    }

    public async Task<OperatorDto> GetOperatorByIdAsync(int id)
    {
        var response = await _api.GetAsync($"operators/{id}");

        return await ApiResponseHandler.HandleApiResponse<OperatorDto>(response, $"Could not fetch operator with id: {id}.");
    }

    public async Task<OperatorDto> CreateOperatorAsync(OperatorForCreateDto operatorForCreate)
    {
        var json = JsonConvert.SerializeObject(operatorForCreate);
        var response = await _api.PostAsync("operators", json);

        return await ApiResponseHandler.HandleApiResponse<OperatorDto>(response, "Error creating operator.");
    }
}
