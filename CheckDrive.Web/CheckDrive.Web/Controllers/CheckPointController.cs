using CheckDrive.Web.Stores.CheckPoint;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class CheckPointController(ICheckPointStore checkPointStore) : Controller
    {
        public async Task<ActionResult> Index(int id)
        {
            var checkPoint = await checkPointStore.GetByIdAsync(id);

            return View(checkPoint);
        }

        public async Task<ActionResult> Details(int id)
        {
            var checkPoint = await checkPointStore.GetByIdAsync(id);

            return View(checkPoint);
        }
    }
}
