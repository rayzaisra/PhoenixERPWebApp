using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models.Media.MasterData;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PhoenixERPWebApp.Areas.Media.Controllers
{
    [Area(nameof(Media))]
    public class MasterMediaOrderStatusController : Controller
    {
        string url = ApiUrl.MediaOrderStatus;
        private readonly ApiClientFactory client;

        public MasterMediaOrderStatusController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<MediaOrderStatus>>(url) ?? new List<MediaOrderStatus>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new MediaOrderStatus();
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<MediaOrderStatus>($"{url}/{Id}");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(MediaOrderStatus model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<MediaOrderStatus>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<MediaOrderStatus>(url, model);
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(MediaOrderStatus model) => Json(await client.DeleteApiResponse<MediaOrderStatus>($"{url}/{model.Id}"));
    }
}