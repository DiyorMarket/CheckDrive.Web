using CheckDrive.ApiContracts.OperatorReview;
using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.OperatorReviews
{
    public interface IOperatorReviewDataStore
    {
        Task<GetOperatorReviewResponse> GetOperatorReviews();
        Task<OperatorReviewDto> GetOperatorReview(int id);
        Task<OperatorReviewDto> CreateOperatorReview(OperatorReviewForCreateDto review);
        Task<OperatorReview> UpdateOperatorReview(int id, OperatorReview operatorReview);
        Task DeleteOperatorReview(int id);
    }
}
