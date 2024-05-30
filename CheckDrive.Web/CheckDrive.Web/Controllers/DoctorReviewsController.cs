using Microsoft.AspNetCore.Mvc;
using CheckDrive.Web.Stores.DoctorReviews;
using System.Threading.Tasks;

namespace CheckDrive.Web.Controllers
{
    public class DoctorReviewsController : Controller
    {
        private readonly IDoctorReviewDataStore _doctorReviewDataStore;

        public DoctorReviewsController(IDoctorReviewDataStore doctorReviewDataStore)
        {
            _doctorReviewDataStore = doctorReviewDataStore;
        }

        public async Task<IActionResult> Index()
        {
            var reviews = await _doctorReviewDataStore.GetDoctorReviews();
            return View(reviews);
        }
    }
}
