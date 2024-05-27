using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.DoctorReviews
{
    public interface IDoctorReviewDataStore
    {
        Task<GetDoctorReviewResponse> GetDoctorReviews();
        Task<DoctorReview> GetDoctorReview(int id);
        Task<DoctorReview> CreateDoctorReview(DoctorReview review);
        Task<DoctorReview> UpdateDoctorReview(int id, DoctorReview review);
        Task DeleteDoctorReview(int id);
    }
}
