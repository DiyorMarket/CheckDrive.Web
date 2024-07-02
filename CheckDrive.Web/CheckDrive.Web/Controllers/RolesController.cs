using CheckDrive.Web.Stores.Roles;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRoleDataStore _roleDataStore;

        public RolesController(IRoleDataStore roleDataStore)
        {
            _roleDataStore = roleDataStore;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleDataStore.GetRoles();
            return View(roles);
        }

        public async Task<IActionResult> Details(int id)
        {
            var role = await _roleDataStore.GetRole(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] ApiContracts.Role.RoleForCreateDto role)
        {
            if (ModelState.IsValid)
            {
                await _roleDataStore.CreateRole(role);
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var role = await _roleDataStore.GetRole(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ApiContracts.Role.RoleForCreateDto role)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    await _roleDataStore.UpdateRole(id, role);
                }
                catch (Exception)
                {
                    if (!await RoleExists(id))
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
            return View(role);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var role = await _roleDataStore.GetRole(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _roleDataStore.DeleteRole(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> RoleExists(int id)
        {
            var role = await _roleDataStore.GetRole(id);
            return role != null;
        }
    }
}
