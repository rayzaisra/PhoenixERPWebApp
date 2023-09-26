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
    public class BudgetCapexController : Controller
    {
        string url = ApiUrl.BudgetCapexUrl;
        private readonly ApiClientFactory client;

        public BudgetCapexController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new BudgetMasterHeader();
            var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<BudgetMasterHeader>($"{url}/{Id}");
            }
            else
            {
                model.CreatedOn = DateTime.Now;
                model.CreatedByName = requestor.EmployeeName;
                model.Version = 0;
                model.BudgetMaster = new BudgetMaster();
                model.CurrencyId = CurrencyIDR.CurrencyId;
                model.ExchangeRate = 1;
                model.Status = "Open"; //"InActive";
            }
            ViewBag.requestorId = requestor.EmployeeId;
            ViewBag.requestor = await client.Get<bool>($"{url}/IsRequestor/{Id}");
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<BudgetMasterHeader>>(url) ?? new List<BudgetMasterHeader>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadBudgetMaster([DataSourceRequest] DataSourceRequest request, string budgetMasterHeaderId)
        {
            var model = await client.Get<List<VwBudgetMaster>>($"{url}/GetBudgetMaster/{budgetMasterHeaderId}") ?? new List<VwBudgetMaster>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadBudgetMasterDetail([DataSourceRequest] DataSourceRequest request, string costSharingAllocationId, string budgetMasterId)
        {
            var model = await client.Get<List<BudgetMasterDetail>>($"{url}/GetBudgetMasterDetails/{costSharingAllocationId ?? "0"}/{budgetMasterId}") ?? new List<BudgetMasterDetail>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Save(BudgetMasterHeader model)
        {
            model.BudgetMaster = null;
            model.BudgetMasterDetail = null;
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<BudgetMasterHeader>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<BudgetMasterHeader>(url, model);
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public async Task<IActionResult> SaveDetail(BudgetMasterHeader model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (model.IsOwner)
            {
                model.BudgetMaster.StatusApproval = StatusTransaction.WaitingApproval;
            }

            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<BudgetMasterHeader>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<BudgetMasterHeader>(url, model);
            }

            return Json(new { success = response.Success, message = response.Message, data = response.Data as BudgetMasterHeader, refreshGrid = "grid-detail", hideModalPopUp = "modalDetails" });
        }

        [HttpPut]
        public async Task<IActionResult> RejectDetail(BudgetMasterHeader model)
        {
            model.BudgetMaster.StatusApproval = StatusTransaction.Rejected;
            var response = await client.PutApiResponse<BudgetMasterHeader>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, refreshGrid = "grid-detail", hideModalPopUp = "modalDetails" });
        }

        [HttpPut]
        public async Task<IActionResult> ApproveDetail(BudgetMasterHeader model)
        {
            model.BudgetMaster.StatusApproval = StatusTransaction.Approved;
            var response = await client.PutApiResponse<BudgetMasterHeader>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, refreshGrid = "grid-detail", hideModalPopUp = "modalDetails" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(BudgetMasterHeader model) => Json(await client.DeleteApiResponse<BudgetMasterHeader>($"{url}/{model.Id}"));

        [HttpDelete]
        public async Task<IActionResult> DeleteDetail(VwBudgetMaster model) => Json(await client.DeleteApiResponse<VwBudgetMaster>($"{url}/Detail/{model.BudgetId}"));


        [HttpGet]
        public async Task<IActionResult> GetMasterBudgetCode(string budgetCodeId)
        {
            var model = await client.Get<BudgetMaster>($"{url}/GetMasterBudgetCode/{budgetCodeId}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        public async Task<IActionResult> GetBudgetMasterById(string masterBudgetCodeId, string id)
        {
            var model = await client.Get<BudgetMaster>($"{url}/GetBudgetMasterById/{masterBudgetCodeId}/{id}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}