using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.Mechanic;
using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.Web.Stores.Cars;
using CheckDrive.Web.Stores.Drivers;
using CheckDrive.Web.Stores.MechanicAcceptances;
using CheckDrive.Web.Stores.MechanicHandovers;
using CheckDrive.Web.Stores.Mechanics;
using CheckDrive.Web.Stores.OperatorReviews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Versioning;
using System.Linq;

namespace CheckDrive.Web.Controllers
{
    public class MechanicAcceptancesController(IMechanicAcceptanceDataStore mechanicAcceptanceDataStore, IMechanicDataStore mechanicDataStore, IOperatorReviewDataStore operatorReviewDataStore) : Controller
    {
        private readonly IMechanicAcceptanceDataStore _mechanicAcceptanceDataStore = mechanicAcceptanceDataStore;
        private readonly IMechanicDataStore _mechanicDataStore = mechanicDataStore;
        private readonly IOperatorReviewDataStore _operatorReviewDataStore = operatorReviewDataStore;

        public async Task<IActionResult> Index(int? pageNumber, string? searchString, DateTime? date)
        {

            var response = await _mechanicAcceptanceDataStore.GetMechanicAcceptancesAsync(pageNumber, searchString, date, null, 1);

            ViewBag.PageSize = response.PageSize;
            ViewBag.PageCount = response.TotalPages;
            ViewBag.TotalCount = response.TotalCount;
            ViewBag.CurrentPage = response.PageNumber;
            ViewBag.HasPreviousPage = response.HasPreviousPage;
            ViewBag.HasNextPage = response.HasNextPage;

            var mechanicAcceptances = response.Data.Select(r => new
            {
                r.Id,
                IsAccepted = (bool)r.IsAccepted ? "Qabul qilindi" : "Rad etildi",
                r.Comments,
                Status = ((StatusForDto)r.Status) switch
                {
                    StatusForDto.Pending => "Pending",
                    StatusForDto.Completed => "Completed",
                    StatusForDto.Rejected => "Rejected",
                    StatusForDto.Unassigned => "Unassigned",
                    _ => "Unknown Status"
                },
                r.Date,
                r.Distance,
                r.DriverName,
                r.MechanicName,
                r.CarName,
                r.CarId
            }).ToList();

            ViewBag.MechanicAcceptances = mechanicAcceptances;

            return View();
        }


        public async Task<IActionResult> PersonalIndex(string? searchString, int? pageNumber)
        {
            var response = await _mechanicAcceptanceDataStore.GetMechanicAcceptancesAsync(pageNumber, searchString, null, null, 6);

            ViewBag.PageSize = response.PageSize;
            ViewBag.PageCount = response.TotalPages;
            ViewBag.TotalCount = response.TotalCount;
            ViewBag.CurrentPage = response.PageNumber;
            ViewBag.HasPreviousPage = response.HasPreviousPage;
            ViewBag.HasNextPage = response.HasNextPage;

            return View(response.Data);
        }
        public async Task<IActionResult> CreateByButton()
        {
            var operatorResponse = await _operatorReviewDataStore.GetOperatorReviews(null, null, DateTime.Today, "Completed", 1);
            var mechanicAcceptanceResponse = await _mechanicAcceptanceDataStore.GetMechanicAcceptancesAsync(null, null, DateTime.Today, null, null);

            var mechanicDriverIds = mechanicAcceptanceResponse.Data.Select(ma => ma.DriverId).ToHashSet();
            var filteredOperatorResponse = operatorResponse.Data.Where(or => !mechanicDriverIds.Contains(or.DriverId)).ToList();

            var accountIdStr = TempData["AccountId"] as string;
            TempData.Keep("AccountId");
            var mechanic = new MechanicDto();
            if (int.TryParse(accountIdStr, out int accountId))
            {
                var mechanicResponse = await _mechanicDataStore.GetMechanics(accountId);
                mechanic = mechanicResponse.Data.FirstOrDefault();
            }
            ViewBag.MechanicId = mechanic.Id;

            ViewBag.FilteredOperatorResponse = filteredOperatorResponse;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IsAccepted,Comments,MechanicId,Distance,CarId,DriverId")] MechanicAcceptanceForCreateDto mechanicAcceptanceForCreateDto)
        {
            if (ModelState.IsValid)
            {
                if (mechanicAcceptanceForCreateDto.IsAccepted == null)
                {
                    mechanicAcceptanceForCreateDto.IsAccepted = false;
                }

                mechanicAcceptanceForCreateDto.Date = DateTime.Now;
                mechanicAcceptanceForCreateDto.Status = mechanicAcceptanceForCreateDto.IsAccepted ? StatusForDto.Pending : StatusForDto.Rejected;
                await _mechanicAcceptanceDataStore.CreateMechanicAcceptanceAsync(mechanicAcceptanceForCreateDto);
                return RedirectToAction(nameof(PersonalIndex));
            }

            return View(mechanicAcceptanceForCreateDto);
        }

        public async Task<IActionResult> CreateByLink(int driverId, int carId, string carName, string driverName)
        {
            var accountIdStr = TempData["AccountId"] as string;
            TempData.Keep("AccountId");
            var mechanic = new MechanicDto();
            if (int.TryParse(accountIdStr, out int accountId))
            {
                var mechanicResponse = await _mechanicDataStore.GetMechanics(accountId);
                mechanic = mechanicResponse.Data.FirstOrDefault();
            }

            ViewBag.MechanicId = mechanic.Id;
            ViewBag.CarId = carId;
            ViewBag.DriverId = driverId;
            ViewBag.CarName = carName;
            ViewBag.DriverName = driverName;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MechanicAcceptanceForUpdateDto mechanicAcceptance)
        {
            if (id != mechanicAcceptance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _mechanicAcceptanceDataStore.UpdateMechanicAcceptanceAsync(id, mechanicAcceptance);
                }
                catch (Exception)
                {
                    if (!await MechanicAcceptanceExists(id))
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

            return View(mechanicAcceptance);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var mechanicAcceptance = await _mechanicAcceptanceDataStore.GetMechanicAcceptanceAsync(id);
            if (mechanicAcceptance == null)
            {
                return NotFound();
            }
            return View(mechanicAcceptance);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mechanicAcceptanceDataStore.DeleteMechanicAcceptanceAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> MechanicAcceptanceExists(int id)
        {
            var mechanicAcceptance = await _mechanicAcceptanceDataStore.GetMechanicAcceptanceAsync(id);
            return mechanicAcceptance != null;
        }
    }
}