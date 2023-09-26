using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.ModelShared;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area(nameof(Finance))]
    public class FinReportControlController : Controller
    {
        string url = ApiUrl.FinReportControlUrl;
        private readonly ApiClientFactory client;

        public FinReportControlController(ApiClientFactory client)
        {
            this.client = client;
        }

        public async Task<IActionResult> Index()
        {
            var model = await client.Get<VwFinReportControl>(url) ?? new VwFinReportControl();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(VwFinReportControl model)
        {
            var response = await client.PostApiResponse<VwFinReportControl>(url, model);
            return Json(new { success = response.Success, message = response.Message });
        }
    }
}