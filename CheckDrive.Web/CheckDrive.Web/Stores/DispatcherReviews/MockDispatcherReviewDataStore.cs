using CheckDrive.Web.Models;

namespace CheckDrive.Web.Stores.DispatcherReviews
{
    public class MockDispatcherReviewDataStore : IDispatcherReviewDataStore
    {
        private readonly List<DispatcherReview> _reviews;

        public MockDispatcherReviewDataStore()
        {
            _reviews = new List<DispatcherReview>
            {
                new DispatcherReview { Id = 1, FuelSpended = 50, DistanceCovered = 100, Date = DateTime.Now },
                new DispatcherReview { Id = 2, FuelSpended = 60, DistanceCovered = 120, Date = DateTime.Now.AddDays(-1) },
            };
        }

        public async Task<List<DispatcherReview>> GetDispatcherReviews()
        {
            await Task.Delay(100);
            return _reviews.ToList();
        }

        public async Task<DispatcherReview> GetDispatcherReview(int id)
        {
            await Task.Delay(100);
            return _reviews.FirstOrDefault(r => r.Id == id);
        }

        public async Task<DispatcherReview> CreateDispatcherReview(DispatcherReview review)
        {
            await Task.Delay(100);
            review.Id = _reviews.Max(r => r.Id) + 1;
            _reviews.Add(review);
            return review;
        }

        public async Task<DispatcherReview> UpdateDispatcherReview(int id, DispatcherReview review)
        {
            await Task.Delay(100); 
            var existingReview = _reviews.FirstOrDefault(r => r.Id == id);
            if (existingReview != null)
            {
                existingReview.FuelSpended = review.FuelSpended;
                existingReview.DistanceCovered = review.DistanceCovered;
                existingReview.Date = review.Date;
            }
            return existingReview;
        }

        public async Task DeleteDispatcherReview(int id)
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
