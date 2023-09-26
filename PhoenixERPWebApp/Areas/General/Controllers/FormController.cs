using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using Phoenix.WebExtension.Attributes;

namespace Phoenix.Web.Areas.General.Controllers
{
    [Area("General")]
    public class FormController : Controller
    {
        [Route("General")]
        public IActionResult Index()
        {
            bool isAjax = Request.Headers["x-requested-with"] == "XMLHttpRequest";
            if (!isAjax)
            {
                return View("index");
            }
            else
            {

                return PartialView("index");
            }
        }
        [Route("General/file")]
        public IActionResult File()
        {
            bool isAjax = Request.Headers["x-requested-with"] == "XMLHttpRequest";
            if (!isAjax)
            {
                return View("file");
            }
            else
            {

                return PartialView("file");
            }
        }
        [Route("General/fileupload")]
        public IActionResult UploadFile()
        {
            bool isAjax = Request.Headers["x-requested-with"] == "XMLHttpRequest";
            if (!isAjax)
            {
                return View("FileUpload");
            }
            else
            {

                return PartialView("FileUpload");
            }
        }
        [Route("General/treebu")]
        public IActionResult treebu()
        {
            bool isAjax = Request.Headers["x-requested-with"] == "XMLHttpRequest";
            if (!isAjax)
            {
                return View("treebu");
            }
            else
            {

                return PartialView("treebu");
            }
        }
        [Route("General/{src}/{subsrc?}/{id?}")]
        public IActionResult Page(string src, string subsrc = "index", string id = "")
        {
            bool isAjax = Request.Headers["x-requested-with"] == "XMLHttpRequest";
            ViewBag.RecordID = id;
            if (!isAjax)
            {
                return View(src + "/" + subsrc);
            }
            else
            {

                return PartialView(src + "/" + subsrc);
            }
        }
    }
}
