using Microsoft.AspNetCore.Mvc;
using Phoenix.WebExtension.Attributes;

namespace PhoenixERPWebApp.Areas.Um.Controllers
{
    [Area(nameof(Um))]
    public class ManageMenuController : Controller
    {
        private string _uniqueName = "SETTINGMENU";
        public IActionResult Index()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }
        public IActionResult ManageIndex()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }
    }

}