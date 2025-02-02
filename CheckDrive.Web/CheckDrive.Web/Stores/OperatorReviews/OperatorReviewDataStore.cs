using CheckDrive.ApiContracts.OperatorReview;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Services;

namespace CheckDrive.Web.Stores.OperatorReviews;

public class OperatorReviewDataStore : IOperatorReviewDataStore
{
    private readonly CheckDriveApi _apiClient;

    public OperatorReviewDataStore(CheckDriveApi apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<OperatorReviewDto> CreateOperatorReview(OperatorReviewForCreateDto review)
    {
        throw new NotImplementedException();
    }

    public Task DeleteOperatorReview(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Stream> GetExportFile(int year, int month)
    {
        throw new NotImplementedException();
    }

    public Task<OperatorReviewDto> GetOperatorReview(int id)
    {
        throw new NotImplementedException();
    }

    public Task<GetOperatorReviewResponse> GetOperatorReviews(int? pageNumber, string? searchString, DateTime? date, string? status, int? roleId, int? accountId)
    {
        throw new NotImplementedException();
    }

    public Task<OperatorReviewDto> UpdateOperatorReview(int id, OperatorReviewForUpdateDto operatorReview)
    {
        throw new NotImplementedException();
    }
}
