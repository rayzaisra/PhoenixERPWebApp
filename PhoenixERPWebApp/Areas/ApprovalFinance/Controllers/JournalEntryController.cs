using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.WebNetCore;
using Microsoft.AspNetCore.Mvc;

namespace PhoenixERPWebApp.Areas.ApprovalFinance.Controllers
{
    [Area("ApprovalFinance")]
    public class JournalEntryController : BaseController
    {
        const String EntityId = "1a60d708-94e2-80e8-eabe-3e74b9145849";
        public IActionResult Index()
        {
            ViewBag.EntityId = EntityId;
            return View();
        }
    }
}