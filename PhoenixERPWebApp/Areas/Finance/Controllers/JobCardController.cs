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
    public class JobCardController : Controller
    {
        string url = ApiUrl.jobCardUrl;
        private readonly ApiClientFactory client;

        public JobCardController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(string jobId)
        {
            var model = await client.Get<VwJobCard>($"{url}/{jobId}");
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<VwJobCard>>(url) ?? new List<VwJobCard>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadReportJobCard([DataSourceRequest] DataSourceRequest request, string jobId)
        {
            var model = await client.Get<List<VwReportJobCard>>($"{url}/GetReportJobCard/{jobId}") ?? new List<VwReportJobCard>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadReportJobCardCOBAp([DataSourceRequest] DataSourceRequest request, string jobId)
        {
            var model = await client.Get<List<VwReportJobCardCobAp>>($"{url}/GetReportJobCardCOBAp/{jobId}") ?? new List<VwReportJobCardCobAp>();
            var data = model.ToTreeDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadReportJobCardCOBCa([DataSourceRequest] DataSourceRequest request, string jobId)
        {
            var model = await client.Get<List<VwReportJobCardCobCa>>($"{url}/GetReportJobCardCOBCa/{jobId}") ?? new List<VwReportJobCardCobCa>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadReportJobCardCOBAr([DataSourceRequest] DataSourceRequest request, string jobId)
        {
            var model = await client.Get<List<VwReportJobCardCobAr>>($"{url}/GetReportJobCardCOBAr/{jobId}") ?? new List<VwReportJobCardCobAr>();
            var data = model.ToTreeDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadReportJobCardCOBBr([DataSourceRequest] DataSourceRequest request, string jobId)
        {
            var model = await client.Get<List<VwReportJobCardCobBr>>($"{url}/GetReportJobCardCOBBr/{jobId}") ?? new List<VwReportJobCardCobBr>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadReportJobCardCOBBp([DataSourceRequest] DataSourceRequest request, string jobId)
        {
            var model = await client.Get<List<VwReportJobCardCobBp>>($"{url}/GetReportJobCardCOBBp/{jobId}") ?? new List<VwReportJobCardCobBp>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadReportJobCardRfqCa([DataSourceRequest] DataSourceRequest request, string jobId)
        {
            var model = await client.Get<List<VwReportJobCardRfqCa>>($"{url}/GetReportJobCardRfqCa/{jobId}") ?? new List<VwReportJobCardRfqCa>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}