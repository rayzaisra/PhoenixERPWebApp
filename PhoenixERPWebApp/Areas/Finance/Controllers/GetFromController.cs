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
    public class GetFromController : Controller
    {
        string url = ApiUrl.GetFromUrl;
        private readonly ApiClientFactory client;

        public GetFromController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<GetFrom>>(url) ?? new List<GetFrom>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> Get(string id)
        {
            var model = await client.Get<GetFrom>($"{url}/{id}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new GetFrom();
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<GetFrom>($"{url}/{Id}");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(GetFrom model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<GetFrom>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<GetFrom>(url, model);
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

    }
}