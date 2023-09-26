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
using Kendo.Mvc.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.Enum;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class PurchaseOrderController : Controller
    {
        string url = ApiUrl.PurchaseOrderUrl;
        private readonly ApiClientFactory client;
        public PurchaseOrderController(ApiClientFactory _client)
        {
            this.client = _client;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(string Id)
        {
            var model = new PurchaseOrder();
            ViewBag.processApprove = false;
            if (!string.IsNullOrEmpty(Id))
            {
                model = await client.Get<PurchaseOrder>($"{url}/{Id}");
                ViewBag.processApprove = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.isRequestor = ViewBag.processApprove == false && (model.Status == PurchaseOrderStatus.Draft || model.Status == PurchaseOrderStatus.Rejected);
                ViewBag.requestorCancel = model.IsCancel && !(model.Status == PurchaseOrderStatus.Closed || model.Status == PurchaseOrderStatus.Cancelled || model.Status == PurchaseOrderStatus.Rejected);
                ViewBag.requestorClose = model.IsClose && !(model.Status == PurchaseOrderStatus.Closed || model.Status == PurchaseOrderStatus.Cancelled || model.Status == PurchaseOrderStatus.Rejected);
                if (ViewBag.processApprove == true)
                    ViewBag.Header = "Approve";
                else
                    ViewBag.Header = "Update";
            }
            else
            {
                ViewBag.Header = "Create";
                ViewBag.isRequestor = ViewBag.processApprove == false;
                model.Status = PurchaseOrderStatus.Draft;
                model.PurchaseOrderDate = DateTime.Now;
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, string status)
        {
            var model = await client.Get<List<PurchaseOrder>>($"{url}/GetPOList/{status}") ?? new List<PurchaseOrder>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        //public async Task<IActionResult> ReadRFQDetail([DataSourceRequest] DataSourceRequest request, string rfqId, string vendorId)
        //{
        //    var model = await client.Get<List<RequestForQuotationDetail>>($"{ApiUrl.RequestForQuotationUrl}/GetDetailRFQList/{rfqId}/{vendorId}") ?? new List<RequestForQuotationDetail>();
        //    var data = model.Where(x => x.VendorId == vendorId).ToDataSourceResult(request);
        //    return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        //}

        public async Task<IActionResult> ReadDetail([DataSourceRequest] DataSourceRequest request, string poId)
        {
            var model = await client.Get<List<PurchaseOrderDetail>>($"{url}/GetDetailPOList/{poId}") ?? new List<PurchaseOrderDetail>();
            var data = model.ToTreeDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> CreateDetail([DataSourceRequest] DataSourceRequest request, PurchaseOrderDetail model)
        {
            if (model != null && ModelState.IsValid)
            {
                model.Id = Guid.NewGuid().ToString();
                model.TaskId = Guid.NewGuid().ToString();
                model.ItemCode = null;
                model.ItemName = null;

                if (!string.IsNullOrEmpty(model.ItemId))
                {
                    var data = await client.Get<ViewItem>($"{ApiUrl.RequestForQuotationUrl}/GetItem/{model.ItemTypeId}/{model.ItemId}") ?? new ViewItem();
                    if (data != null)
                    {
                        model.ItemCode = data.ItemNumber;
                        model.ItemName = data.ItemName;
                    }
                }
            }

            return Json(new[] { model }.ToTreeDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> UpdateDetail([DataSourceRequest] DataSourceRequest request, PurchaseOrderDetail model)
        {
            if (model != null && ModelState.IsValid)
            {
                model.ItemCode = null;
                model.ItemName = null;

                if (!string.IsNullOrEmpty(model.ItemId))
                {
                    var data = await client.Get<ViewItem>($"{ApiUrl.RequestForQuotationUrl}/GetItem/{model.ItemTypeId}/{model.ItemId}") ?? new ViewItem();
                    if (data != null)
                    {
                        model.ItemCode = data.ItemNumber;
                        model.ItemName = data.ItemName;
                    }
                }
            }

            return Json(new[] { model }.ToTreeDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult DestroyDetail([DataSourceRequest] DataSourceRequest request, PurchaseOrderDetail model)
        {
            if (model != null)
            {
            }

            return Json(new[] { model }.ToTreeDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Save(PurchaseOrder model)
        {
            model.Status = PurchaseOrderStatus.Draft;
            return await Process(model, true);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(PurchaseOrder model)
        {
            model.Status = PurchaseOrderStatus.WaitingApproval;
            return await Process(model, false);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(PurchaseOrder model)
        {
            model.Status = PurchaseOrderStatus.Cancelled;
            return await Process(model, false);
        }

        [HttpPost]
        public async Task<IActionResult> Close(PurchaseOrder model)
        {
            model.Status = PurchaseOrderStatus.Closed;
            return await Process(model, false);
        }

        [HttpPut]
        public async Task<IActionResult> Approve(PurchaseOrder model)
        {
            model.StatusApproval = StatusTransaction.Approved;
            var response = await client.PutApiResponse<PurchaseOrder>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPut]
        public async Task<IActionResult> Reject(PurchaseOrder model)
        {
            model.StatusApproval = StatusTransaction.Rejected;
            var response = await client.PutApiResponse<PurchaseOrder>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        private async Task<IActionResult> Process(PurchaseOrder model, bool isDraft)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<PurchaseOrder>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<PurchaseOrder>(url, model);
            }

            if (isDraft)
            {
                var data = response.Data as PurchaseOrder;
                //clear remarks log
                var setHiddenValue = new[] { new { id = "RemarksProcess", value = "" } };
                return Json(new { success = response.Success, message = response.Message, refreshGrid = "gridstatuslogHistoryList", setHiddenValue = setHiddenValue, data = data });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var model = await client.Get<PurchaseOrder>($"{url}/{id}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeData(string Id)
        {
            var model = await client.Get<InfoEmployee>($"{url}/GetEmployeeData/{Id}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> Print(string Id)
        {
            var model = await client.Get<PrintPO>($"{url}/print/{Id}");
            return View(model);
        }

    }
}