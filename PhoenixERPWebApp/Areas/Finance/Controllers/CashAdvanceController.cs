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
using Microsoft.AspNetCore.Hosting.Internal;
using PhoenixERPWebApp.Models.Enum;
using PhoenixERPWebApp.Models.Finance.Transaction;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class CashAdvanceController : BaseController
    {
        string url = ApiUrl.CashAdvanceUrl;
        private readonly ApiClientFactory client;

        public CashAdvanceController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(string Id, string IsRead)
        {
            var model = new CashAdvance();
            ViewBag.processApprove = false;
            ViewBag.IsRead = !string.IsNullOrEmpty(IsRead);
            if (!string.IsNullOrEmpty(Id))
            {
                model = await client.Get<CashAdvance>($"{url}/{Id}") ?? new CashAdvance();
                ViewBag.processApprove = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.isRequestor = ViewBag.processApprove == false && (model.Status == StatusTransactionName.Draft);// || model.Status == StatusTransaction.Rejected);
                if (ViewBag.processApprove == true)
                    ViewBag.Header = "Approve Cash Advance";
                else
                    ViewBag.Header = "Update Cash Advance";
            }
            else
            {
                ViewBag.Header = "Create Cash Advance";
                ViewBag.isRequestor = ViewBag.processApprove == false;

                //PCA --Yanto
                var get = GlobalCache.GetPCA();
                if (get != null)
                {
                    var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                    model.RequestedBy = requestor.EmployeeId;
                    model.RequestEmployeeId = requestor.EmployeeId;
                    model.RequestEmployeeName = requestor.EmployeeName;
                    model.RequestJobGradeName = requestor.Grade;
                    model.RequestJobTitleName = requestor.JobTitle;
                    model.RequestJobGradeId = requestor.GradeId;
                    model.RequestJobTitleId = requestor.JobTitleId;
                    model.RequestBusinessUnitId = requestor.BusinessUnitId;
                    model.CaDate = DateTime.Now;
                    model.RequestedOn = DateTime.Now;

                    JobDetail job = await client.Get<JobDetail>($"{ApiUrl.JobDetail}/{get.JobId}") ?? new JobDetail();
                    model.JobId = get.JobId;
                    model.PcaId = get.Id;
                    model.PceId = get.PceId;
                    model.IsProjectCost = true;
                    model.MainserviceCategoryId = get.MainserviceCategoryId;
                    model.ClientId = job.CompanyId;
                    model.ClientName = get.CompanyName;
                    model.ShareServiceId = get.ShareservicesId;
                    model.PceTypeId = get.TypeOfExpenseId;
                    model.ClientBrandId = job.BrandId;
                    model.BusinessUnitId = job.BusinessUnitDivisionId;
                    model.LegalEntityId = job.LegalEntityId;
                    model.AffiliationId = job.AffiliationId;
                    model.CurrencyId = get.CurrencyId;
                    model.PCACurrencyId = get.CurrencyId;
                    model.DefaultCurrencyId = get.DefaultCurrencyId;
                    model.Revision = 0;
                    model.CurrencySymbol = get.CurrencySymbol;
                    model.CurrencySymbolExchange = get.CurrencySymbol;
                    GlobalCache.ClearPCA();
                }
                else
                {
                    RedirectToAction(nameof(Index));
                }
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
            var returnVal = new List<CashAdvanceDetail>();
            var pcaTaskList = GlobalCache.GetCashAdvanceList();

            if (pcaTaskList != null && pcaTaskList.Count > 0)
            {
                foreach (CashAdvanceDetail data in pcaTaskList)
                {
                    data.Id = Guid.NewGuid().ToString();
                    //data.Amount = data.UnitPrice * data.Qty;
                    returnVal.Add(data);
                }
                GlobalCache.ClearCashAdvanceList();
            }
            else
            {
                if (caId == null)
                {
                    returnVal = new List<CashAdvanceDetail>();
                }
                else
                {
                    returnVal = await client.Get<List<CashAdvanceDetail>>($"{url}/GetDetails/{caId}") ?? new List<CashAdvanceDetail>();
                }
            }
            return Json(returnVal.ToDataSourceResult(request), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult CreateDetail([DataSourceRequest] DataSourceRequest request, CashAdvanceDetail cad)
        {
            if (cad != null && ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(cad.Id)) cad.Id = Guid.NewGuid().ToString();
            }
            return Json(new[] { cad }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs("Post")]
        public ActionResult UpdateDetail([DataSourceRequest] DataSourceRequest request, CashAdvanceDetail cad)
        {
            if (cad != null && ModelState.IsValid) { }
            return Json(new[] { cad }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public ActionResult DestroyDetail([DataSourceRequest] DataSourceRequest request, CashAdvanceDetail cad)
        {
            if (cad != null) { }
            return Json(new[] { cad }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public async Task<IActionResult> Approve(CashAdvance model)
        {
            var response = await client.PutApiResponse<CashAdvance>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public async Task<IActionResult> Reject(CashAdvance model)
        {
            var response = await client.PutApiResponse<CashAdvance>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(CashAdvance model)
        {
            model.StatusApproval = StatusTransaction.WaitingApproval;
            return await Process(model, false);
        }

        [HttpPost]
        public async Task<IActionResult> Save(CashAdvance model)
        {
            model.StatusApproval = StatusTransaction.Draft;
            return await Process(model, true);
        }

        private async Task<IActionResult> Process(CashAdvance model, bool isDraft)
        {
            var response = new ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<CashAdvance>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<CashAdvance>(url, model);
            }

            if (isDraft)
            {
                var data = response.Data as CashAdvance;
                var setHiddenValue = new[] {
                    new { id = "CaNumber", value = data.CaNumber },
                    new { id = "Status", value = data.Status },
                    new { id = "MainserviceCategoryId", value = data.MainserviceCategoryId },
                    new { id = "IsProjectCost", value = data.IsProjectCost.ToString() },
                    new { id = "CaNumber", value = data.CaNumber },
                    new { id = "Revision", value = data.Revision.ToString() },
                    new { id = "RequestedBy", value = data.RequestedBy },
                    new { id = "RequestedOn", value = Convert.ToDateTime(data.RequestedOn).ToString("yyyy/MM/dd")},
                    new { id = "RemarksProcess", value = "" }
                };
                return Json(new { success = response.Success, message = response.Message, refreshGrid = "gridstatuslogHistoryList;DetailGrid", setHiddenValue = setHiddenValue, data = data });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        public async Task<IActionResult> GetVendorData(string vendorid)
        {
            var model = new CashAdvance();
            if (!string.IsNullOrEmpty(vendorid))
                model = await client.Get<CashAdvance>($"{url}/{"GetCompanyInfo"}/{vendorid}") ?? new CashAdvance();

            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}