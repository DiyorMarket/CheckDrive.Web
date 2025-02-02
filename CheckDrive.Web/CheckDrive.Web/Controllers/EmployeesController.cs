using CheckDrive.Web.Mappings;
using CheckDrive.Web.Models.Enums;
using CheckDrive.Web.Requests.Employee;
using CheckDrive.Web.Stores.Cars;
using CheckDrive.Web.Stores.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CheckDrive.Web.Controllers;

public class EmployeesController(IEmployeeStore employeeStore, ICarStore carStore) : Controller
{
    public async Task<ActionResult> Index()
    {
        var employees = await employeeStore.GetAsync();
        ViewBag.Positions = GetPositions();

        return View(employees);
    }

    public async Task<ActionResult> Details(int id)
    {
        var employee = await employeeStore.GetByIdAsync(id);

        return View(employee);
    }

    public async Task<ActionResult> Create()
    {
        ViewBag.Positions = GetPositions();
        ViewBag.Cars = await GetCarsAsync();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CreateEmployeeRequest request)
    {
        try
        {
            var createdEmployee = await employeeStore.CreateAsync(request);
            
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    public async Task<ActionResult> Edit(int id)
    {
        var employee = await employeeStore.GetByIdAsync(id);
        ViewBag.Positions = GetPositions();
        ViewBag.Cars = await GetCarsAsync();

        var updateRequest = employee.ToUpdateViewModel();

        return View(updateRequest);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(UpdateEmployeeRequest request)
    {
        try
        {
            await employeeStore.UpdateAsync(request);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    public async Task<ActionResult> Delete(int id)
    {
        var employee = await employeeStore.GetByIdAsync(id);

        return View(employee);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Delete(int id, IFormCollection _)
    {
        try
        {
            await employeeStore.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    private static List<SelectListItem> GetPositions()
    {
        var excludedPositions = new[] { EmployeePosition.Manager, EmployeePosition.Custom };
        var positions = Enum.GetValues(typeof(EmployeePosition))
                    .Cast<EmployeePosition>()
                    .Where(e => !excludedPositions.Contains(e))
                    .Select(e => new SelectListItem()
                    {
                        Value = e.ToString(),
                        Text = e.GetType().GetField(e.ToString())?.GetCustomAttribute<DisplayAttribute>()?.Name ?? e.ToString()
                    })
                    .ToList();

        return positions;
    }

    private async Task<List<SelectListItem>> GetCarsAsync()
    {
        var cars = await carStore.GetAsync();

        return cars
            .Select(e => new SelectListItem()
            {
                Value = e.Id.ToString(),
                Text = e.ToString()
            })
            .ToList();
    }
}
