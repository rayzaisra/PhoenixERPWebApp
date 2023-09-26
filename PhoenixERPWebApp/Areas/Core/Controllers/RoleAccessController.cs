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
    public class RoleAccessController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SetupRoleAccess([FromQuery(Name = "roleId")] string AppRoleId)
        {
            ViewBag.AppRoleID = AppRoleId;
            return View();
        }

    }
}