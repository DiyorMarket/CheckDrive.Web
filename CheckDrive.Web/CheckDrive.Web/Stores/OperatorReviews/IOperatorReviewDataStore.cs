using CheckDrive.ApiContracts.OperatorReview;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.OperatorReviews;

public interface IOperatorReviewDataStore
{
    Task<GetOperatorReviewResponse> GetOperatorReviewsAsync(string? searchString, int? pageNumber);
    Task<GetOperatorReviewResponse> GetOperatorReviewsAsync();
    Task<OperatorReviewDto> GetOperatorReviewByIdAsync(int id);
    Task<OperatorReviewDto> CreateOperatorReviewAsync(OperatorReviewForCreateDto operatorForCreate);
}
