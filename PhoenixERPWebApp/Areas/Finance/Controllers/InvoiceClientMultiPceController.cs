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
using Kendo.Mvc.UI;
using PhoenixERPWebApp.Models.Finance.Transaction;
using PhoenixERPWebApp.Models.Enum;
using Kendo.Mvc.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PhoenixERPWebApp.Models.Finance;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class InvoiceClientMultiPceController : BaseController
    {
        private readonly ApiClientFactory client;
        string url = ApiUrl.InvoiceClientMultipleUrl;

        public InvoiceClientMultiPceController(ApiClientFactory client)
        {
            this.client = client;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ReadIndex([DataSourceRequest] DataSourceRequest request, FilterInvoiceClient Filter)
        {
            var model = new List<InvoiceClient>();
            var response = new ApiResponse();
            response = await client.PostApiResponse<List<InvoiceClient>>($"{url}/GetInvoiceList/", Filter);
            if (response.Data != null)
            {
                model = response.Data as List<InvoiceClient>;
            }

            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        } 

        public async Task<IActionResult> Detail(string multipceid)
        {
            var model = new InvoiceClient();
            ViewBag.processApprove = false;
            if (!string.IsNullOrEmpty(multipceid))
            {
                model = await client.Get<InvoiceClient>($"{url}/GetPceInfo/{multipceid}");
                ViewBag.processApprove = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{multipceid}");
                ViewBag.isRequestor = ViewBag.processApprove == false && (model.Status == InvoiceClientStatus.Open || model.Status == InvoiceClientStatus.Rejected);
            }
            else
            {
                model.Status = InvoiceClientStatus.Open;
                model.VAtPercentage = 10;
                ViewBag.isRequestor = ViewBag.processApprove == false;
            }

            ViewBag.CreateJournal = ViewBag.isRequestor;
            return View(model);
        }

        public async Task<IActionResult> ReadInvDtl([DataSourceRequest] DataSourceRequest request, string multipceid)
        {
            var data = await client.Get<List<InvoiceClient>>($"{url}/GetInvoiceDetail/{multipceid}") ?? new List<InvoiceClient>();
            var model = data.ToDataSourceResult(request);
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> GetPceChoiceList([DataSourceRequest] DataSourceRequest request, string clientid, string legalid, string brandid, string mainservice, string currencyid, string divisionid, string invoiceId, bool? pceTypeCrossBilling)
        {
            var data = await client.Get<List<InvoiceClient>>($"{url}/GetPceList/{clientid ?? "0"}/{legalid ?? "0"}/{brandid ?? "0"}/{mainservice ?? "0"}/{currencyid ?? "0"}/{divisionid ?? "0"}/{ invoiceId ?? "0"}/{pceTypeCrossBilling}") ?? new List<InvoiceClient>();
            var model = data.ToDataSourceResult(request);
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> ReadPceTaskDetail([DataSourceRequest] DataSourceRequest request, string pceId)
        {
            var dataTask = await client.Get<List<InvoiceClientItemOther>>($"{url}/GetPceTask/{pceId}") ?? new List<InvoiceClientItemOther>();
            var data = dataTask.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadMultipleBrand([DataSourceRequest] DataSourceRequest request, string multipceid)
        {
            var data = await client.Get<List<MultipleBrand>>($"{url}/GetMultipleBrandList/{multipceid}") ?? new List<MultipleBrand>();
            var model = data.ToDataSourceResult(request);
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Save(InvoiceClient invoice, IFormFile fileVatDoc, IFormFile filePoDoc)
        {
            invoice.Status = InvoiceClientStatus.Open;
            return await Process(invoice, fileVatDoc, filePoDoc, true);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(InvoiceClient invoice, IFormFile fileVatDoc, IFormFile filePoDoc)
        {
            invoice.Status = InvoiceClientStatus.WaitingApproval;
            return await Process(invoice, fileVatDoc, filePoDoc, false);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(InvoiceClient model)
        {
            var response = await client.PutApiResponse<InvoiceClient>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public async Task<IActionResult> Reject(InvoiceClient model)
        {
            var response = await client.PutApiResponse<InvoiceClient>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public async Task<IActionResult> CreateJournal(InvoiceClient model, IFormFile fileVatDoc, IFormFile filePoDoc)
        {
            if (fileVatDoc != null) { model.VatDocFileName = fileVatDoc.FileName; }
            if (filePoDoc != null) { model.PoDocFileName = filePoDoc.FileName; }
            var response = await client.PostApiResponse<InvoiceClient>($"{url}/CreateJournal", model, fileVatDoc, filePoDoc);
            var data = response.Data as InvoiceClient;
            var setHiddenValue = new[] { new { id = "InvoiceNumber", value = data.InvoiceNumber }, new { id = "MultiPceId", value = data.MultiPceId } };
            return Json(new { success = response.Success, message = response.Message, refreshGrid = "gridjournal;gridstatuslogHistoryList", setHiddenValue = setHiddenValue, data = data });
        }

        private async Task<IActionResult> Process(InvoiceClient invoice, IFormFile fileVatDoc, IFormFile filePoDoc, bool isDraft)
        {
            var response = new ApiResponse();
            if (fileVatDoc != null) { invoice.VatDocFileName = fileVatDoc.FileName; }
            if (filePoDoc != null) { invoice.PoDocFileName = filePoDoc.FileName; }

            if (!string.IsNullOrEmpty(invoice.MultiPceId))
            {
                response = await client.PutApiResponse<InvoiceClient>(url, invoice, fileVatDoc, filePoDoc);
            }
            else
            {
                response = await client.PostApiResponse<InvoiceClient>(url, invoice, fileVatDoc, filePoDoc);
            }

            if (isDraft)
            {
                var data = response.Data as InvoiceClient;
                var setHiddenValue = new[] { new { id = "InvoiceNumber", value = data.InvoiceNumber }, new { id = "MultiPceId", value = data.MultiPceId } };
                return Json(new { success = response.Success, message = response.Message, refreshGrid = "gridstatuslogHistoryList", setHiddenValue = setHiddenValue, data = data });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        public async Task<IActionResult> ReadJournalInvoiceClient([DataSourceRequest] DataSourceRequest request, string multipceid)
        {
            var model = await client.Get<List<Journal>>($"{url}/GetDataJournal/{multipceid}") ?? new List<Journal>();
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
    }
}