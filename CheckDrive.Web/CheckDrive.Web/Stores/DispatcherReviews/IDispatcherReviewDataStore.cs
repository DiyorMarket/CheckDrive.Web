using CheckDrive.ApiContracts.DispatcherReview;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.DispatcherReviews
{
    public interface IDispatcherReviewDataStore
    {
        Task<GetDispatcherReviewResponse> GetDispatcherReviews(int? pageNumber, string? searchString, DateTime? date, int? roleId, int? accountId);
        Task<GetDispatcherReviewResponse> GetDispatcherReviews(int? pageNumber, string? searchString, DateTime? date, string? status);
        Task<DispatcherReviewDto> GetDispatcherReview(int id);
        Task<DispatcherReviewDto> CreateDispatcherReview(DispatcherReviewForCreateDto review);
        Task<DispatcherReviewDto> UpdateDispatcherReview(int id, DispatcherReviewForUpdateDto review);
        Task DeleteDispatcherReview(int id);
        Task<Stream> GetExportFile(int year, int month);
    }
}
