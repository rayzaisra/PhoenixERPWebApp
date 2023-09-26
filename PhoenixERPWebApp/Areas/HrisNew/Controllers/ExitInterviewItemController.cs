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
    public class ExitInterviewItemController : Controller
    {
        string url = ApiUrl.ExitInterviewItemUrl;
        private readonly ApiClientFactory client;

        public ExitInterviewItemController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new ExitInterviewItem();
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<ExitInterviewItem>($"{url}/{Id}");
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<ExitInterviewItem>>(url) ?? new List<ExitInterviewItem>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(ExitInterviewItem model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<ExitInterviewItem>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<ExitInterviewItem>(url, model);
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(ExitInterviewItem model) => Json(await client.DeleteApiResponse<ExitInterviewItem>($"{url}/{model.Id}"));

        public async Task<ActionResult> GetAutoNo(string categoryExitInterviewId)
        {
            var no = await client.Get<int>($"{url}/getAutoNumber/{categoryExitInterviewId}");
            return Json(no);
        }
    }
}