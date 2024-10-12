using CheckDrive.ApiContracts.DoctorReview;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.DoctorReviews;

public interface IDoctorReviewDataStore
{
    Task<GetDoctorResponse> GetDoctorReviewsAsync(string? searchString, int? pageNumber);
    Task<GetDoctorResponse> GetDoctorReviewsAsync();
    Task<DoctorReviewDto> GetDoctorReviewByIdAsync(int id);
    Task<DoctorReviewDto> CreateDoctorReviewAsync(DoctorReviewForCreateDto doctorReviewForCreate);
}
