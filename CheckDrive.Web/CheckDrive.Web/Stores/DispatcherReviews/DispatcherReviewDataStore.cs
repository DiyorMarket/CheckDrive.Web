using CheckDrive.ApiContracts.DispatcherReview;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Services;

namespace CheckDrive.Web.Stores.DispatcherReviews;

public class DispatcherReviewDataStore : IDispatcherReviewDataStore
{
    private readonly CheckDriveApi _apiClient;

    public DispatcherReviewDataStore(CheckDriveApi apiClient)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public Task<DispatcherReviewDto> CreateDispatcherReview(DispatcherReviewForCreateDto review)
    {
        throw new NotImplementedException();
    }

    public Task DeleteDispatcherReview(int id)
    {
        throw new NotImplementedException();
    }

    public Task<DispatcherReviewDto> GetDispatcherReview(int id)
    {
        throw new NotImplementedException();
    }

    public Task<GetDispatcherReviewResponse> GetDispatcherReviews(int? pageNumber, string? searchString, DateTime? date, int? roleId, int? accountId)
    {
        throw new NotImplementedException();
    }

    public Task<Stream> GetExportFile(int year, int month)
    {
        throw new NotImplementedException();
    }

    public Task<DispatcherReviewDto> UpdateDispatcherReview(int id, DispatcherReviewForUpdateDto review)
    {
        throw new NotImplementedException();
    }
}
