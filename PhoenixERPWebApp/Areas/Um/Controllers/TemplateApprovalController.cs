using Microsoft.AspNetCore.Mvc;
using Phoenix.WebExtension.Attributes;

namespace PhoenixERPWebApp.Areas.Um.Controllers
{
    [Area(nameof(Um))]
    public class TemplateApprovalController : Controller
    {
        private string _uniqueName = "TMPLAPPROV";
        public IActionResult Index()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }
        public IActionResult AdditionalTemplate()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }
        
        public IActionResult AddConditional()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }
        public IActionResult DetailConditional()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }
    }
}