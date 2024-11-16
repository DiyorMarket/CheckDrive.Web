using CheckDrive.Web.Stores.Debts;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class DebtsController : Controller
    {
        private readonly IDebtsStore _store;
        public DebtsController(IDebtsStore debtsStore)
        {
            _store = debtsStore;
        }
        public ActionResult Index(string? searchText, string? status)
        {
            var debts = _store.GetAll(searchText,status);

            ViewBag.Status = _store.GetEnum();
            ViewBag.SearchText = searchText;
            ViewBag.SelectedStatus = status;
            return View(debts);
        }
    }
}
