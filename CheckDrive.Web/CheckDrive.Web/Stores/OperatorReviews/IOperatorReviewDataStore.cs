using CheckDrive.ApiContracts.OperatorReview;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.OperatorReviews
{
    public interface IOperatorReviewDataStore
    {
        Task<GetOperatorReviewResponse> GetOperatorReviews(int? pageNumber, string? searchString, DateTime? date, string? status, int? roleId, int? accountId);
        Task<OperatorReviewDto> GetOperatorReview(int id);
        Task<OperatorReviewDto> CreateOperatorReview(OperatorReviewForCreateDto review);
        Task<OperatorReviewDto> UpdateOperatorReview(int id, OperatorReviewForUpdateDto operatorReview);
        Task DeleteOperatorReview(int id);
        Task<Stream> GetExportFile(int year, int month);
    }
}
