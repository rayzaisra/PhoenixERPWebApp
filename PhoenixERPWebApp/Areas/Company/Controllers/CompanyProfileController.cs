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

namespace PhoenixERPWebApp.Areas.Company.Controllers
{
    [Area("Company")]
    public class CompanyProfileController : BaseController
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Detail([FromQuery(Name = "id")] String Id,
            [FromQuery(Name="is_client")] bool IsClient =false, [FromQuery(Name = "is_vendor")] bool IsVendor = false,
            [FromQuery(Name = "back")] String backUrl="~")
        {
            ViewBag.RecordID = Id;
            ViewBag.IsClient = IsClient;
            ViewBag.IsVendor = IsVendor;
            ViewBag.BackUrl = backUrl;
            return View();
        }

    }
}