using Microsoft.AspNetCore.Mvc;

namespace PhoenixERPWebApp.Areas.HrisNew.Controllers
{
    [Area(nameof(HrisNew))]
    public class DelegateOfAuthorityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RequestDoa()
        {
            return View();
        }
        public IActionResult ListNeedApproval()
        {
            return View();
        }
        public IActionResult DetailDoa()
        {
            return View();
        }
    }
}