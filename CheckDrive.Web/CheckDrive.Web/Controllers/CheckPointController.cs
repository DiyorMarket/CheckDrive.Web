using CheckDrive.Web.Stores.CheckPoint;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class CheckPointController : Controller
    {
        private readonly ICheckPointStore _checkPointStore;
        public CheckPointController(ICheckPointStore checkPointStore)
        {
            _checkPointStore = checkPointStore ?? throw new ArgumentNullException(nameof(checkPointStore));
        }
        public ActionResult Index(int id)
        {
            var checkPoint = _checkPointStore.GetCheckPointById(id);

            return View(checkPoint);
        }
    }
}
