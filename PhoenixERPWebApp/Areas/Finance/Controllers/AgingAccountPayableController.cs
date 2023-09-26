using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.WebNetCore;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.Finance.FinanceReport;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class AgingAccountPayableController : Controller
    {
        string url = ApiUrl.AccountPayableAging;
        private readonly ApiClientFactory client;

        public AgingAccountPayableController(ApiClientFactory client)
        {
            this.client = client;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, VwAgingApFilter Filter)
        {
            var model = new List<VwAgingAp>();
            var response = new ApiResponse();

            if (!string.IsNullOrEmpty(Filter.LegalEntityId)) //harus pilih legal entity terlebih dahulu.
            {
                response = await client.PostApiResponse<List<VwAgingAp>>($"{url}/GetData/", Filter);
                if (response.Data != null)
                {
                    model = response.Data as List<VwAgingAp>;
                }
            }

            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> Detail(string reffNo)
        {
            var model = new JournalWriteOffAp();
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(reffNo))
            {
                ViewBag.Header = "Update";
                model = await client.Get<JournalWriteOffAp>($"{url}/{reffNo}");
            }

            return View(model);
        }

        public async Task<IActionResult> ReadJournal([DataSourceRequest] DataSourceRequest request, string id)
        {
            var model = await client.Get<List<Journal>>($"{url}/JournalList/{id}") ?? new List<Journal>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(JournalWriteOffAp model)
        {
            model.IsDraft = false;
            return await Process(model, false, false);
        }

        [HttpPost]
        public async Task<IActionResult> Save(JournalWriteOffAp model)
        {
            model.IsDraft = true;
            return await Process(model, true, false);
        }
        [HttpPost]
        public async Task<IActionResult> CreateJournal(JournalWriteOffAp model)
        {
            model.IsDraft = true;
            return await Process(model, true, true);
        }

        private async Task<IActionResult> Process(JournalWriteOffAp model, bool isDraft, bool isJournal)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                if (isJournal)
                {
                    response = await client.PutApiResponse<JournalWriteOffAp>($"{url}/EditJournal", model);
                }
                else
                {
                    response = await client.PutApiResponse<JournalWriteOffAp>(url, model);
                }
            }
            else
            {
                if (isJournal)
                {
                    response = await client.PostApiResponse<JournalWriteOffAp>($"{url}/CreateJournal", model);
                }
                else
                {
                    response = await client.PostApiResponse<JournalWriteOffAp>(url, model);
                }
            }

            if (isJournal)
            {
                return Json(new { success = response.Success, message = response.Message, refreshGrid = "gridJournal", data = response.Data as JournalWriteOffAp });
            }
            else if (isDraft)
            {
                return Json(new { success = response.Success, message = response.Message, data = response.Data as JournalWriteOffAp });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(JournalWriteOffAp model)
        {
            var response = await client.DeleteApiResponse<JournalWriteOffAp>($"{url}/{model.Id}");
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public ActionResult Download(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
    }
}