using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.WebNetCore;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using Phoenix.WebExtension.RestApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Http;
using PhoenixERPWebApp.Models.Media.UploadMasterTemplate;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.Media.MediaPlanEstimates;
using PhoenixERPWebApp.Models.Media.MediaOrder;

namespace PhoenixERPWebApp.Areas.Media.Controllers
{
    [Area(nameof(Media))]
    public class MediaOrderController : BaseController
    {
        string url = ApiUrl.MediaOrderURL;
        string urlPlan = ApiUrl.MediaPlanEstimatesURL;
        private readonly ApiClientFactory client;
        public MediaOrderController(ApiClientFactory client)
        {
            this.client = client;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, string status)
        {
            if (string.IsNullOrEmpty(status))
                status = "-";
            var model = await client.Get<List<MediaOrderHeader>>($"{url}/getdata/{status}") ?? new List<MediaOrderHeader>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> AddEditOrder(string planno, bool isApprove)
        {
            var modelPlan = new MediaPlanHeader();
            var modelDetail = new List<MediaOrderDetail>();
            MediaOrderHeader modelOrder = new MediaOrderHeader();
            var isCurrentApproval = false;
            modelOrder = await client.Get<MediaOrderHeader>($"{url}/{"getorderbyid"}/{planno}");
            modelDetail = await client.Get<List<MediaOrderDetail>>($"{url}/{"getmediaorderdetails"}/{modelOrder.Id}") ?? new List<MediaOrderDetail>();
            modelPlan = await client.Get<MediaPlanHeader>($"{urlPlan}/{"getplanheaderbyid"}/{planno}");
            isCurrentApproval = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{modelOrder.Id}");
            ViewBag.processApprove = isApprove && isCurrentApproval;
            modelOrder.isApprove = isApprove;
            if (modelDetail.Count != 0)
                modelOrder.MediaOrderDetails = modelDetail;
            modelOrder.PlanHeaders = modelPlan;


            return View(modelOrder);
        }

        public async Task<IActionResult> ReadDetailMediaPlan([DataSourceRequest] DataSourceRequest request, string headerId)
        {
            var model = new List<MediaPlanDetail>();
            if (!string.IsNullOrEmpty(headerId))
                model = await client.Get<List<MediaPlanDetail>>($"{urlPlan}/{"getdetailmediaplan"}/{headerId}/{"order"}") ?? new List<MediaPlanDetail>();

            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });

        }

        public async Task<IActionResult> DetailOrder_Read([DataSourceRequest] DataSourceRequest request, string headerId)
        {
            var model = new List<MediaOrderDetail>();
            if (!string.IsNullOrEmpty(headerId))
                model = await client.Get<List<MediaOrderDetail>>($"{url}/{"getmediaorderdetails"}/{headerId}") ?? new List<MediaOrderDetail>();

            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });

        }

        public async Task<IActionResult> GetVendorData(string vendorid)
        {
            var model = new MediaOrderHeader();
            if (!string.IsNullOrEmpty(vendorid))
                model = await client.Get<MediaOrderHeader>($"{url}/{"getvendordata"}/{vendorid}") ?? new MediaOrderHeader();

            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });

        }

        [HttpPost]
        public async Task<IActionResult> Submit(MediaOrderHeader model) 
        {
            if (model.MediaOrderDetails.Count > 0)
            {
                model.StatusID = StatusTransaction.WaitingApproval.ToString();
                return await Process(model, false);
            }
            else
            {
                return Json(new { success = false, message = "Please Enter Media Order Detail" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save(MediaOrderHeader model, IFormFile file)
        {
            if(model.MediaOrderDetails.Count > 0)
            {
                return await Process(model, true);
            }
            else
            {
                return Json(new { success = false, message = "Please Enter Media Order Detail" });
            }
            
        }


        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm(Name = "filesorder")]IFormFile formId)
        {
            var response = new ApiResponse();
            response = await client.PostApiResponse<MediaOrderDetail>($"{url}/uploadfile", null, formId);
            return Json(new { success = response.Success, message = response.Message, data = response.Data });

        }
        [HttpPost]
        public async Task<IActionResult> Approve(MediaOrderHeader model, IFormFile file)
        {

            var response = await client.PutApiResponse<MediaOrderHeader>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public async Task<IActionResult> Finish(MediaOrderHeader model)
        {
            var response = await client.PutApiResponse<MediaOrderHeader>($"{url}/Finish", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        public async Task<IActionResult> SaveInvoice(MediaOrderHeader model, IFormFile file)
        {

            var response = await client.PostApiResponse<MediaOrderHeader>($"{url}/saveinvoice", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public async Task<IActionResult> Reject(MediaOrderHeader model)
        {
            var response = await client.PutApiResponse<MediaOrderHeader>($"{url}/Cancel", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }
        private async Task<IActionResult> Process(MediaOrderHeader model, bool isDraft)
        {
            try
            {
                var response = new ApiResponse();
                if (!string.IsNullOrEmpty(model.Id))
                {
                    response = await client.PutApiResponse<MediaOrderHeader>(url, model);//edit
                }
                else
                {
                    response = await client.PostApiResponse<MediaOrderHeader>(url, model);// add new
                }

                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
                //if (isDraft)
                //{
                //    return Json(new { success = response.Success, message = response.Message, data = response.Data as MediaOrderHeader });
                //}
                //else
                //{
                //    return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
                //}

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message.ToString(), url = Url.Action(nameof(Index)) });
            }

        }

        public async Task<ActionResult> SaveFileAsync(IEnumerable<IFormFile> filenane)
        {
            return Content("");
        }

        public async Task<IActionResult> UploadReport(MediaOrderHeader model, IFormFile reportfiles)
        {
            var response = new ApiResponse();
            if (reportfiles != null)
            {
                response = await client.PutApiResponse<MediaOrderHeader>($"{url}/uploadreport", model, reportfiles);
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
            else
            {
                return Json(new { success = false, message = "Please choose report file" });
            }
        }

        public async Task<IActionResult> GetFileTemplate(string typeid)
        {
            var model = new TrMediaFileMaster();
            if (!string.IsNullOrEmpty(typeid))
                model = await client.Get<TrMediaFileMaster>($"{ApiUrl.UploadMasterTemplateUrl}/{typeid}") ?? new TrMediaFileMaster();

            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });

        }
    }
}