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
    public class ApprovalTemplateController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail([FromQuery(Name = "id")] string Id)
        {
            ViewBag.RecordID = Id;
            return View();
        }

        public IActionResult Preview([FromQuery(Name = "id")] string Id)
        {
            ViewBag.RecordID = Id;
            return View();
        }



        public IActionResult SetupApproval([FromQuery(Name = "EntityId")] String EntityId, [FromQuery(Name ="RecordID")] String RecordId, 
            [FromQuery(Name = "Title")] String Title, [FromQuery(Name = "Ref")] String Reference, [FromQuery(Name = "url")] String BackUrl)
        {
            ViewBag.EntityID = EntityId;
            ViewBag.RecordID = RecordId;
            ViewBag.ReferenceFor = Title;
            ViewBag.Reference = Reference;
            ViewBag.BackURL = BackUrl;
            return View();
        }


    }
}