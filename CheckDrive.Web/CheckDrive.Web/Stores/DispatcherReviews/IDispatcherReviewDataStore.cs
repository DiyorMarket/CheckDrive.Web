using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.DispatcherReviews
{
    public interface IDispatcherReviewDataStore
    {
        Task<GetDispatcherReviewResponse> GetDispatcherReviews(int? pageNumber, string? searchString, DateTime? date);
        Task<DispatcherReview> GetDispatcherReview(int id);
        Task<DispatcherReview> CreateDispatcherReview(DispatcherReview review);
        Task<DispatcherReview> UpdateDispatcherReview(int id, DispatcherReview review);
        Task DeleteDispatcherReview(int id);
    }
}
