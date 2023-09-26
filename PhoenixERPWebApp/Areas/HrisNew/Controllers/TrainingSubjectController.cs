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

namespace PhoenixERPWebApp.Areas.Hrisnew.Controllers
{
    [Area(nameof(Hrisnew))]
    public class TrainingSubjectController : Controller
    {
        string url = ApiUrl.TrainingSubjectUrl;
        private readonly ApiClientFactory client;

        public TrainingSubjectController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new TrainingSubject();
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<TrainingSubject>($"{url}/{Id}");
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<TrainingSubject>>(url) ?? new List<TrainingSubject>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(TrainingSubject model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<TrainingSubject>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<TrainingSubject>(url, model);
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(TrainingSubject model) => Json(await client.DeleteApiResponse<TrainingSubject>($"{url}/{model.Id}"));
    }
}