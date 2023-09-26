using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PhoenixERPWebApp.Areas.ApprovalFinance.Controllers
{
    [Area("ApprovalFinance")]
    public class ListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}