using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CodeMarvel.Infrastructure.WebNetCore;
using Kendo.Mvc.UI;
using Phoenix.WebExtension.RestApi;
using Kendo.Mvc.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PhoenixERPWebApp.Models;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class AccountReceivableAgingController : BaseController
    {
        string url = ApiUrl.AccountReceivableAging;
        private readonly ApiClientFactory client;

        public AccountReceivableAgingController(ApiClientFactory client)
        {
            this.client = client;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, VwAgingArFilter Filter)
        {
            var model = new List<VwAgingAr>();
            var response = new ApiResponse();

            if (!string.IsNullOrEmpty(Filter.LegalEntityId)) //harus pilih legal entity terlebih dahulu.
            {
                response = await client.PostApiResponse<List<VwAgingAr>>($"{url}/GetData/", Filter);
                if (response.Data != null)
                {
                    model = response.Data as List<VwAgingAr>;
                }
            }

            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> Detail(string reffNo)
        {
            var model = new JournalWriteOffAr();
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(reffNo))
            {
                ViewBag.Header = "Update";
                model = await client.Get<JournalWriteOffAr>($"{url}/{reffNo}");
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
        public async Task<IActionResult> Submit(JournalWriteOffAr model)
        {
            model.IsDraft = false;
            return await Process(model, false, false);
        }

        [HttpPost]
        public async Task<IActionResult> Save(JournalWriteOffAr model)
        {
            model.IsDraft = true;
            return await Process(model, true, false);
        }
        [HttpPost]
        public async Task<IActionResult> CreateJournal(JournalWriteOffAr model)
        {
            model.IsDraft = true;
            return await Process(model, true, true);
        }

        private async Task<IActionResult> Process(JournalWriteOffAr model, bool isDraft, bool isJournal)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                if (isJournal)
                {
                    response = await client.PutApiResponse<JournalWriteOffAr>($"{url}/EditJournal", model);
                }
                else
                {
                    response = await client.PutApiResponse<JournalWriteOffAr>(url, model);
                }
            }
            else
            {
                if (isJournal)
                {
                    response = await client.PostApiResponse<JournalWriteOffAr>($"{url}/CreateJournal", model);
                }
                else
                {
                    response = await client.PostApiResponse<JournalWriteOffAr>(url, model);
                }
            }

            if (isJournal)
            {
                return Json(new { success = response.Success, message = response.Message, refreshGrid = "gridJournal", data = response.Data as JournalWriteOffAr });
            }
            else if (isDraft)
            {
                return Json(new { success = response.Success, message = response.Message, data = response.Data as JournalWriteOffAr });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(JournalWriteOffAr model)
        {
            var response = await client.DeleteApiResponse<JournalWriteOffAr>($"{url}/{model.Id}");
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