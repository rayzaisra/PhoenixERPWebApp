using Microsoft.AspNetCore.Mvc;
using Phoenix.WebExtension.Attributes;

namespace PhoenixERPWebApp.Areas.Um.Controllers
{
    [Area(nameof(Um))]
    public class ManageGroupAccessController : Controller
    {
        private string _uniqueName = "SETTINGGROUPACCSESS";
        public IActionResult Index()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }
        public IActionResult GaList()
        {
            return View();
        }
    }

}