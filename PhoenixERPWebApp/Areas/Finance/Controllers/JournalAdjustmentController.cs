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
    public class JournalAdjustmentController : Controller
    {
        string url = ApiUrl.JournalAdjustmentUrl;
        private readonly ApiClientFactory client;

        public JournalAdjustmentController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(string Id)
        {
            var model = new JournalAdjustment();
            ViewBag.processApprove = false;
            if (!string.IsNullOrEmpty(Id))
            {
                model = await client.Get<JournalAdjustment>($"{url}/{Id}");
                ViewBag.processApprove = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.isRequestor = ViewBag.processApprove == false && (model.Status == StatusTransactionName.Draft || model.Status == StatusTransactionName.Rejected);

                if (ViewBag.processApprove == true)
                    ViewBag.Header = "Approve";
                else
                    ViewBag.Header = "Update";
            }
            else
            {
                ViewBag.Header = "Create";
                model.CurrencyId = CurrencyIDR.CurrencyId;
                model.ExchangeRate = 1;
                ViewBag.isRequestor = ViewBag.processApprove == false;
            }
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, JournalAdjustment search)
        {
            var model = await client.PostApiResponse<List<JournalAdjustment>>($"{url}/GetList", search);
            var List = new List<JournalAdjustment>();
            if (model.Data != null) List = model.Data as List<JournalAdjustment>;
            var data = List.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadDetail([DataSourceRequest] DataSourceRequest request, string id)
        {
            var model = await client.Get<List<JournalAdjustmentDetail>>($"{url}/DetailJournalList/{id}") ?? new List<JournalAdjustmentDetail>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult CreateDetail([DataSourceRequest] DataSourceRequest request, JournalAdjustmentDetail model)
        {
            if (model != null && ModelState.IsValid)
            {
                model.Id = Guid.NewGuid().ToString();
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult UpdateDetail([DataSourceRequest] DataSourceRequest request, JournalAdjustmentDetail model)
        {
            if (model != null && ModelState.IsValid)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult DestroyDetail([DataSourceRequest] DataSourceRequest request, JournalAdjustmentDetail model)
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

        public async Task<IActionResult> ReadJournalHistory([DataSourceRequest] DataSourceRequest request, string referenceId)
        {
            var model = await client.Get<List<Journal>>($"{url}/JournalHistoryList/{referenceId}") ?? new List<Journal>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        public async Task<IActionResult> Search(string type, string number)
        {
            var model = await client.Get<JournalAdjustment>($"{url}/GetByNumber/{type}/{number}") ?? new JournalAdjustment();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Save(JournalAdjustment model)
        {
            model.StatusApproval = StatusTransaction.Draft;
            return await Process(model, true, false);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(JournalAdjustment model)
        {
            model.StatusApproval = StatusTransaction.WaitingApproval;
            return await Process(model, false, false);
        }

        [HttpPut]
        public async Task<IActionResult> Approve(JournalAdjustment model)
        {
            model.StatusApproval = StatusTransaction.Approved;
            var response = await client.PutApiResponse<JournalAdjustment>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPut]
        public async Task<IActionResult> Reject(JournalAdjustment model)
        {
            model.StatusApproval = StatusTransaction.Rejected;
            var response = await client.PutApiResponse<JournalAdjustment>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public async Task<IActionResult> CreateJournal(JournalAdjustment model)
        {
            model.StatusApproval = StatusTransaction.Draft;
            return await Process(model, true, true);
        }

        private async Task<IActionResult> Process(JournalAdjustment model, bool isDraft, bool isJournal)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                if (isJournal)
                {
                    response = await client.PutApiResponse<JournalAdjustment>($"{url}/EditJournal", model);
                }
                else
                {
                    response = await client.PutApiResponse<JournalAdjustment>(url, model);
                }
            }
            else
            {
                if (isJournal)
                {
                    response = await client.PostApiResponse<JournalAdjustment>($"{url}/CreateJournal", model);
                }
                else
                {
                    response = await client.PostApiResponse<JournalAdjustment>(url, model);
                }
            }

            if (isJournal)
            {
                return Json(new { success = response.Success, message = response.Message, refreshGrid = "gridJournal;gridstatuslogHistoryList", data = response.Data as JournalAdjustment });
            }
            else if (isDraft)
            {
                var data = response.Data as JournalAdjustment;
                //clear remarks log
                var setHiddenValue = new[] { new { id = "RemarksProcess", value = "" } };
                return Json(new { success = response.Success, message = response.Message, refreshGrid = "gridstatuslogHistoryList", setHiddenValue = setHiddenValue, data = data });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(JournalAdjustment model)
        {
            var response = await client.DeleteApiResponse<JournalAdjustment>($"{url}/{model.Id}");
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }
    }
}