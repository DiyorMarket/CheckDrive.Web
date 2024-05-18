using CheckDrive.Web.Models;

namespace CheckDrive.Web.Stores.OperatorReviews
{
    public interface IOperatorReviewDataStore
    {
        Task<List<OperatorReview>> GetOperatorReviews();
        Task<OperatorReview> GetOperatorReview(int id);
        Task<OperatorReview> CreateOperatorReview(OperatorReview operatorReview);
        Task<OperatorReview> UpdateOperatorReview(int id, OperatorReview operatorReview);
        Task DeleteOperatorReview(int id);
    }
}
