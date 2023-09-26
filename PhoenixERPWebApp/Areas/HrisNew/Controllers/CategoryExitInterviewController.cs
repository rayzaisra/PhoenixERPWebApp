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
    public class CategoryExitInterviewController : Controller
    {
        string url = ApiUrl.CategoryExitInterviewUrl;
        private readonly ApiClientFactory client;

        public CategoryExitInterviewController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new CategoryExitInterview();

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<CategoryExitInterview>($"{url}/{Id}");
            }
            else
            {
                ViewBag.Header = "Create";
                model.No = await client.Get<int>($"{url}/getAutoNumber");
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<CategoryExitInterview>>(url) ?? new List<CategoryExitInterview>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(CategoryExitInterview model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<CategoryExitInterview>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<CategoryExitInterview>(url, model);
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(CategoryExitInterview model) => Json(await client.DeleteApiResponse<CategoryExitInterview>($"{url}/{model.Id}"));
    }
}