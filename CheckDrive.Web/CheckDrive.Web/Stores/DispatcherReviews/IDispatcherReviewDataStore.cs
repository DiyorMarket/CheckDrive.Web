using CheckDrive.Web.Models;

namespace CheckDrive.Web.Stores.DispatcherReviews
{
    public interface IDispatcherReviewDataStore
    {
        Task<List<DispatcherReview>> GetDispatcherReviews();
        Task<DispatcherReview> GetDispatcherReview(int id);
        Task<DispatcherReview> CreateDispatcherReview(DispatcherReview review);
        Task<DispatcherReview> UpdateDispatcherReview(int id, DispatcherReview review);
        Task DeleteDispatcherReview(int id);
    }
}
