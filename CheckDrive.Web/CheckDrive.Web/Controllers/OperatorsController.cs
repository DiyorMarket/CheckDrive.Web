using CheckDrive.Web.Models;
using CheckDrive.Web.Stores.Operators;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class OperatorsController : Controller
    {
        private readonly IOperatorDataStore _operatorDataStore;

        public OperatorsController(IOperatorDataStore operatorDataStore)
        {
            _operatorDataStore = operatorDataStore;
        }

        public async Task<IActionResult> Index()
        {
            var operators = await _operatorDataStore.GetOperators();
            return View(operators);
        }

        public async Task<IActionResult> Details(int id)
        {
            var @operator = await _operatorDataStore.GetOperator(id);
            if (@operator == null)
            {
                return NotFound();
            }
            return View(@operator);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId")] Operator @operator)
        {
            if (ModelState.IsValid)
            {
                await _operatorDataStore.CreateOperator(@operator);
                return RedirectToAction(nameof(Index));
            }
            return View(@operator);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var @operator = await _operatorDataStore.GetOperator(id);
            if (@operator == null)
            {
                return NotFound();
            }
            return View(@operator);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountId")] Operator @operator)
        {
            if (id != @operator.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _operatorDataStore.UpdateOperator(id, @operator);
                }
                catch (Exception)
                {
                    if (!await OperatorExists(id))
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
            return View(@operator);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var @operator = await _operatorDataStore.GetOperator(id);
            if (@operator == null)
            {
                return NotFound();
            }
            return View(@operator);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _operatorDataStore.DeleteOperator(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> OperatorExists(int id)
        {
            var @operator = await _operatorDataStore.GetOperator(id);
            return @operator != null;
        }
    }
}
