using CheckDrive.Web.Models;

namespace CheckDrive.Web.Stores.OperatorReviews
{
    public class MockOperatorReviewDataStore : IOperatorReviewDataStore
    {
        private readonly List<OperatorReview> _operatorReviews;

        public MockOperatorReviewDataStore()
        {
            _operatorReviews = new List<OperatorReview>
            {
                new OperatorReview { Id = 1, OilAmount = 5.0, Comments = "Good service", Status = Status.Completed, Date = DateTime.Now, OperatorId = 1, DriverId = 1 },
                new OperatorReview { Id = 2, OilAmount = 6.0, Comments = "Needs improvement", Status = Status.Pending, Date = DateTime.Now, OperatorId = 2, DriverId = 2 },
            };
        }

        public async Task<List<OperatorReview>> GetOperatorReviews()
        {
            await Task.Delay(100);
            return _operatorReviews.ToList();
        }

        public async Task<OperatorReview> GetOperatorReview(int id)
        {
            await Task.Delay(100); 
            return _operatorReviews.FirstOrDefault(or => or.Id == id);
        }

        public async Task<OperatorReview> CreateOperatorReview(OperatorReview operatorReview)
        {
            await Task.Delay(100);
            operatorReview.Id = _operatorReviews.Max(or => or.Id) + 1;
            _operatorReviews.Add(operatorReview);
            return operatorReview;
        }

        public async Task<OperatorReview> UpdateOperatorReview(int id, OperatorReview operatorReview)
        {
            await Task.Delay(100); 
            var existingOperatorReview = _operatorReviews.FirstOrDefault(or => or.Id == id);
            if (existingOperatorReview != null)
            {
                existingOperatorReview.OilAmount = operatorReview.OilAmount;
                existingOperatorReview.Comments = operatorReview.Comments;
                existingOperatorReview.Status = operatorReview.Status;
                existingOperatorReview.Date = operatorReview.Date;
                existingOperatorReview.OperatorId = operatorReview.OperatorId;
                existingOperatorReview.DriverId = operatorReview.DriverId;
            }
            return existingOperatorReview;
        }

        public async Task DeleteOperatorReview(int id)
        {
            await Task.Delay(100); 
            var operatorReviewToRemove = _operatorReviews.FirstOrDefault(or => or.Id == id);
            if (operatorReviewToRemove != null)
            {
                _operatorReviews.Remove(operatorReviewToRemove);
            }
        }
    }
}
