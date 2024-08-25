using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.Car;
using CheckDrive.Web.Stores.Cars;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarDataStore _carDataStore;

        public CarsController(ICarDataStore carDataStore)
        {
            _carDataStore = carDataStore;
        }

        public async Task<IActionResult> Index(string? searchString, int? pageNumber)
        {
            var cars = await _carDataStore.GetCarsAsync(searchString, pageNumber);

            ViewBag.SearchString = searchString;

            ViewBag.PageSize = cars.PageSize;
            ViewBag.PageCount = cars.TotalPages;
            ViewBag.TotalCount = cars.TotalCount;
            ViewBag.CurrentPage = cars.PageNumber;
            ViewBag.HasPreviousPage = cars.HasPreviousPage;
            ViewBag.HasNextPage = cars.HasNextPage;

            var _cars = cars.Data.Select(c => new
            {
                c.Id,
                c.Model,
                c.Number,
                c.Mileage,
                c.Color,
                c.RemainingFuel,
                c.MeduimFuelConsumption,
                c.FuelTankCapacity,
                c.ManufacturedYear,
                Status = ((CarStatusDto)c.CarStatus) switch
                {
                    CarStatusDto.Free => "Bo'sh",
                    CarStatusDto.Busy => "Band",
                    CarStatusDto.Limited => "Limit tugagan",
                    _ => "No`malum holat"
                }
            }).ToList();

            ViewBag.Cars = _cars;
            return View();
        }

        public async Task<IActionResult> CarHistoryIndex(string? searchString, int? pageNumber, int? year, int? month)
        {
            var cars = await _carDataStore.GetCarsHistoryAsync(searchString, pageNumber, year, month);

            ViewBag.SearchString = searchString;
            ViewBag.Cars = cars.Data;

            ViewBag.PageSize = cars.PageSize;
            ViewBag.PageCount = cars.TotalPages;
            ViewBag.TotalCount = cars.TotalCount;
            ViewBag.CurrentPage = cars.PageNumber;
            ViewBag.HasPreviousPage = cars.HasPreviousPage;
            ViewBag.HasNextPage = cars.HasNextPage;
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var car = await _carDataStore.GetCarAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        public async Task<IActionResult> DetailsForMechanicAcceptance(int id)
        {
            var car = await _carDataStore.GetCarAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return PartialView("_CarDetailsForMechanicAcceptance", car);
        }

        public async Task<IActionResult> DetailsForMechanicHandover(int id)
        {
            var car = await _carDataStore.GetCarAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return PartialView("_CarDetailsForMechanicHandover", car);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Model,Color,Number,RemainingFuel,Mileage, MeduimFuelConsumption,FuelTankCapacity,ManufacturedYear, OneYearMediumDistance")] CarForCreateDto car)
        {
            if (ModelState.IsValid)
            {
                var newCar = await _carDataStore.CreateCarAsync(car);
                return RedirectToAction("Details", new { id = newCar.Id });
            }
            return View(car);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var car = await _carDataStore.GetCarAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Model,Color,Number,RemainingFuel,Mileage, MeduimFuelConsumption,FuelTankCapacity,ManufacturedYear, OneYearMediumDistance")] CarForUpdateDto car)
        {
            if (ModelState.IsValid)
            {
                var newCar = await _carDataStore.UpdateCarAsync(id, car);
                return RedirectToAction("Details", new { id = newCar.Id });
            }
            return View(car);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var car = await _carDataStore.GetCarAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _carDataStore.DeleteCarAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
