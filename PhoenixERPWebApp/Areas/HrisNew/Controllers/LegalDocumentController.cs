using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.ModelShared;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;

namespace PhoenixERPWebApp.Areas.HrisNew.Controllers
{
    [Area(nameof(HrisNew))]
    public class LegalDocumentController : Controller
    {
        string url = ApiUrl.LegalDocumentUrl;
        private readonly ApiClientFactory client;

        public LegalDocumentController(ApiClientFactory client)
        {
            this.client = client;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.UserAcess = await client.Get<bool>($"{url}/UserAcessLegalDoc");
            return View(new LegalDocumentSearch());
        }

        public IActionResult Dashboard()
        {
            return View(new DashboardSearch());
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new LegalDocument() { Category = CategoryLegalDoc.Employee };
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<LegalDocument>($"{url}/{Id}");
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, LegalDocumentSearch search)
        {
            var model = await client.PostApiResponse<List<LegalDocument>>($"{url}/getDocumentList", search);
            if (model.Data == null) model.Data = new List<LegalDocument>();
            var data = (model.Data as List<LegalDocument>).ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadDashboard([DataSourceRequest] DataSourceRequest request, DashboardSearch search)
        {
            var model = await client.PostApiResponse<List<LegalDocument>>($"{url}/getDashboardList", search);
            if (model.Data == null) model.Data = new List<LegalDocument>();
            var data = ((model.Data) as List<LegalDocument>).ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(LegalDocument model, IFormFile file)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<LegalDocument>(url, model, file);
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
            else
            {
                response = await client.PostApiResponse<LegalDocument>(url, model, file);
                return Json(new { success = response.Success, message = response.Message, url = Url.Action("Index", new { controller = "DocumentManagement", area = "HrisNew" }) });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(LegalDocument model) => Json(await client.DeleteApiResponse<LegalDocument>($"{url}/{model.Id}"));
    }
}