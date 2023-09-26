using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class ChartOfAccountLv2Controller : Controller
    {
        string url = $"{ApiUrl.ChartOfAccountUrl}/Level2";
        private readonly ApiClientFactory client;

        public ChartOfAccountLv2Controller(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new ChartofAccount();
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<ChartofAccount>($"{url}/{Id}");
            }
            else
            {
                model.CodeRec = "0.0.00.00.00";
            }
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<ChartofAccount>>(url) ?? new List<ChartofAccount>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(ChartofAccount model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<ChartofAccount>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<ChartofAccount>(url, model);
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(ChartofAccount model) => Json(await client.DeleteApiResponse<ChartofAccount>($"{ApiUrl.ChartOfAccountUrl}/{model.Id}"));
    }
}