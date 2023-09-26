using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Kendo.Mvc.Extensions;
using System;
using PhoenixERPWebApp.Models.Finance.Transaction;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class PceController : Controller
    {
        string url = ApiUrl.PceUrl;
        private readonly ApiClientFactory client;

        public PceController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Print(string Id)
        {
            ViewBag.Channel = await client.Get<List<SelectListItem>>($"{ApiUrl.dropdownUrl}/Channel") ?? new List<SelectListItem>();
            var model = await client.Get<PrintPce>($"{url}/print/{Id}");
            return View(model);
        }

        public async Task<IActionResult> AddEdit(string Id, string IsRead)
        {
            var model = new Pce();
            ViewBag.processApprove = false;
            ViewBag.IsRead = !string.IsNullOrEmpty(IsRead);
            if (!string.IsNullOrEmpty(Id))
            {
                if (ViewBag.processApprove == true)
                    ViewBag.Header = "Approve";
                else
                    ViewBag.Header = "Update";
                model = await client.Get<Pce>($"{url}/{Id}");
                ViewBag.processApprove = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.isRequestor = ViewBag.processApprove == false && (model.Status == StatusTransactionName.Draft) && model.Status != StatusTransactionName.Revised;
            }
            else
            {
                ViewBag.Header = "Create";
                model.Revision = "0";
                ViewBag.isRequestor = ViewBag.processApprove == false;
                model.Status = StatusTransactionName.Draft;
            }
            model.IsPM = false;

            ViewBag.CrossBilling = TypeOfExpenseCrossBilling.TypeOfExpenseId;

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, string status)
        {
            var model = await client.Get<List<Pce>>($"{url}/GetFinancePCEList/{status}") ?? new List<Pce>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadInvoiceDetail([DataSourceRequest] DataSourceRequest request, string pceId)
        {
            var model = await client.Get<List<InvoiceClient>>($"{url}/invoiceList/{pceId}") ?? new List<InvoiceClient>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        //public async Task<IActionResult> ReadApproval([DataSourceRequest] DataSourceRequest request)
        //{
        //    var model = await client.Get<List<Pce>>($"{url}/approvalList") ?? new List<Pce>();
        //    var data = model.ToDataSourceResult(request);
        //    return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        //}

        [HttpPut]
        public async Task<IActionResult> Approve(Pce model)
        {
            model.StatusApproval = StatusTransaction.Approved;
            var response = await client.PutApiResponse<Pce>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPut]
        public async Task<IActionResult> Reject(Pce model)
        {
            model.StatusApproval = StatusTransaction.Rejected;
            var response = await client.PutApiResponse<Pce>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPut]
        public async Task<IActionResult> SubmitToClient(Pce model)
        {
            var response = await client.PutApiResponse<Pce>($"{url}/SubmitToClient", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(Pce model)
        {
            if (model.StatusApproval == StatusTransaction.Rejected)
            {
                model.Revision = (int.Parse(model.Revision ?? "0") + 1).ToString();
            }
            model.StatusApproval = StatusTransaction.WaitingApproval;
            return await Process(model, false);
        }

        [HttpPost]
        public async Task<IActionResult> Revise(Pce model)
        {
            model.StatusApproval = StatusTransaction.Revised;
            return await Process(model, false);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Pce model)
        {
            //if (model.Status == StatusTransaction.Rejected)
            //{
            //    model.Revision = (int.Parse(model.Revision ?? "0") + 1).ToString();
            //}
            model.StatusApproval = StatusTransaction.Draft;
            return await Process(model, true);
        }

        private async Task<IActionResult> Process(Pce model, bool isDraft)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<Pce>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<Pce>(url, model);
            }

            if (isDraft)
            {
                var data = response.Data as Pce;
                //clear remarks log
                var setHiddenValue = new[] { new { id = "RemarkRejected", value = "" } };
                return Json(new { success = response.Success, message = response.Message, refreshGrid = "gridstatuslogHistoryList", setHiddenValue = setHiddenValue, data = data });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Pce model) => Json(await client.DeleteApiResponse<Pce>($"{url}/{model.Id}"));

        public async Task<IActionResult> ReadTask([DataSourceRequest] DataSourceRequest request, string pceId)
        {
            var model = await client.Get<List<PceTask>>($"{url}/PceTaskList/{pceId}") ?? new List<PceTask>();
            return Json(model.ToTreeDataSourceResult(request), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        //[HttpGet]
        //public async Task<IActionResult> GetByJobId(string jobId)
        //{
        //    var model = await client.Get<Pce>($"{url}/GetByJobId/{jobId}");
        //    return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        //}
    }
}