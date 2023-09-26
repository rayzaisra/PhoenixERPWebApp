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
using Phoenix.WebExtension.Attributes;

namespace PhoenixERPWebApp.Areas.Company.Controllers
{
    [Area("Company")]
    //[MenuArea("590541e3-a932-4d29-b972-ddccb8efc1d7")]
    public class JobCardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(string Id)
        {
            ViewBag.RecordID = Id;
            return View();
        }
    }
}