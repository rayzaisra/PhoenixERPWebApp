using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CodeMarvel.Infrastructure;
using CodeMarvel.Infrastructure.Sessions;
using CodeMarvel.Infrastructure.WebNetCore;
using Microsoft.Extensions.Configuration;
using CodeMarvel.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Phoenix.WebExtension.RestApi;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using PhoenixERPWebApp.Models.Finance;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PhoenixERPWebApp.Models;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using System.IO;
using PhoenixERPWebApp.Models.Finance.Transaction;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class CashAdvanceSattlementController : BaseController
    {
        string url = ApiUrl.CashAdvanceSattleUrl;
        string urlca = ApiUrl.CashAdvanceUrl;
        string urlReverse = ApiUrl.CashAdvanceSattleReverseUrl;
        private readonly ApiClientFactory client;
        public IHostingEnvironment HostingEnvironment { get; set; }

        public CashAdvanceSattlementController(ApiClientFactory client, IHostingEnvironment hostingEnvironment)
        {
            this.client = client;
            HostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(string caId, string IsRead)
        {
            var model = new CASettlement();
            ViewBag.IsRead = !string.IsNullOrEmpty(IsRead);
            if (string.IsNullOrEmpty(caId))
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                model = await client.Get<CASettlement>($"{url}/{caId}");
                ViewBag.processApprove = false;
                ViewBag.CreateJournal = true;
                if (!string.IsNullOrEmpty(model.Id))
                {
                    ViewBag.processApprove = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{model.Id}");
                    ViewBag.isRequestor = ViewBag.processApprove == false && (model.Status == StatusTransactionName.Draft || model.Status == StatusTransactionName.Revised);
                    if (ViewBag.processApprove == true)
                        ViewBag.Header = "Approve Cash Advance Settlement";
                    else
                        ViewBag.Header = "Update Cash Advance Settlement";
                }
                else
                {
                    ViewBag.Header = "Create Cash Advance Settlement";
                    ViewBag.isRequestor = ViewBag.processApprove == false;
                }
                ViewBag.CreateJournal = ViewBag.isRequestor;
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, string legalEntityId, string mainServiceCategoryId)
        {
            var model = await client.Get<List<CashAdvance>>($"{url}/{(string.IsNullOrEmpty(legalEntityId) ? "0" : legalEntityId)}/{(string.IsNullOrEmpty(mainServiceCategoryId) ? "0" : mainServiceCategoryId)}") ?? new List<CashAdvance>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadDetail([DataSourceRequest] DataSourceRequest request, string caId)
        {
            var model = await client.Get<List<CASettlementDetail>>($"{url}/GetDetails/{caId}") ?? new List<CASettlementDetail>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadJournal([DataSourceRequest] DataSourceRequest request, string casId)
        {
            var model = await client.Get<List<Journal>>($"{url}/GetJournal/{casId}") ?? new List<Journal>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult UpdateDetail([DataSourceRequest] DataSourceRequest request, CASettlementDetail cad)
        {
            if (cad != null && ModelState.IsValid) { }
            return Json(new[] { cad }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public async Task<IActionResult> Approve(CASettlement model)
        {
            var response = await client.PutApiResponse<CASettlement>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public async Task<IActionResult> Reject(CASettlement model)
        {
            var response = await client.PutApiResponse<CASettlement>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        //[HttpPost]
        //public async Task<IActionResult> Createjournal(CASettlement model, IFormFile receiptFile, IFormFile evidenceFile)
        //{
        //    var response = new Phoenix.WebExtension.RestApi.ApiResponse();
        //    if (!string.IsNullOrEmpty(model.Id))
        //    {
        //        response = await client.PutApiResponse<CASettlement>($"{url}/EditJournal", model, receiptFile, evidenceFile);
        //    }
        //    else
        //    {
        //        response = await client.PostApiResponse<CASettlement>($"{url}/Createjournal", model, receiptFile, evidenceFile);
        //    }

        //    var data = response.Data as CASettlement;
        //    var setHiddenValue = new[] {
        //            new { id = "SattlementNumber", value = data.SettlementNumber },
        //            new { id = "CaSettlementDate", value = Convert.ToDateTime(data.CaSettlementDate).ToString("yyyy/MM/dd") }
        //        };

        //    return Json(new { success = response.Success, message = response.Message, refreshGrid = "gridjournal", setHiddenValue = setHiddenValue, data = data });
        //}

        [HttpPost]
        public async Task<IActionResult> Save(CASettlement model, IFormFile receiptFile, IFormFile evidenceFile)
        {
            model.StatusApproval = StatusTransaction.Draft;
            return await Process(model, receiptFile, evidenceFile, true);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(CASettlement model, IFormFile receiptFile, IFormFile evidenceFile)
        {
            model.StatusApproval = StatusTransaction.WaitingApproval;
            return await Process(model, receiptFile, evidenceFile, false);
        }

        private async Task<IActionResult> Process(CASettlement model, IFormFile receiptFile, IFormFile evidenceFile, bool isDraft)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<CASettlement>(url, model, receiptFile, evidenceFile);
            }
            else
            {
                response = await client.PostApiResponse<CASettlement>(url, model, receiptFile, evidenceFile);
            }

            if (isDraft)
            {
                var data = response.Data as CASettlement;
                var setHiddenValue = new[] {
                    new { id = "SattlementNumber", value = data.SettlementNumber },
                    new { id = "CaSettlementDate", value = Convert.ToDateTime(data.CaSettlementDate).ToString("yyyy/MM/dd") },
                    new { id = "RemarksProcess", value = "" }
                };
                return Json(new { success = response.Success, message = response.Message, refreshGrid = "gridstatuslogHistoryList", setHiddenValue = setHiddenValue, data = data });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        #region REVERSE
        public async Task<IActionResult> Reverse(string reffNo)
        {
            var model = new CASettlementJournalReverse();
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(reffNo))
            {
                ViewBag.Header = "Update";
                model = await client.Get<CASettlementJournalReverse>($"{urlReverse}/{reffNo}");
            }

            return View(model);
        }

        public async Task<IActionResult> ReadJournalReverse([DataSourceRequest] DataSourceRequest request, string id)
        {
            var model = await client.Get<List<Journal>>($"{urlReverse}/JournalList/{id}") ?? new List<Journal>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> SubmitReverse(CASettlementJournalReverse model)
        {
            model.IsDraft = false;
            return await Process(model, false, false);
        }

        [HttpPost]
        public async Task<IActionResult> SaveReverse(CASettlementJournalReverse model)
        {
            model.IsDraft = true;
            return await Process(model, true, false);
        }
        [HttpPost]
        public async Task<IActionResult> CreateJournalReverse(CASettlementJournalReverse model)
        {
            model.IsDraft = true;
            return await Process(model, true, true);
        }

        private async Task<IActionResult> Process(CASettlementJournalReverse model, bool isDraft, bool isJournal)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                if (isJournal)
                {
                    response = await client.PutApiResponse<CASettlementJournalReverse>($"{urlReverse}/EditJournal", model);
                }
                else
                {
                    response = await client.PutApiResponse<CASettlementJournalReverse>(urlReverse, model);
                }
            }
            else
            {
                if (isJournal)
                {
                    response = await client.PostApiResponse<CASettlementJournalReverse>($"{urlReverse}/CreateJournal", model);
                }
                else
                {
                    response = await client.PostApiResponse<CASettlementJournalReverse>(urlReverse, model);
                }
            }

            if (isJournal)
            {
                return Json(new { success = response.Success, message = response.Message, refreshGrid = "gridJournal", data = response.Data as CASettlementJournalReverse });
            }
            else if (isDraft)
            {
                return Json(new { success = response.Success, message = response.Message, data = response.Data as CASettlementJournalReverse });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteReverse(CASettlementJournalReverse model)
        {
            var response = await client.DeleteApiResponse<CASettlementJournalReverse>($"{urlReverse}/{model.Id}");
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }
        #endregion

    }
}