using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.Enum;
using Kendo.Mvc.UI;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Phoenix.WebExtension.Extentions;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class RequestForQuotationController : Controller
    {
        string url = ApiUrl.RequestForQuotationUrl;
        private readonly ApiClientFactory client;
        public RequestForQuotationController(ApiClientFactory _client)
        {
            this.client = _client;
        }

        public async Task<IActionResult> Index()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/MainserviceCategory";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            ViewBag.Category = model;
            return View();
        }

        public async Task<IActionResult> Detail(string Id, string IsRead)
        {
            ViewBag.processApprove = false;
            ViewBag.IsRead = !string.IsNullOrEmpty(IsRead);
            var model = await client.Get<RequestForQuotation>($"{url}/{Id}");
            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.processApprove = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.isRequestor = ViewBag.processApprove == false && (model.Status == StatusTransactionName.Draft || model.Status == StatusTransactionName.OnProcess);
            }
            else
            {
                ViewBag.isRequestor = ViewBag.processApprove == false;
            }

            model.RequestForQuotationVendor = new RequestForQuotationVendor();
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, string mainserviceCategoryId, string status)
        {
            var model = await client.Get<List<RequestForQuotation>>($"{url}/GetRFQList/{mainserviceCategoryId}/{status}") ?? new List<RequestForQuotation>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadTask([DataSourceRequest] DataSourceRequest request, string rfqId)
        {
            var model = await client.Get<List<RequestForQuotationTask>>($"{url}/GetTaskList/{rfqId}") ?? new List<RequestForQuotationTask>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadVendorList([DataSourceRequest] DataSourceRequest request, string rfqId, bool isRequestor)
        {
            var model = await client.Get<List<RequestForQuotationVendor>>($"{url}/GetVendorList/{rfqId}") ?? new List<RequestForQuotationVendor>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadDetail([DataSourceRequest] DataSourceRequest request, string rfqId, string vendorId)
        {
            var model = await client.Get<List<RequestForQuotationDetail>>($"{url}/GetDetailRFQList/{rfqId}/{vendorId}") ?? new List<RequestForQuotationDetail>();
            return Json(model.ToTreeDataSourceResult(request), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public IActionResult CreateDetail([DataSourceRequest] DataSourceRequest request, RequestForQuotationDetail model)
        {
            if (model != null && ModelState.IsValid)
            {
                model.Id = Guid.NewGuid().ToString();
                model.TaskId = Guid.NewGuid().ToString();
            }
            return Json(new[] { model }.ToTreeDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public IActionResult UpdateDetail([DataSourceRequest] DataSourceRequest request, RequestForQuotationDetail model)
        {
            if (model != null && ModelState.IsValid)
            {
            }
            return Json(new[] { model }.ToTreeDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public IActionResult DeleteDetail([DataSourceRequest] DataSourceRequest request, RequestForQuotationDetail model)
        {
            if (model != null)
            {
            }
            return Json(new[] { model }.ToTreeDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        public async Task<IActionResult> GetDataItem(string itemTypeId, string itemId)
        {
            var data = await client.Get<ViewItem>($"{url}/GetItem/{itemTypeId}/{itemId}") ?? new ViewItem();
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> SaveVendor(RequestForQuotation model, IFormFile fileDetail)
        {
            var data = model.RequestForQuotationVendor;
            if (fileDetail != null)
            {
                data.File = fileDetail;
                data.FilemasterName = fileDetail.FileName;
            }

            var response = await client.PutApiResponse<RequestForQuotation>($"{url}/Vendor", model, fileDetail);
            return Json(new { success = response.Success, message = response.Message, data = response.Data as RequestForQuotation, refreshGrid = "grid-RequestforQuotationVendorList", hideModalPopUp = "modalVendor" });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteVendor(RequestForQuotationVendor model) => Json(await client.DeleteApiResponse<RequestForQuotationVendor>($"{url}/vendor/{model.Id}"));

        [HttpPut]
        public async Task<IActionResult> Approve(RequestForQuotation model)
        {
            model.StatusApproval = StatusTransaction.Approved;
            var response = await client.PutApiResponse<RequestForQuotation>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPut]
        public async Task<IActionResult> Reject(RequestForQuotation model)
        {
            model.StatusApproval = StatusTransaction.Rejected;
            var response = await client.PutApiResponse<RequestForQuotation>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(RequestForQuotation model, IFormFile file)
        {
            model.StatusApproval = StatusTransaction.WaitingApproval;
            return await Process(model, file, false);
        }

        [HttpPost]
        public async Task<IActionResult> Save(RequestForQuotation model, IFormFile file)
        {
            model.StatusApproval = StatusTransaction.Draft;
            return await Process(model, file, true);
        }

        private async Task<IActionResult> Process(RequestForQuotation model, IFormFile file, bool isDraft)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<RequestForQuotation>(url, model, file);
            }
            else
            {
                response = await client.PostApiResponse<RequestForQuotation>(url, model, file);
            }

            if (isDraft)
            {
                var data = response.Data as RequestForQuotation;
                //clear remarks log
                var setHiddenValue = new[] { new { id = "RemarksProcess", value = "" } };
                return Json(new { success = response.Success, message = response.Message, refreshGrid = "gridstatuslogHistoryList", setHiddenValue = setHiddenValue, data = data });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        public async Task<IActionResult> Download(string vendorId, string id)
        {
            var response = await client.Get<DownloadFile>($"{ApiUrl.DownloadFileUrl}/{id}");
            return Json(response, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }


        [AcceptVerbs("Post")]
        public async Task<ActionResult> UpdateTask([DataSourceRequest] DataSourceRequest request, RequestForQuotationTask model)
        {
            if (model != null && ModelState.IsValid)
            {
                model.ItemCode = null;
                model.ItemName = null;

                if (!string.IsNullOrEmpty(model.ItemId))
                {
                    var data = await client.Get<ViewBudgetCode>($"{ApiUrl.PurchaseRequestUrl}/GetBudgetMasterById/{model.ItemId}");
                    if (data != null)
                    {
                        model.ItemCode = data.BudgetCode;
                        model.ItemName = data.BudgetName;
                    }
                }
                await client.PutApiResponse<RequestForQuotationTask>($"{url}/task", model);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}