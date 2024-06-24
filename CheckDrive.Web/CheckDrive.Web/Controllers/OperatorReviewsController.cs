using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.OperatorReview;
using CheckDrive.Web.Models;
using CheckDrive.Web.Stores.OperatorReviews;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class OperatorReviewsController(IOperatorReviewDataStore operatorReviewDataStore) : Controller
    {
        private readonly IOperatorReviewDataStore _operatorReviewDataStore = operatorReviewDataStore;

        public async Task<IActionResult> Index(int? pageNumber, string? searchString, DateTime? date)
        {

            var operatorReviews = await _operatorReviewDataStore.GetOperatorReviews(pageNumber, searchString, date);

            ViewBag.PageSize = operatorReviews.PageSize;
            ViewBag.PageCount = operatorReviews.TotalPages;
            ViewBag.TotalCount = operatorReviews.TotalCount;
            ViewBag.CurrentPage = operatorReviews.PageNumber;
            ViewBag.HasPreviousPage = operatorReviews.HasPreviousPage;
            ViewBag.HasNextPage = operatorReviews.HasNextPage;

            var operatorReviewss = operatorReviews.Data.Select(r => new
            {
                r.Id,
                r.OperatorName,
                r.DriverName,
                r.OilAmount,
                r.Date,
                r.Comments,
                Status = ((StatusForDto)r.Status) switch
                {
                    StatusForDto.Pending => "Pending",
                    StatusForDto.Completed => "Completed",
                    StatusForDto.Rejected => "Rejected",
                    StatusForDto.Unassigned => "Unassigned",
                    _ => "Unknown Status"
                }
            }).ToList();

            ViewBag.OperatorsReview = operatorReviewss;
            return View();

        }

        public async Task<IActionResult> Details(int id)
        {
            var operatorReview = await _operatorReviewDataStore.GetOperatorReview(id);
            if (operatorReview == null)
            {
                return NotFound();
            }
            return View(operatorReview);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OilAmount,Comments,Status,Date,OperatorId,DriverId")] OperatorReviewForCreateDto operatorReview)
        {
            if (ModelState.IsValid)
            {
                await _operatorReviewDataStore.CreateOperatorReview(operatorReview);
                return RedirectToAction(nameof(Index));
            }
            return View(operatorReview);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var operatorReview = await _operatorReviewDataStore.GetOperatorReview(id);
            if (operatorReview == null)
            {
                return NotFound();
            }
            return View(operatorReview);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OilAmount,Comments,Status,Date,OperatorId,DriverId")] OperatorReview operatorReview)
        {
            if (id != operatorReview.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _operatorReviewDataStore.UpdateOperatorReview(id, operatorReview);
                }
                catch (Exception)
                {
                    if (!await OperatorReviewExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(operatorReview);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var operatorReview = await _operatorReviewDataStore.GetOperatorReview(id);
            if (operatorReview == null)
            {
                return NotFound();
            }
            return View(operatorReview);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _operatorReviewDataStore.DeleteOperatorReview(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> OperatorReviewExists(int id)
        {
            var operatorReview = await _operatorReviewDataStore.GetOperatorReview(id);
            return operatorReview != null;
        }
    }
}
