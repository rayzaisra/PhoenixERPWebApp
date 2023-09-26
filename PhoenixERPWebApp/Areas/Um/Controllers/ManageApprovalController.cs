using Microsoft.AspNetCore.Mvc;

namespace PhoenixERPWebApp.Areas.Um.Controllers
{
    [Area(nameof(Um))]
    public class ManageApprovalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AdditionalTemplate()
        {
            return View();
        }

        public IActionResult AddConditional()
        {
            return View();
        }
        public IActionResult DetailConditional()
        {
            return View();
        }
    }
}