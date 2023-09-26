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
    public class JournalEntryController : Controller
    {
        string url = ApiUrl.JournalEntryUrl;
        private readonly ApiClientFactory client;

        public JournalEntryController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(string Id)
        {
            var model = new JournalEntry();
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<JournalEntry>($"{url}/{Id}");
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, JournalEntry search)
        {
            var model = await client.PostApiResponse<List<JournalEntry>>($"{url}/GetList", search);
            var List = new List<JournalEntry>();
            if (model.Data != null) List = model.Data as List<JournalEntry>;
            var data = List.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadDetail([DataSourceRequest] DataSourceRequest request, string id)
        {
            var model = await client.Get<List<JournalEntryDetail>>($"{url}/DetailJournalList/{id}") ?? new List<JournalEntryDetail>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }


        [AcceptVerbs("Post")]
        public ActionResult CreateDetail([DataSourceRequest] DataSourceRequest request, JournalEntryDetail model)
        {
            if (model != null && ModelState.IsValid)
            {
                model.Id = Guid.NewGuid().ToString();
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult UpdateDetail([DataSourceRequest] DataSourceRequest request, JournalEntryDetail model)
        {
            if (model != null && ModelState.IsValid)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult DestroyDetail([DataSourceRequest] DataSourceRequest request, JournalEntryDetail model)
        {
            if (model != null)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadJournal([DataSourceRequest] DataSourceRequest request, string id)
        {
            var model = await client.Get<List<Journal>>($"{url}/JournalList/{id}") ?? new List<Journal>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(JournalEntry model)
        {
            model.IsDraft = false;
            return await Process(model, false, false);
        }

        [HttpPost]
        public async Task<IActionResult> Save(JournalEntry model)
        {
            model.IsDraft = true;
            return await Process(model, true, false);
        }
        [HttpPost]
        public async Task<IActionResult> CreateJournal(JournalEntry model)
        {
            model.IsDraft = true;
            return await Process(model, true, true);
        }

        private async Task<IActionResult> Process(JournalEntry model, bool isDraft, bool isJournal)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                if (isJournal)
                {
                    response = await client.PutApiResponse<JournalEntry>($"{url}/EditJournal", model);
                }
                else
                {
                    response = await client.PutApiResponse<JournalEntry>(url, model);
                }
            }
            else
            {
                if (isJournal)
                {
                    response = await client.PostApiResponse<JournalEntry>($"{url}/CreateJournal", model);
                }
                else
                {
                    response = await client.PostApiResponse<JournalEntry>(url, model);
                }
            }

            if (isJournal)
            {
                return Json(new { success = response.Success, message = response.Message, refreshGrid = "gridJournal", data = response.Data as JournalEntry });
            }
            else if (isDraft)
            {
                return Json(new { success = response.Success, message = response.Message, data = response.Data as JournalEntry });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(JournalEntry model)
        {
            var response = await client.DeleteApiResponse<JournalEntry>($"{url}/{model.Id}");
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }
    }
}