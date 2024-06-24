using CheckDrive.ApiContracts.DoctorReview;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.DoctorReviews
{
    public interface IDoctorReviewDataStore
    {
        Task<GetDoctorReviewResponse> GetDoctorReviewsAsync(int? pageNumber,string? searchString,DateTime? date);
        Task<IEnumerable<DoctorReviewDto>>? GetTodayReviewsAsync();
        Task<DoctorReviewDto> GetDoctorReviewAsync(int id);
        Task<DoctorReviewDto> CreateDoctorReviewAsync(DoctorReviewForCreateDto review);
        Task<DoctorReviewDto> UpdateDoctorReviewAsync(int id, DoctorReviewForUpdateDto review);
        Task DeleteDoctorReviewAsync(int id);
    }
}
