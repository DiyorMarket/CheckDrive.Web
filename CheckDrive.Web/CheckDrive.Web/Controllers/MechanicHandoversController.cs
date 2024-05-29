using CheckDrive.ApiContracts;
using CheckDrive.Web.Models;
using CheckDrive.Web.Stores.MechanicHandovers;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class MechanicHandoversController : Controller
    {
        private readonly IMechanicHandoverDataStore _mechanicHandoverDataStore;

        public MechanicHandoversController(IMechanicHandoverDataStore mechanicHandoverDataStore)
        {
            _mechanicHandoverDataStore = mechanicHandoverDataStore;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {

            var response = await _mechanicHandoverDataStore.GetMechanicHandoversAsync(pageNumber);

            ViewBag.PageSize = response.PageSize;
            ViewBag.PageCount = response.TotalPages;
            ViewBag.TotalCount = response.TotalCount;
            ViewBag.CurrentPage = response.PageNumber;
            ViewBag.HasPreviousPage = response.HasPreviousPage;
            ViewBag.HasNextPage = response.HasNextPage;

            var mechanicHandovers = response.Data.Select(r => new
            {
                r.Id,
                IsHanded = r.IsHanded ? "Qabul qilindi" : "Rad etildi",
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

            if (mechanicHandovers == null)
            {
                return BadRequest();
            }

            ViewBag.MechanicHandovers = mechanicHandovers;

            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var mechanicHandover = await _mechanicHandoverDataStore.GetMechanicHandoverAsync(id);
            if (mechanicHandover == null)
            {
                return NotFound();
            }
            return View(mechanicHandover);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IsHanded,Comments,Status,Date,MechanicId,CarId,DriverId")] MechanicHandover mechanicHandover)
        {
            if (ModelState.IsValid)
            {
                await _mechanicHandoverDataStore.CreateMechanicHandoverAsync(mechanicHandover);
                return RedirectToAction(nameof(Index));
            }
            return View(mechanicHandover);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var mechanicHandover = await _mechanicHandoverDataStore.GetMechanicHandoverAsync(id);
            if (mechanicHandover == null)
            {
                return NotFound();
            }
            return View(mechanicHandover);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IsHanded,Comments,Status,Date,MechanicId,CarId,DriverId")] MechanicHandover mechanicHandover)
        {
            if (id != mechanicHandover.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _mechanicHandoverDataStore.UpdateMechanicHandoverAsync(id, mechanicHandover);
                }
                catch (Exception)
                {
                    if (!await MechanicHandoverExists(id))
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
            return View(mechanicHandover);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var mechanicHandover = await _mechanicHandoverDataStore.GetMechanicHandoverAsync(id);
            if (mechanicHandover == null)
            {
                return NotFound();
            }
            return View(mechanicHandover);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mechanicHandoverDataStore.DeleteMechanicHandoverAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> MechanicHandoverExists(int id)
        {
            var mechanicHandover = await _mechanicHandoverDataStore.GetMechanicHandoverAsync(id);
            return mechanicHandover != null;
        }
    }
}
