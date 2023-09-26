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
using Microsoft.AspNetCore.Authorization;

namespace PhoenixERPWebApp.Areas.Core.Controllers
{
    [AllowAnonymous]
    [Area("Core")]
    public class HomeController : BaseController
    {
        private AppSettingsOption appSetting;
        public HomeController(IOptions<AppSettingsOption> AppSetting)
        {
            appSetting = AppSetting.Value;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            var conf = appSetting.ApiServerUrl;
            return View();
        }

    }
}