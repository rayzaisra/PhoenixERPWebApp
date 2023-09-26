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
using Phoenix.WebExtension.Attributes;
using Kendo.Mvc.UI;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Kendo.Mvc.Extensions;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models.Enum;
using PhoenixERPWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class InvoiceReceivableController : BaseController
    {
        string url = ApiUrl.InvoiceReceivable;
        private readonly ApiClientFactory client;

        public InvoiceReceivableController(ApiClientFactory client)
        {
            this.client = client;
        }

        public async Task<IActionResult> Index()
        {
            var model = new InvoiceReceivable() { InvoiceStatusId = InvoiceReceivedStatus.Open };
            var group = await client.Get<List<TmUserApp>>($"{ApiUrl.ManageUserUrl}/GetByUserLogin");
            ViewBag.FrontDesk = group.Where(x => x.GroupId == GroupAcess.FrontDesk).Count() > 0;
            return View(model);
        }

        public async Task<IActionResult> Detail(string name, string id)
        {
            var model = new InvoiceReceivable();
            if (!string.IsNullOrEmpty(id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<InvoiceReceivable>($"{url}/{id}");
            }
            else
            {
                ViewBag.Header = "Create";
                model.InvoiceDate = DateTime.Today;
                model.ApprovalFrontDesk = false;
                model.ApprovalAdminDesk = false;
                model.PurchaseOrder = new PurchaseOrder();
                model.Company = new Models.Company();
                model.InvoiceStatusId = InvoiceReceivedStatus.Open;
            }
            model.ListGroup = name;

            var group = await client.Get<List<TmUserApp>>($"{ApiUrl.ManageUserUrl}/GetByUserLogin");
            ViewBag.FrontDesk = group.Where(x => x.GroupId == GroupAcess.FrontDesk).Count() > 0;
            ViewBag.AdminFinance = group.Where(x => x.GroupId == GroupAcess.AdminFinance).Count() > 0;
            ViewBag.AccountingSupervisor = group.Where(x => x.GroupId == GroupAcess.AccountingSupervisor).Count() > 0;
            ViewBag.TaxSupervisor = group.Where(x => x.GroupId == GroupAcess.TaxSupervisor).Count() > 0;
            ViewBag.FinanceManager = group.Where(x => x.GroupId == GroupAcess.FinanceManager).Count() > 0;


            ViewBag.UserAccessFrontDesk = ViewBag.FrontDesk && model.InvoiceStatusId == InvoiceReceivedStatus.Open && !(model.ApprovalAdminDesk ?? false) && !(model.ApprovalFrontDesk ?? false);
            ViewBag.UserAccessAdminFinance = ViewBag.AdminFinance && model.InvoiceStatusId == InvoiceReceivedStatus.Open && (model.ApprovalFrontDesk ?? false) && !(model.ApprovalAdminDesk ?? false);
            ViewBag.UserAccessAccountingSupervisor = ViewBag.AccountingSupervisor && model.InvoiceStatusId == InvoiceReceivedStatus.TAP;
            ViewBag.UserAccessUserTaxation = ViewBag.TaxSupervisor && model.InvoiceStatusId == InvoiceReceivedStatus.TAXTAP;
            ViewBag.UserAccessFinanceManager = ViewBag.FinanceManager && model.InvoiceStatusId == InvoiceReceivedStatus.OpenAP;
            ViewBag.CreateJournal = (ViewBag.UserAccessAccountingSupervisor || ViewBag.UserAccessUserTaxation || ViewBag.UserAccessFinanceManager);//tambahan finance manager bisa create journal karena bisa merubah nilai vat
            ViewBag.DetailInvoiceShowAccountId = model.MainServiceCategoryId == MainServiceCategoryOther.MainServiceCategoryId && !(ViewBag.FrontDesk && ViewBag.AdminFinance);
            ViewBag.EditInvoice = ViewBag.UserAccessAdminFinance || ViewBag.UserAccessAccountingSupervisor || ViewBag.UserAccessUserTaxation || ViewBag.UserAccessFinanceManager;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(string poId)
        {
            var model = await client.Get<InvoiceReceivable>($"{url}/GetPoById/{poId}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadHistoryPayment([DataSourceRequest] DataSourceRequest request, string poId)
        {
            var model = await client.Get<List<InvoiceReceivable>>($"{url}/GetHistoryPayment/{poId}") ?? new List<InvoiceReceivable>();
            return Json(model.ToDataSourceResult(request), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, string status, string businessUnit, string legalEntity, string invoiceNumber, string invoiceVendor)
        {
            var model = await client.Get<List<InvoiceReceivable>>($"{url}/GetList/{(status ?? "0")}/{(businessUnit ?? "0")}/{(legalEntity ?? "0")}/{(invoiceNumber ?? "0")}/{(invoiceVendor ?? "0")}") ?? new List<InvoiceReceivable>();
            return Json(model.ToDataSourceResult(request), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadDetail([DataSourceRequest] DataSourceRequest request, string poId, string paymentTo, string invId)
        {
            var model = new List<InvoiceReceivableDetail>();
            if (poId != null) model = await client.Get<List<InvoiceReceivableDetail>>($"{url}/GetDetailInvoiceList/{poId}/{paymentTo}/{invId}") ?? new List<InvoiceReceivableDetail>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadJournal([DataSourceRequest] DataSourceRequest request, string invId)
        {
            var model = await client.Get<List<Journal>>($"{url}/GetJournal/{invId}") ?? new List<Journal>();
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

        [AcceptVerbs("Post")]
        public async Task<IActionResult> CreateJournalOther([DataSourceRequest] DataSourceRequest request, Journal model)
        {
            if (model != null && ModelState.IsValid)
            {
                model.Id = Guid.NewGuid().ToString();
                var response = await client.PutApiResponse<Journal>($"{url}/CreateJournalOther", model);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }


        [AcceptVerbs("Post")]
        public IActionResult UpdateDetail([DataSourceRequest] DataSourceRequest request, InvoiceReceivableDetail model)
        {
            if (model != null && ModelState.IsValid)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Save(InvoiceReceivable model, IFormFile filePoObject, IFormFile fileTaxObject,
            IFormFile fileContractObject, IFormFile fileDoObject, IFormFile fileGrObject, IFormFile fileInvObject)
        {
            return await Process(model, filePoObject, fileTaxObject, fileContractObject, fileDoObject, fileGrObject, fileInvObject, true);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitFrontDesk(InvoiceReceivable model, IFormFile filePoObject, IFormFile fileTaxObject,
            IFormFile fileContractObject, IFormFile fileDoObject, IFormFile fileGrObject, IFormFile fileInvObject)
        {
            model.InvoiceStatusId = InvoiceReceivedStatus.Open;
            model.ApprovalFrontDesk = true;
            return await Process(model, filePoObject, fileTaxObject, fileContractObject, fileDoObject, fileGrObject, fileInvObject, false);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitAdminFinance(InvoiceReceivable model, IFormFile filePoObject, IFormFile fileTaxObject,
            IFormFile fileContractObject, IFormFile fileDoObject, IFormFile fileGrObject, IFormFile fileInvObject)
        {
            model.InvoiceStatusId = InvoiceReceivedStatus.TAP;
            model.ApprovalAdminDesk = true;
            return await Process(model, filePoObject, fileTaxObject, fileContractObject, fileDoObject, fileGrObject, fileInvObject, false);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitAccountingSupervisor(InvoiceReceivable model, IFormFile filePoObject, IFormFile fileTaxObject, IFormFile fileContractObject, IFormFile fileDoObject, IFormFile fileGrObject, IFormFile fileInvObject)
        {
            model.InvoiceStatusId = InvoiceReceivedStatus.TAXTAP;
            return await Process(model, filePoObject, fileTaxObject, fileContractObject, fileDoObject, fileGrObject, fileInvObject, false);
        }

        [HttpPost]
        public async Task<IActionResult> RevisedAccountingSupervisor(InvoiceReceivable model, IFormFile filePoObject, IFormFile fileTaxObject, IFormFile fileContractObject, IFormFile fileDoObject, IFormFile fileGrObject, IFormFile fileInvObject)
        {
            model.InvoiceStatusId = InvoiceReceivedStatus.Open;
            model.ApprovalAdminDesk = false;
            return await Process(model, filePoObject, fileTaxObject, fileContractObject, fileDoObject, fileGrObject, fileInvObject, false);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitUserTaxation(InvoiceReceivable model, IFormFile filePoObject, IFormFile fileTaxObject, IFormFile fileContractObject, IFormFile fileDoObject, IFormFile fileGrObject, IFormFile fileInvObject)
        {
            model.InvoiceStatusId = InvoiceReceivedStatus.OpenAP;
            return await Process(model, filePoObject, fileTaxObject, fileContractObject, fileDoObject, fileGrObject, fileInvObject, false);
        }

        [HttpPost]
        public async Task<IActionResult> RevisedUserTaxation(InvoiceReceivable model, IFormFile filePoObject, IFormFile fileTaxObject, IFormFile fileContractObject, IFormFile fileDoObject, IFormFile fileGrObject, IFormFile fileInvObject)
        {
            model.InvoiceStatusId = InvoiceReceivedStatus.TAP;
            return await Process(model, filePoObject, fileTaxObject, fileContractObject, fileDoObject, fileGrObject, fileInvObject, false);
        }

        [HttpPost]
        public async Task<IActionResult> PostFinanceManager(InvoiceReceivable model, IFormFile filePoObject, IFormFile fileTaxObject, IFormFile fileContractObject, IFormFile fileDoObject, IFormFile fileGrObject, IFormFile fileInvObject)
        {
            model.InvoiceStatusId = InvoiceReceivedStatus.Posted;
            return await Process(model, filePoObject, fileTaxObject, fileContractObject, fileDoObject, fileGrObject, fileInvObject, false);
        }

        [HttpPost]
        public async Task<IActionResult> RevisedFinanceManager(InvoiceReceivable model, IFormFile filePoObject, IFormFile fileTaxObject, IFormFile fileContractObject, IFormFile fileDoObject, IFormFile fileGrObject, IFormFile fileInvObject)
        {
            model.InvoiceStatusId = InvoiceReceivedStatus.TAXTAP;
            return await Process(model, filePoObject, fileTaxObject, fileContractObject, fileDoObject, fileGrObject, fileInvObject, false);
        }

        [HttpPost]
        public async Task<IActionResult> RejectFinanceManager(InvoiceReceivable model, IFormFile filePoObject, IFormFile fileTaxObject, IFormFile fileContractObject, IFormFile fileDoObject, IFormFile fileGrObject, IFormFile fileInvObject)
        {
            model.InvoiceStatusId = InvoiceReceivedStatus.Rejected;
            return await Process(model, filePoObject, fileTaxObject, fileContractObject, fileDoObject, fileGrObject, fileInvObject, false);
        }

        [HttpPost]
        public async Task<IActionResult> Createjournal(InvoiceReceivable model, IFormFile filePoObject, IFormFile fileTaxObject, IFormFile fileContractObject, IFormFile fileDoObject, IFormFile fileGrObject, IFormFile fileInvObject)
        {
            var response = await client.PutApiResponse<Journal>($"{url}/CreateJournal", model, filePoObject, fileTaxObject, fileContractObject, fileDoObject, fileGrObject, fileInvObject);
            return Json(new { success = response.Success, message = response.Message, refreshGrid = "gridJournal" });
        }

        private async Task<IActionResult> Process(InvoiceReceivable model, IFormFile filePoObject, IFormFile fileTaxObject, IFormFile fileContractObject, IFormFile fileDoObject, IFormFile fileGrObject, IFormFile fileInvObject, bool isDraft)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<InvoiceReceivable>(url, model, filePoObject, fileTaxObject, fileContractObject, fileDoObject, fileGrObject, fileInvObject);
            }
            else
            {
                response = await client.PostApiResponse<InvoiceReceivable>(url, model, filePoObject, fileTaxObject, fileContractObject, fileDoObject, fileGrObject, fileInvObject);
            }

            if (isDraft)
            {
                return Json(new { success = response.Success, message = response.Message, refreshGrid = "gridstatuslogHistoryList", data = response.Data as InvoiceReceivable });
            }
            else
            {
                var url = Url.Action(nameof(Index));
                if (model.ListGroup == "tap") { url = Url.Action(nameof(Index), "TemporaryAccountPayable"); }
                else if (model.ListGroup == "ap") { url = Url.Action(nameof(Index), "AccountPayable"); }
                return Json(new { success = response.Success, message = response.Message, url = url });
            }
        }
    }
}