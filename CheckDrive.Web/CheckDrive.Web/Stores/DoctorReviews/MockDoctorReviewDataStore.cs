using CheckDrive.Web.Models;

namespace CheckDrive.Web.Stores.DoctorReviews
{
    public class MockDoctorReviewDataStore : IDoctorReviewDataStore
    {
        private readonly List<DoctorReview> _reviews;

        public MockDoctorReviewDataStore()
        {
            _reviews = new List<DoctorReview>
            {
                new DoctorReview { Id = 1, IsHealthy = true, Comments = "Good condition", Date = DateTime.Now },
                new DoctorReview { Id = 2, IsHealthy = false, Comments = "Needs rest", Date = DateTime.Now.AddDays(-1) },
            };
        }

        public async Task<List<DoctorReview>> GetDoctorReviews()
        {
            await Task.Delay(100);
            return _reviews.ToList();
        }

        public async Task<DoctorReview> GetDoctorReview(int id)
        {
            await Task.Delay(100); 
            return _reviews.FirstOrDefault(r => r.Id == id);
        }

        public async Task<DoctorReview> CreateDoctorReview(DoctorReview review)
        {
            await Task.Delay(100); 
            review.Id = _reviews.Max(r => r.Id) + 1;
            _reviews.Add(review);
            return review;
        }

        public async Task<DoctorReview> UpdateDoctorReview(int id, DoctorReview review)
        {
            await Task.Delay(100);
            var existingReview = _reviews.FirstOrDefault(r => r.Id == id);
            if (existingReview != null)
            {
                existingReview.IsHealthy = review.IsHealthy;
                existingReview.Comments = review.Comments;
                existingReview.Date = review.Date;
            }
            return existingReview;
        }

        public async Task DeleteDoctorReview(int id)
        {
            await Task.Delay(100); 
            var reviewToRemove = _reviews.FirstOrDefault(r => r.Id == id);
            if (reviewToRemove != null)
            {
                _reviews.Remove(reviewToRemove);
            }
        }
    }
}
