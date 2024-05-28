using CheckDrive.ApiContracts.DoctorReview;
using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.DoctorReviews
{
    public interface IDoctorReviewDataStore
    {
        Task<GetDoctorReviewResponse> GetDoctorReviews();
        Task<DoctorReviewDto> GetDoctorReview(int id);
        Task<DoctorReviewDto> CreateDoctorReview(DoctorReviewForCreateDto review);
        Task<DoctorReview> UpdateDoctorReview(int id, DoctorReviewForUpdateDto review);
        Task DeleteDoctorReview(int id);
    }
}
