using System.ComponentModel.DataAnnotations;
using System.Reflection;
using CheckDrive.Web.Mappings;
using CheckDrive.Web.Models.Enums;
using CheckDrive.Web.Requests.Debt;
using CheckDrive.Web.Stores.Debts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckDrive.Web.Controllers;

public class DebtsController : Controller
{
    private readonly IDebtsStore _debtStore;
    public DebtsController(IDebtsStore debtStore)
    {
        _debtStore = debtStore ?? throw new ArgumentNullException(nameof(debtStore));
    }
    public async Task<ActionResult> Index()
    {
        var debts = await _debtStore.GetAsync();

        ViewBag.SelectedStatus = GetDebtStatuses();

        return View(debts);
    }

    [HttpGet]
    public async Task<ActionResult> Edit(int id)
    {
        var debt = await _debtStore.GetByIdAsync(id);

        var updateRequest = debt.ToUpdateViewModel();
        
        return View(updateRequest);
    }

    [HttpPost]
    public async Task<ActionResult> Edit(UpdateDebtRequest request)
    {   
        request.Status = DebtStatus.Paid;

        await _debtStore.UpdateAsync(request);

        return RedirectToAction(nameof(Index));
    }

    private static List<SelectListItem> GetDebtStatuses()
    {
        var excludedStatuses = new[] { DebtStatus.Paid, DebtStatus.Unpaid };
        var statuses = Enum.GetValues(typeof(DebtStatus))
                    .Cast<DebtStatus>()
                    .Where(e => excludedStatuses.Contains(e))
                    .Select(e => new SelectListItem()
                    {
                        Value = e.ToString(),
                        Text = e.GetType().GetField(e.ToString())?.GetCustomAttribute<DisplayAttribute>()?.Name ?? e.ToString()
                    })
                    .ToList();
    
        return statuses;
    }
}
