using Microsoft.AspNetCore.Mvc;

namespace PhoenixERPWebApp.Areas.Um.Controllers
{
    [Area(nameof(Um))]
    public class ManageAzureAccountController : Controller
    {
        private string _uniqueName = "MNGAZACC";
        public IActionResult Index()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }
        public IActionResult Add(string id = "")
        {
            ViewBag.PageUniqueName = _uniqueName;
            if (string.IsNullOrEmpty(id)) id = "0";
            ViewBag.Codex = id;
            return View();
        }
    }
}