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

namespace PhoenixERPWebApp.Areas.HrisNew.Controllers
{
    [Area(nameof(HrisNew))]
    public class TmTerminationTypeController : Controller
    {
        string url = ApiUrl.TmTerminationTypeUrl;
        private readonly ApiClientFactory client;

        public TmTerminationTypeController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new TmTerminationType();
            ViewBag.Header = "Create";
            ViewBag.isEdit = !string.IsNullOrEmpty(Id);
            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<TmTerminationType>($"{url}/{Id}");
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<TmTerminationType>>(url) ?? new List<TmTerminationType>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(TmTerminationType model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            var data = await client.Get<TmTerminationType>($"{url}/{model.Id}");
            if (data.Id != null)
            {
                response = await client.PutApiResponse<TmTerminationType>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<TmTerminationType>(url, model);
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(TmTerminationType model) => Json(await client.DeleteApiResponse<TmTerminationType>($"{url}/{model.Id}"));
    }
}