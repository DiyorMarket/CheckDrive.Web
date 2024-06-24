using CheckDrive.ApiContracts.DispatcherReview;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.DispatcherReviews
{
    public interface IDispatcherReviewDataStore
    {
        Task<GetDispatcherReviewResponse> GetDispatcherReviews(int? pageNumber, string? searchString, DateTime? date);
        Task<DispatcherReviewForCreateDto> GetDispatcherReview(int id);
        Task<DispatcherReviewDto> CreateDispatcherReview(DispatcherReviewForCreateDto review);
        Task<DispatcherReviewForUpdateDto> UpdateDispatcherReview(int id, DispatcherReviewForUpdateDto review);
        Task DeleteDispatcherReview(int id);
    }
}
