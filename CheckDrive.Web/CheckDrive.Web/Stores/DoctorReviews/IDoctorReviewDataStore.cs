using CheckDrive.ApiContracts.DoctorReview;
using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.DoctorReviews
{
    public interface IDoctorReviewDataStore
    {
        Task<GetDoctorReviewResponse> GetDoctorReviewsAsync(int? pageNumber,string? searchString);
        Task<IEnumerable<DoctorReviewDto>>? GetTodayReviewsAsync();
        Task<DoctorReviewDto> GetDoctorReviewAsync(int id);
        Task<DoctorReviewDto> CreateDoctorReviewAsync(DoctorReviewForCreateDto review);
        Task<DoctorReview> UpdateDoctorReviewAsync(int id, DoctorReviewForUpdateDto review);
        Task DeleteDoctorReviewAsync(int id);
    }
}
