using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.OperatorReviews
{
    public interface IOperatorReviewDataStore
    {
        Task<GetOperatorReviewResponse> GetOperatorReviews();
        Task<OperatorReview> GetOperatorReview(int id);
        Task<OperatorReview> CreateOperatorReview(OperatorReview operatorReview);
        Task<OperatorReview> UpdateOperatorReview(int id, OperatorReview operatorReview);
        Task DeleteOperatorReview(int id);
    }
}
