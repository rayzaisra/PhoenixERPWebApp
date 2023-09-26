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

namespace PhoenixERPWebApp.Areas.Core.Controllers
{
    [Area("Core")]
    public class BusinessUnitController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail([FromQuery(Name = "id")] String Id, [FromQuery(Name = "parentid")] string ParentId, [FromQuery(Name = "parentname")] string ParentName)
        {
            ViewBag.RecordID = Id;
            ViewBag.ParentId = ParentId;
            ViewBag.ParentName = ParentName;
            return View();
        }

        public IActionResult Lookup()
        {
            return View();
        }


    }
}