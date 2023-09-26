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
using PhoenixERPWebApp.Models.Pm.MasterData;

namespace PhoenixERPWebApp.Areas.Pm.Controllers
{
    [Area(nameof(Pm))]
    public class MasterJobStatusController : Controller
    {
        string url = ApiUrl.MasterJobStatus;
        private readonly ApiClientFactory client;

        public MasterJobStatusController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new MasterJobStatus();
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<MasterJobStatus>($"{url}/{Id}");
                model.StatusName = model.name;
                model.StatusOrder = model.OwnerId;
                model.Is_Default = model.IsDefault ?? false;
                model.Is_Locked = model.IsLocked ?? false;
                model.is_showing_on_timesheet = model.is_showing_on_timesheet ?? false;
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<MasterJobStatus>>(url) ?? new List<MasterJobStatus>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(MasterJobStatus model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            model.OwnerId = model.StatusOrder;
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<MasterJobStatus>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<MasterJobStatus>(url, model);
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(MasterJobStatus model) => Json(await client.DeleteApiResponse<MasterJobStatus>($"{url}/{model.Id}"));
    }
}