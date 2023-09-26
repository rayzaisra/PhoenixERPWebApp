using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CodeMarvel.Infrastructure;
using CodeMarvel.Infrastructure.Sessions;
using CodeMarvel.Infrastructure.WebNetCore;
using Microsoft.Extensions.Configuration;
using CodeMarvel.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class CoaBalanceController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail([FromQuery(Name = "id")] string Id)
        {
            ViewBag.RecordID = Id;
            ViewBag.EntityID = "1a60d708-94e2-80e8-eabe-3e74b9145849";
            return View();
        }

    }
}