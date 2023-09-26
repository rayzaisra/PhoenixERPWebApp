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

namespace PhoenixERPWebApp.Areas.HrisNew.Controllers
{

    [Area(nameof(Hrisnew))]
    public class TrainingShareSessionController : Controller
    {
        string url = ApiUrl.TrainingShareSessionUrl;
    
        private readonly ApiClientFactory client;
        public TrainingShareSessionController(ApiClientFactory client)
        {
            this.client = client;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ReadTrainingShareSession([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<TrainingShareSession>>($"{url}/TrainingList") ?? new List<TrainingShareSession>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new TrainingShareSession();
            ViewBag.Header = "Form Create Training Share Session";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<TrainingShareSession>($"{url}/{Id}");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(TrainingShareSession model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                model.EmployeeBasicInfoId = model.TrainerEmployeeBasicInfoId;
                response = await client.PutApiResponse<TrainingShareSession>(url, model);
            }
            else
            {
                model.EmployeeBasicInfoId = model.TrainerEmployeeBasicInfoId;
                response = await client.PostApiResponse<TrainingShareSession>(url, model);
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpGet]
        public async Task<IActionResult> GetTrainingCompletedById(string id)
        {
            var model = new TrainingShareSession();
            model = await client.Get<TrainingShareSession>($"{url}/gettrainingcompleted/{id}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}