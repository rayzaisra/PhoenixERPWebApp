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
    public class PdrSalaryIncreaseController : Controller
    {
        string url = ApiUrl.PdrSalaryIncreaseUrl;
        private readonly ApiClientFactory client;

        public PdrSalaryIncreaseController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new PdrSalaryIncrease();
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<PdrSalaryIncrease>($"{url}/{Id}");
           
            }
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<PdrSalaryIncrease>>(url) ?? new List<PdrSalaryIncrease>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(PdrSalaryIncrease model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            //model.SalaryIncreaseString = model.SalaryIncreaseString.Replace(".", ",");
            //model.SalaryIncrease = (decimal)float.Parse(model.SalaryIncreaseString);
           
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<PdrSalaryIncrease>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<PdrSalaryIncrease>(url, model);
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(PdrSalaryIncrease model) => Json(await client.DeleteApiResponse<PdrSalaryIncrease>($"{url}/{model.Id}"));
    }
}