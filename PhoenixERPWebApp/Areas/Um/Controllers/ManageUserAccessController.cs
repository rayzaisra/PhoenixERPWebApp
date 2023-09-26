using Microsoft.AspNetCore.Mvc;
using Phoenix.WebExtension.Attributes;

namespace PhoenixERPWebApp.Areas.Um.Controllers
{
    [Area(nameof(Um))]
    public class ManageUserAccessController : Controller
    {
        private string _uniqueName = "SETTINGUSERACCESS";
        public IActionResult Index()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }
        public IActionResult EditUserAccess()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }

    }

}