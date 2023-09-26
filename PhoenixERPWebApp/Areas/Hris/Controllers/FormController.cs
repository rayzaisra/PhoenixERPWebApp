using Microsoft.AspNetCore.Mvc;
using System;

using Microsoft.AspNetCore.Authorization;
using PhoenixERPWebApp.Models;

namespace Phoenix.Web.Areas.Hris.Controllers
{
    [Area("Hris")]
    public class FormController : Controller
    {
        
        [Route("Hris")]
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
        
        [Route("Hris/{src}/{subsrc?}/{id?}/{idsec?}/{idthd?}")]
        public IActionResult Page(string src, string subsrc = "index", string id = "",string idsec = "", string idthd = "")
        {
            bool isAjax = Request.Headers["x-requested-with"] == "XMLHttpRequest";
            ViewBag.RecordID = id;
            ViewBag.RecordIDSec = idsec;
            ViewBag.RecordIDThd = idthd;
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
