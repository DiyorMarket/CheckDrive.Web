using CheckDrive.ApiContracts.DoctorReview;
using CheckDrive.Web.Exceptions;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Web;

namespace CheckDrive.Web.Stores.DoctorReviews;

public class DoctorReviewDataStore : IDoctorReviewDataStore
{
    private readonly ApiClient _api;

    public DoctorReviewDataStore(ApiClient apiClient)
    {
        _api = apiClient;
    }

    public async Task<GetDoctorReviewResponse> GetDoctorReviewsAsync(string? searchString, int? pageNumber)
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

        var response = await _api.GetAsync($"doctors/reviews?{query}");

        return await ApiResponseHandler.HandleApiResponse<GetDoctorReviewResponse>(response, "Could not fetch doctor reviews.");
    }

    public async Task<GetDoctorReviewResponse> GetDoctorReviewsAsync()
    {
        var response = await _api.GetAsync("doctors/reviews");

        return await ApiResponseHandler.HandleApiResponse<GetDoctorReviewResponse>(response, "Could not fetch doctor reviews.");
    }

    public async Task<DoctorReviewDto> GetDoctorReviewByIdAsync(int id)
    {
        var response = await _api.GetAsync($"doctors/review/{id}");

        return await ApiResponseHandler.HandleApiResponse<DoctorReviewDto>(response, $"Could not fetch doctor review with id: {id}.");
    }

    public async Task<DoctorReviewDto> CreateDoctorReviewAsync(DoctorReviewForCreateDto doctorReviewForCreate)
    {
        var json = JsonConvert.SerializeObject(doctorReviewForCreate);
        var response = await _api.PostAsync("doctors/review", json);

        return await ApiResponseHandler.HandleApiResponse<DoctorReviewDto>(response, "Error creating doctor review.");
    }
}
