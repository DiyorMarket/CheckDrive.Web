using CheckDrive.ApiContracts.DoctorReview;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Services;

namespace CheckDrive.Web.Stores.DoctorReviews;

public class DoctorReviewDataStore : IDoctorReviewDataStore
{
    private readonly CheckDriveApi _apiClient;

    public DoctorReviewDataStore(CheckDriveApi apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<DoctorReviewDto> CreateDoctorReviewAsync(DoctorReviewForCreateDto review)
    {
        throw new NotImplementedException();
    }

    public Task DeleteDoctorReviewAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<DoctorReviewDto> GetDoctorReviewAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<GetDoctorReviewResponse> GetDoctorReviewsAsync(int? pageNumber, string? searchString, DateTime? date, bool? isHealthy, int? roleId, int? accountID)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DoctorReviewDto>>? GetTodayReviewsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<DoctorReviewDto> UpdateDoctorReviewAsync(int id, DoctorReviewForUpdateDto review)
    {
        throw new NotImplementedException();
    }
}
