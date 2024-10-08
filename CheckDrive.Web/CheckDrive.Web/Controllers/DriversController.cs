﻿using CheckDrive.ApiContracts.Account;
using CheckDrive.ApiContracts.Driver;
using CheckDrive.Web.Stores.Drivers;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class DriversController(IDriverDataStore driverDataStore) : Controller
    {
        private readonly IDriverDataStore _driverDataStore = driverDataStore;

        public async Task<IActionResult> Index()
        {
            var drivers = await _driverDataStore.GetDriversAsync(2, null);

            ViewBag.Drivers = drivers;
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var driver = await _driverDataStore.GetDriverAsync(id);
            if (driver == null)
            {
                return NotFound();
            }
            return View(driver);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId")] DriverForCreateDto driver)
        {
            if (ModelState.IsValid)
            {
                await _driverDataStore.CreateDriverAsync(driver);
                return RedirectToAction(nameof(Index));
            }
            return View(driver);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var driver = await _driverDataStore.GetDriverAsync(id);
            if (driver == null)
            {
                return NotFound();
            }
            return View(driver);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountId")] AccountForUpdateDto driver)
        {
            if (id != driver.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _driverDataStore.UpdateDriverAsync(id, driver);
                }
                catch (Exception)
                {
                    if (!await DriverExists(id))
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
            return View(driver);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var driver = await _driverDataStore.GetDriverAsync(id);
            if (driver == null)
            {
                return NotFound();
            }
            return View(driver);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _driverDataStore.DeleteDriverAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> DriverExists(int id)
        {
            var driver = await _driverDataStore.GetDriverAsync(id);
            return driver != null;
        }
    }
}
