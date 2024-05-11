using CheckDrive.Web.Models;
using CheckDrive.Web.Stores.Dispatchers;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class DispatchersController : Controller
    {
        private readonly IDispatcherDataStore _dispatcherDataStore;

        public DispatchersController(IDispatcherDataStore dispatcherDataStore)
        {
            _dispatcherDataStore = dispatcherDataStore;
        }

        public async Task<IActionResult> Index()
        {
            var dispatchers = await _dispatcherDataStore.GetDispatchers();
            return View(dispatchers);
        }

        public async Task<IActionResult> Details(int id)
        {
            var dispatcher = await _dispatcherDataStore.GetDispatcher(id);
            if (dispatcher == null)
            {
                return NotFound();
            }
            return View(dispatcher);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId")] Dispatcher dispatcher)
        {
            if (ModelState.IsValid)
            {
                await _dispatcherDataStore.CreateDispatcher(dispatcher);
                return RedirectToAction(nameof(Index));
            }
            return View(dispatcher);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dispatcher = await _dispatcherDataStore.GetDispatcher(id);
            if (dispatcher == null)
            {
                return NotFound();
            }
            return View(dispatcher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountId")] Dispatcher dispatcher)
        {
            if (id != dispatcher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _dispatcherDataStore.UpdateDispatcher(id, dispatcher);
                }
                catch (Exception)
                {
                    if (!await DispatcherExists(id))
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
            return View(dispatcher);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var dispatcher = await _dispatcherDataStore.GetDispatcher(id);
            if (dispatcher == null)
            {
                return NotFound();
            }
            return View(dispatcher);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _dispatcherDataStore.DeleteDispatcher(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> DispatcherExists(int id)
        {
            var dispatcher = await _dispatcherDataStore.GetDispatcher(id);
            return dispatcher != null;
        }
    }
}
