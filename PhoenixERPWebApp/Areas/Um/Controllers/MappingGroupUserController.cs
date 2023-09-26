using Microsoft.AspNetCore.Mvc;

namespace PhoenixERPWebApp.Areas.Um.Controllers
{
    [Area(nameof(Um))]
    public class MappingGroupUserController : Controller
    {
        private string _uniqueName = "MPGRPUSR";
        public IActionResult Index()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }
        public IActionResult AddUser()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }
    }
}