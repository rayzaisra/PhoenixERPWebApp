using Microsoft.AspNetCore.Mvc;

namespace PhoenixERPWebApp.Areas.Um.Controllers
{
    [Area(nameof(Um))]
    public class OneDriveOfficeController : Controller
    {
        private string _uniqueName = "ODRIVOFF";
        public IActionResult Index()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }
        public IActionResult ListEmail()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }
        public IActionResult ComposeEmail()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }
    }
}