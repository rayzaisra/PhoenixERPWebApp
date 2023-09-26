using Microsoft.AspNetCore.Mvc;

namespace PhoenixERPWebApp.Areas.Um.Controllers
{
    [Area(nameof(Um))]
    public class PivotTableReportingController : Controller
    {
        private string _uniqueName = "PVTTBLRPT";
        public IActionResult Index()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }
    }
}