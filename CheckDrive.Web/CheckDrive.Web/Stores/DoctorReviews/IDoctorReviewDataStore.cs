using CheckDrive.Web.Models;

namespace CheckDrive.Web.Stores.DoctorReviews
{
    public interface IDoctorReviewDataStore
    {
        Task<List<DoctorReview>> GetDoctorReviews();
        Task<DoctorReview> GetDoctorReview(int id);
        Task<DoctorReview> CreateDoctorReview(DoctorReview review);
        Task<DoctorReview> UpdateDoctorReview(int id, DoctorReview review);
        Task DeleteDoctorReview(int id);
    }
}
