using CheckDrive.ApiContracts.DispatcherReview;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.DispatcherReviewDataStore;

public interface IDispatcherReviewDataStore
{
    Task<GetDispatcherReviewResponse> GetDispatcherReviewsAsync(string? searchString, int? pageNumber);
    Task<GetDispatcherReviewResponse>GetDispatcherReviewsAsync();
    Task<DispatcherReviewDto> GetDispatcherReviewByIdAsync(int id);
    Task<DispatcherReviewDto> CreateDispatcherReviewAsync(DispatcherReviewForCreateDto dispatcherForCreate);
}
