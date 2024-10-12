using CheckDrive.ApiContracts.DispatcherReview;
using CheckDrive.Web.Exceptions;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Web;

namespace CheckDrive.Web.Stores.DispatcherReviewDataStore;

public class DispatcherReviewDataStore : IDispatcherReviewDataStore
{
    private readonly ApiClient _api;

    public DispatcherReviewDataStore(ApiClient apiClient)
    {
        _api = apiClient;
    }

    public async Task<GetDispatcherReviewResponse> GetDispatcherReviewsAsync(string? searchString, int? pageNumber)
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

        var response = await _api.GetAsync($"dispatchers/reviews?{query}");

        return await ApiResponseHandler.HandleApiResponse<GetDispatcherReviewResponse>(response, "Could not fetch dispatcher reviews.");
    }

    public async Task<GetDispatcherReviewResponse> GetDispatcherReviewsAsync()
    {
        var response = await _api.GetAsync("dispatchers/reviews");

        return await ApiResponseHandler.HandleApiResponse<GetDispatcherReviewResponse>(response, "Could not fetch dispatcher reviews.");
    }

    public async Task<DispatcherReviewDto> GetDispatcherReviewByIdAsync(int id)
    {
        var response = await _api.GetAsync($"dispatchers/review/{id}");

        return await ApiResponseHandler.HandleApiResponse<DispatcherReviewDto>(response, $"Could not fetch dispatcher review with id: {id}.");
    }

    public async Task<DispatcherReviewDto> CreateDispatcherReviewAsync(DispatcherReviewForCreateDto driverForCreate)
    {
        var json = JsonConvert.SerializeObject(driverForCreate);
        var response = await _api.PostAsync("dispatchers/reviews", json);

        return await ApiResponseHandler.HandleApiResponse<DispatcherReviewDto>(response, "Error creating dispatcher review.");
    }
}
