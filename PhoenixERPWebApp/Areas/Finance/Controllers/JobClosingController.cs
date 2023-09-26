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
    public class JobClosingController : Controller
    {
        string url = ApiUrl.JobClosingUrl;
        private readonly ApiClientFactory client;

        public JobClosingController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(string jobId)
        {
            var model = new JobClosingHeader();
            ViewBag.processApprove = false;
            if (!string.IsNullOrEmpty(jobId))
            {
                if (ViewBag.processApprove == true)
                    ViewBag.Header = "Approve";
                else
                    ViewBag.Header = "Edit";
                model = await client.Get<JobClosingHeader>($"{url}/{jobId}");
                ViewBag.processApprove = string.IsNullOrEmpty(model.Id) ? false : await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{model.Id}");
                ViewBag.isRequestor = ViewBag.processApprove == false && (model.FinanceStatus == StatusTransactionName.Draft || model.FinanceStatus == StatusTransactionName.Rejected);
            }
            else
            {
                ViewBag.Header = "Create";
                ViewBag.isRequestor = ViewBag.processApprove == false;
                model.FinanceStatus = StatusTransactionName.Draft;
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<VwJobClosing>>(url) ?? new List<VwJobClosing>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadJobClosingAPList([DataSourceRequest] DataSourceRequest request, string JobId)
        {
            var model = await client.Get<List<VwJobClosingAp>>($"{url}/jobclosingaplist/{JobId}") ?? new List<VwJobClosingAp>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadJobClosingARList([DataSourceRequest] DataSourceRequest request, string JobId)
        {
            var model = await client.Get<List<VwJobClosingAr>>($"{url}/jobclosingarlist/{JobId}") ?? new List<VwJobClosingAr>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadJobClosingSummary([DataSourceRequest] DataSourceRequest request, string id)
        {
            var model = await client.Get<List<VwJobClosingSummary>>($"{url}/JobClosingSummary/{id}") ?? new List<VwJobClosingSummary>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadJournal([DataSourceRequest] DataSourceRequest request, string id)
        {
            var model = await client.Get<List<Journal>>($"{url}/JournalList/{id}") ?? new List<Journal>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> updateJournal([DataSourceRequest] DataSourceRequest request, Journal model)
        {
            if (model != null && ModelState.IsValid)
            {
                var response = await client.PutApiResponse<Journal>($"{url}/UpdateJournal", model);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPut]
        public async Task<IActionResult> Approve(JobClosingHeader model)
        {
            model.StatusApproval = StatusTransaction.Approved;
            var response = await client.PutApiResponse<JobClosingHeader>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPut]
        public async Task<IActionResult> Reject(JobClosingHeader model)
        {
            model.StatusApproval = StatusTransaction.Rejected;
            var response = await client.PutApiResponse<JobClosingHeader>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(JobClosingHeader model)
        {
            model.StatusApproval = StatusTransaction.WaitingApproval;
            return await Process(model, 2);
        }

        [HttpPost]
        public async Task<IActionResult> Save(JobClosingHeader model)
        {
            model.StatusApproval = StatusTransaction.Draft;
            return await Process(model, 1);
        }

        [HttpPost]
        public async Task<IActionResult> Generate(JobClosingHeader model)
        {
            return await Process(model, 3);
        }

        [HttpPost]
        public async Task<IActionResult> CreateJournal(JobClosingHeader model)
        {
            var response = await client.PostApiResponse<JobClosingHeader>($"{url}/CreateJournal", model);
            return Json(new { success = response.Success, message = response.Message, refreshGrid = "gridJournal;gridSummary", data = response.Data as JobClosingHeader });
        }

        private async Task<IActionResult> Process(JobClosingHeader model, int btnProcess)
        {
            //1. Save, 2. Submit, 3. Generate
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<JobClosingHeader>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<JobClosingHeader>(url, model);
            }
            if (btnProcess == 3)
            {
                return Json(new { success = response.Success, message = response.Message, refreshGrid = "gridSummary", data = response.Data as JobClosingHeader });
            }
            else if (btnProcess == 1)
            {
                return Json(new { success = response.Success, message = response.Message, refreshGrid = "gridstatuslogHistoryList;gridSummary", data = response.Data as JobClosingHeader });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        public async Task<IActionResult> ReadJournalEntry([DataSourceRequest] DataSourceRequest request, string id)
        {
            var model = await client.Get<List<JobClosingDetailJournal>>($"{url}/EntryJournalList/{id}") ?? new List<JobClosingDetailJournal>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }


        [AcceptVerbs("Post")]
        public ActionResult CreateJournalEntry([DataSourceRequest] DataSourceRequest request, JobClosingDetailJournal model)
        {
            if (model != null && ModelState.IsValid)
            {
                model.Id = Guid.NewGuid().ToString();
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult UpdateJournalEntry([DataSourceRequest] DataSourceRequest request, JobClosingDetailJournal model)
        {
            if (model != null && ModelState.IsValid)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult DestroyJournalEntry([DataSourceRequest] DataSourceRequest request, JobClosingDetailJournal model)
        {
            if (model != null)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}