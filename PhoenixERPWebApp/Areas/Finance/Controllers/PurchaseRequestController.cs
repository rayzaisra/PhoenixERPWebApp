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
using PhoenixERPWebApp.Models.Finance.Transaction;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using Kendo.Mvc.Extensions;
using Newtonsoft.Json.Serialization;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.Enum;
using PhoenixERPWebApp.Models.Finance.Transaction.Budget;
using Microsoft.AspNetCore.Http;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class PurchaseRequestController : BaseController
    {
        string url = ApiUrl.PurchaseRequestUrl;
        private readonly ApiClientFactory client;
        public PurchaseRequestController(ApiClientFactory _client)
        {
            this.client = _client;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(string Id)
        {
            var model = new PurchaseRequest();
            ViewBag.processApprove = false;
            ViewBag.checkBudget = false;
            if (!string.IsNullOrEmpty(Id))
            {
                model = await client.Get<PurchaseRequest>($"{url}/{Id}");
                ViewBag.processApprove = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.isRequestor = ViewBag.processApprove == false && (model.Status == StatusTransactionName.Draft || model.Status == StatusTransactionName.Rejected);
                ViewBag.checkBudget = ViewBag.processApprove;// model.BudgetOwnerId == requestor.EmployeeId;
                if (ViewBag.processApprove == true)
                    ViewBag.Header = "Approve";
                else
                    ViewBag.Header = "Update";
            }
            else
            {

                var currencyDefault = await client.Get<Currency>($"{ApiUrl.CurrencyUrl}/GetCurrencyDefault");
                ViewBag.Header = "Create";
                model.RequestDatetime = DateTime.Now;
                model.BudgetYear = DateTime.Now.Year;
                model.CurrencyId = currencyDefault.Id;
                model.CurrencySymbol = currencyDefault.CurrencySymbol;
                model.ExchangeRate = 1;
                model.Operator = "*";
                model.VatPercent = 0;
                ViewBag.isRequestor = ViewBag.processApprove == false;

                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                if (requestor != null)
                {
                    model.RequestorDivisionId = requestor.DivisionId;
                    model.Requestor = requestor.EmployeeName;
                }
            }
            return View(model);
        }

        public IActionResult IndexRef(int aptype, string VendorId, string Id)
        {
            ViewBag.ApType = aptype;
            ViewBag.Id = Id;
            return PartialView("_IndexRef");
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, string status)
        {
            var model = await client.Get<List<PurchaseRequest>>($"{url}/GetPRList/{status}") ?? new List<PurchaseRequest>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadDetail([DataSourceRequest] DataSourceRequest request, string Id)
        {
            var model = await client.Get<List<PurchaseRequestDetail>>($"{url}/GetDetail/{Id}") ?? new List<PurchaseRequestDetail>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadBudget([DataSourceRequest] DataSourceRequest request, string Id)
        {
            var model = await client.Get<List<BudgetMaster>>($"{url}/GetBudgetByPRId/{Id}") ?? new List<BudgetMaster>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Save(PurchaseRequest model, IFormFile file)
        {
            model.StatusApproval = StatusTransaction.Draft;
            return await Process(model, file, true);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(PurchaseRequest model, IFormFile file)
        {
            model.StatusApproval = StatusTransaction.WaitingApproval;
            return await Process(model, file, false);
        }

        [HttpPut]
        public async Task<IActionResult> Approve(PurchaseRequest model)
        {
            model.StatusApproval = StatusTransaction.Approved;
            var response = await client.PutApiResponse<PurchaseRequest>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPut]
        public async Task<IActionResult> Reject(PurchaseRequest model)
        {
            model.StatusApproval = StatusTransaction.Rejected;
            var response = await client.PutApiResponse<PurchaseRequest>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        private async Task<IActionResult> Process(PurchaseRequest model, IFormFile file, bool isDraft)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<PurchaseRequest>(url, model, file);
            }
            else
            {
                response = await client.PostApiResponse<PurchaseRequest>(url, model, file);
            }

            if (isDraft)
            {
                var data = response.Data as PurchaseRequest;
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
        public async Task<IActionResult> Delete(PurchaseRequest model) => Json(await client.DeleteApiResponse<PurchaseRequest>($"{url}/{model.Id}"));

        [AcceptVerbs("Post")]
        public ActionResult CreateDetail([DataSourceRequest] DataSourceRequest request, PurchaseRequestDetail model)
        {
            if (model != null && ModelState.IsValid)
            {
                model.Id = Guid.NewGuid().ToString();
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult UpdateDetail([DataSourceRequest] DataSourceRequest request, PurchaseRequestDetail model)
        {
            if (model != null && ModelState.IsValid)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult DestroyDetail([DataSourceRequest] DataSourceRequest request, PurchaseRequestDetail model)
        {
            if (model != null)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        public async Task<IActionResult> GetItemId(string id)
        {
            var model = await client.Get<ViewBudgetCode>($"{url}/GetBudgetMasterById/{id}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}