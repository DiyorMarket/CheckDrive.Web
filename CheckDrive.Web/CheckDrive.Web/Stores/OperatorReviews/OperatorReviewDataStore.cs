using CheckDrive.ApiContracts.OperatorReview;
using CheckDrive.Web.Exceptions;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Web;

namespace CheckDrive.Web.Stores.OperatorReviews;

public class OperatorReviewDataStore : IOperatorReviewDataStore
{
    private readonly ApiClient _api;

    public OperatorReviewDataStore(ApiClient apiClient)
    {
        _api = apiClient;
    }

    public async Task<GetOperatorReviewResponse> GetOperatorReviewsAsync(string? searchString, int? pageNumber)
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

        var response = await _api.GetAsync($"operators/reviews?{query}");

        return await ApiResponseHandler.HandleApiResponse<GetOperatorReviewResponse>(response, "Could not fetch operator reviews.");
    }

    public async Task<GetOperatorReviewResponse> GetOperatorReviewsAsync()
    {
        var response = await _api.GetAsync("operators/reviews");

        return await ApiResponseHandler.HandleApiResponse<GetOperatorReviewResponse>(response, "Could not fetch operator reviews.");
    }

    public async Task<OperatorReviewDto> GetOperatorReviewByIdAsync(int id)
    {
        var response = await _api.GetAsync($"operators/review/{id}");

        return await ApiResponseHandler.HandleApiResponse<OperatorReviewDto>(response, $"Could not fetch operator review with id: {id}.");
    }

    public async Task<OperatorReviewDto> CreateOperatorReviewAsync(OperatorReviewForCreateDto operatorReviewForCreate)
    {
        var json = JsonConvert.SerializeObject(operatorReviewForCreate);
        var response = await _api.PostAsync("operators/review", json);

        return await ApiResponseHandler.HandleApiResponse<OperatorReviewDto>(response, "Error creating operator.");
    }
}
