using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.Debts;
using CheckDrive.Web.Stores.Debts;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class DebtsController(IDebtDataStore debtDataStore) : Controller
    {
        private readonly IDebtDataStore _debtDataStore = debtDataStore;

        public async Task<IActionResult> Index(string? searchString, int? pageNumber)
        {
            var debts = await _debtDataStore.GetDebtsAsync(searchString, pageNumber, null);
            ViewBag.Debts = debts.Data.ToList();
            ViewBag.SearchString = searchString;

            ViewBag.PageSize = debts.PageSize;
            ViewBag.PageCount = debts.TotalPages;
            ViewBag.TotalCount = debts.TotalCount;
            ViewBag.CurrentPage = debts.PageNumber;
            ViewBag.HasPreviousPage = debts.HasPreviousPage;
            ViewBag.HasNextPage = debts.HasNextPage;

            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var debt = await _debtDataStore.GetDebtByIdAsync(id);

            return View(debt);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OilAmount, Status, DriverId")] DebtsForCreateDto debtsForCreateDto)
        {
            if (ModelState.IsValid)
            {
                await _debtDataStore.CreateDebtAsync(debtsForCreateDto);
                return RedirectToAction(nameof(Index));
            }
            return View(debtsForCreateDto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var debt = await _debtDataStore.GetDebtByIdAsync(id);

            return View(debt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date, OilAmount, DispatcherReviewId, Status, DriverId, CarId")] DebtsForUpdateDto debtsForUpdateDto)
        {
            if (ModelState.IsValid)
            {
                await _debtDataStore.UpdateDebtAsync(id, debtsForUpdateDto);
                return RedirectToAction(nameof(Index));
            }
            return View(debtsForUpdateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id)
        {
            var debt = await _debtDataStore.GetDebtByIdAsync(id);

            if (debt != null)
            {
                debt.Status = StatusForDto.Completed;
                await _debtDataStore.UpdateDebtAsync(id, new DebtsForUpdateDto
                {
                    Id = debt.Id,
                    OilAmount = 0,
                    Status = StatusForDto.Completed,
                    DriverId = debt.DriverId,
                    Date = debt.Date,
                    DispatcherReviewId = debt.DispatcherReviewId,
                    CarId = debt.CarId
                });
            }

            return RedirectToAction(nameof(Details), new { id = id });
        }


        public async Task<IActionResult> Delete(int id)
        {
            var oilMark = await _debtDataStore.GetDebtByIdAsync(id);

            return View(oilMark);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _debtDataStore.DeleteDebtAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
