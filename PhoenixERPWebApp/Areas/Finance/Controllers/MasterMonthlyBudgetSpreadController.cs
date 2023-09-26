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
    public class MasterMonthlyBudgetSpreadController : Controller
    {
        string url = ApiUrl.MasterMonthlyBudgetSpreadUrl;
        private readonly ApiClientFactory client;

        public MasterMonthlyBudgetSpreadController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new MasterMonthlyBudgetSpread();
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<MasterMonthlyBudgetSpread>($"{url}/{Id}");
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<MasterMonthlyBudgetSpread>>(url) ?? new List<MasterMonthlyBudgetSpread>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(MasterMonthlyBudgetSpread model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<MasterMonthlyBudgetSpread>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<MasterMonthlyBudgetSpread>(url, model);
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(MasterMonthlyBudgetSpread model) => Json(await client.DeleteApiResponse<MasterMonthlyBudgetSpread>($"{url}/{model.Id}"));


        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            var model = await client.Get<MasterMonthlyBudgetSpread>($"{url}/{id}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}