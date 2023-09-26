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
using PhoenixERPWebApp.Models.Pm.ViewModel;

namespace PhoenixERPWebApp.Areas.Pm.Controllers
{
    [Area(nameof(Pm))]
    public class DamApprovalController : Controller
    {
        string url = ApiUrl.DamApproval;
        private readonly ApiClientFactory client;

        public DamApprovalController(ApiClientFactory client)
        {
            this.client = client;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(string Id)
        {
            var model = new DamFileShared();
            ViewBag.Header = "Detail";

            if (!string.IsNullOrEmpty(Id))
            {
                model = await client.Get<DamFileShared>($"{url}/{Id}");
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<DamFileShared>>(url) ?? new List<DamFileShared>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(DamFileShared model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            response = await client.PostApiResponse<DamFileShared>(url, model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(DamFileShared model) => Json(await client.DeleteApiResponse<DamFileShared>($"{url}/{model.Id}"));
    }
}