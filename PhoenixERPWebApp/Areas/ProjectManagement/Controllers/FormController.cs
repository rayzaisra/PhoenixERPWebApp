using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;

namespace Phoenix.Web.Areas.ProjectManagement.Controllers
{
    [Area("ProjectManagement")]
    public class FormController : Controller
    {
        //private string _uniqueNameJob = "PM.JOB";
        [Route("ProjectManagement")]
        
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
        [Route("ProjectManagement/{src}/{subsrc?}/{id?}")]
       
        public IActionResult Page(string src, string subsrc = "index", string id = "")
        {
            bool isAjax = Request.Headers["x-requested-with"] == "XMLHttpRequest";
            ViewBag.RecordID = id;
            //ViewBag.PageUniqueNameJob = _uniqueNameJob;
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
