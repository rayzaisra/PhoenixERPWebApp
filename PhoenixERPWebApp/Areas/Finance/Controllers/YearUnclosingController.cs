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
    public class YearUnclosingController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail([FromQuery(Name = "id")] string Id)
        {
            ViewBag.RecordID = Id;
           // ViewBag.EntityID = "93eeb844-0c4d-9602-e288-7b50f8fa7dae";
            return View();
        }

        public IActionResult CreateEmail([FromQuery(Name = "id")] string Id)
        {
            ViewBag.RecordID = Id;
            ViewBag.EntityID = "93eeb844-0c4d-9602-e288-7b50f8fa7dae";
            return View();
        }
    }
}