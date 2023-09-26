using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.WebNetCore;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models.Media;

namespace PhoenixERPWebApp.Areas.Media.Controllers
{
    [Area(nameof(Media))]
    public class MasterMediaPlanRequestTypeController : BaseController
    {
        string url = ApiUrl.MediaPlanRequestType;
        private readonly ApiClientFactory client;

        public MasterMediaPlanRequestTypeController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(string Id)
        {
            var model = new MediaPlanRequestTypeDetail();
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<MediaPlanRequestTypeDetail>($"{url}/{Id}");
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<MediaPlanRequestTypeList>>(url) ?? new List<MediaPlanRequestTypeList>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> Save(MediaPlanRequestTypeDetail model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<MediaPlanRequestTypeDetail>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<MediaPlanRequestTypeDetail>(url, model);
            }

            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(MediaPlanRequestTypeDetail model) => Json(await client.DeleteApiResponse<MediaPlanRequestTypeDetail>($"{url}/{model.Id}"));
    }
}