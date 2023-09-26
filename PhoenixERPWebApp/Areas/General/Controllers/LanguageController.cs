using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phoenix.WebExtension.Attributes;

namespace Phoenix.Web.Areas.General.Controllers
{
    [Area("General")]
    public class LanguageController : Controller
    {
        [Route("General/language/{src}")]
        [HttpGet]
        public JsonResult Change(string src)
        {
            HttpContext.Session.SetString("Language", src);
            var jsonResult = new { success = true };
            return Json(jsonResult);
        }
    }
}
