using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
    public class LegalDocumentRequestController : Controller
    {
        string url = ApiUrl.LegalDocumentRequestUrl;
        private readonly ApiClientFactory client;

        public LegalDocumentRequestController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new LegalDocumentRequest();
            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<LegalDocumentRequest>($"{url}/{Id}");
            }
            else
            {
                ViewBag.Header = "Create";
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<LegalDocumentRequest>>(url) ?? new List<LegalDocumentRequest>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(LegalDocumentRequest model)
        {
            model.Status = StatusTransaction.WaitingApproval;
            model.SubmitUser = true;
            return await Process(model, false);
        }

        [HttpPost]
        public async Task<IActionResult> Save(LegalDocumentRequest model)
        {
            model.Status = StatusTransaction.Draft;
            return await Process(model, true);
        }

        private async Task<IActionResult> Process(LegalDocumentRequest model, bool isDraft)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<LegalDocumentRequest>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<LegalDocumentRequest>(url, model);
            }

            if (isDraft)
            {
                return Json(new { success = response.Success, message = response.Message, data = response.Data as LegalDocumentRequest });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(LegalDocumentRequest model) => Json(await client.DeleteApiResponse<LegalDocumentRequest>($"{url}/{model.Id}"));
    }
}