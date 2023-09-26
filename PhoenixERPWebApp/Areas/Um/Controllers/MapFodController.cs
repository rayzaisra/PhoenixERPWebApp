using Microsoft.AspNetCore.Mvc;

namespace PhoenixERPWebApp.Areas.Um.Controllers
{
    [Area(nameof(Um))]
    public class MapFodController : Controller
    {
        private string _uniqueName = "MAPFOD";
        public IActionResult Index()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }
        public IActionResult MappingDrive()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }

        public IActionResult SampleUploadAndPreviewImage()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }
    }

    [Area(nameof(Um))]
    public class UserProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Edited()
        {
            return View();
        }
    }

}