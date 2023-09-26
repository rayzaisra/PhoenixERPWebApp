using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Mvc;
using Microsoft.Extensions.DependencyInjection;
using CodeMarvel.Infrastructure.WebNetCore;
using CodeMarvel.Infrastructure.Options;
using Microsoft.Extensions.Options;
using PhoenixERPWebApp.Areas.PhoenixReport.Helper;
using System.Web;

namespace PhoenixERPWebApp.Areas.PhoenixReport.Controllers
{
    [Area("PhoenixReport")]
    public class FinanceController : BaseController
    {
        public IActionResult GeneralLedger()
        {
            return View();
        }

        public IActionResult BalanceSheet()
        {
            return View();
        }

       
    }
}