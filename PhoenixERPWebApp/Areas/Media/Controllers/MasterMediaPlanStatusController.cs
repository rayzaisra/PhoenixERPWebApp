using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.WebNetCore;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using Phoenix.WebExtension.RestApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PhoenixERPWebApp.Models.Media.MasterData;

namespace PhoenixERPWebApp.Areas.Media.Controllers
{
    [Area(nameof(Media))]
    public class MasterMediaPlanStatusController : Controller
    {
        string url = ApiUrl.MediaPlanStatus;
        private readonly ApiClientFactory client;

        public MasterMediaPlanStatusController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<MediaPlanStatus>>(url) ?? new List<MediaPlanStatus>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new MediaPlanStatus();
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<MediaPlanStatus>($"{url}/{Id}");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(MediaPlanStatus model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<MediaPlanStatus>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<MediaPlanStatus>(url, model);
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(MediaPlanStatus model) => Json(await client.DeleteApiResponse<MediaPlanStatus>($"{url}/{model.Id}"));
    }
}