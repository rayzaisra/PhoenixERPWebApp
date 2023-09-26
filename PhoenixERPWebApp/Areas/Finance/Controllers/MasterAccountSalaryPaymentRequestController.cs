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
    public class MasterAccountSalaryPaymentRequestController : Controller
    {
        string url = ApiUrl.MapCoaTransaksiUrl;
        private readonly ApiClientFactory client;

        public MasterAccountSalaryPaymentRequestController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<MapCoaTransaksi>>($"{url}/GetJournalSalaryPaymentRequest") ?? new List<MapCoaTransaksi>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public IActionResult Update([DataSourceRequest] DataSourceRequest request, MapCoaTransaksi model)
        {
            if (model != null && ModelState.IsValid) { }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(List<MapCoaTransaksi> model)
        {
            var response = await client.PostApiResponse<MapCoaTransaksi>($"{url}/AddJournalSalaryPaymentRequest", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }
    }
}