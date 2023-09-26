using Microsoft.AspNetCore.Mvc;

namespace PhoenixERPWebApp.Areas.Um.Controllers
{
    [Area(nameof(Um))]
    public class GroupChattingController : Controller
    {
        private string _uniqueName = "MGRPCHAT";
        public IActionResult Index()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }

        [Route("Um/GroupChatting/Add")]
        [Route("Um/GroupChatting/Add/{id}")]
        public IActionResult Add(string id = "0")
        {
            ViewBag.Codex = id;
            return View();
        }
    }
}