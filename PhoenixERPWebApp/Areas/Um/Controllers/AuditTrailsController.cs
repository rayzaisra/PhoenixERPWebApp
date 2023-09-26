using Microsoft.AspNetCore.Mvc;

namespace PhoenixERPWebApp.Areas.Um.Controllers
{
    [Area(nameof(Um))]
    public class AuditTrailsController : Controller
    {
        private string _uniqueName = "AUDTTRAILS";
        public IActionResult Index()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }
    }
}