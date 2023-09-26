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
    public class TeamMemberController : BaseController
    {
        //TEST
        public IActionResult AssignTeamMember([FromQuery(Name = "teamid")] string TeamId)
        {
            ViewBag.TeamID = TeamId;
            return View();
        }

        public IActionResult AddTeamMember([FromQuery(Name = "teamid")] string TeamId)
        {
            ViewBag.TeamID = TeamId;
            return View();
        }

        public IActionResult Detail([FromQuery(Name = "id")] string Id)
        {
            ViewBag.RecordID = Id;
            return View();
        }
    }
}