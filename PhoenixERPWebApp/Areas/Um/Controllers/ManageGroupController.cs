using Microsoft.AspNetCore.Mvc;
using Phoenix.WebExtension.Attributes;

namespace PhoenixERPWebApp.Areas.Um.Controllers
{
    [Area(nameof(Um))]
    public class ManageGroupController : Controller
    {
        private string _uniqueName = "SETTINGGROUP";
        public IActionResult Index()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }
    }
}