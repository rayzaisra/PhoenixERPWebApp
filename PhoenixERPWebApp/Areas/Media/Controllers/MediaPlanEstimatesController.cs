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

namespace PhoenixERPWebApp.Areas.Media.Controllers
{
    [Area(nameof(Media))]
    public class MediaPlanEstimatesController : BaseController
    {
        string url = ApiUrl.MediaPlanEstimatesURL;
        private readonly ApiClientFactory client;

        public MediaPlanEstimatesController(ApiClientFactory client)
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
            var model = await client.Get<List<MediaPlanHeader>>($"{url}/getdata/{status}") ?? new List<MediaPlanHeader>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadDetailMediaPlan([DataSourceRequest] DataSourceRequest request, string headerId)
        {
            var model = new List<MediaPlanDetail>();
            if (!string.IsNullOrEmpty(headerId))
                model = await client.Get<List<MediaPlanDetail>>($"{url}/{"getdetailmediaplan"}/{headerId}/{"plan"}") ?? new List<MediaPlanDetail>();

            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });

        }

        public async Task<IActionResult> GetTransactionID(string caseOption, string jobid)
        {
            var transID = await client.Get<MediaPlanHeader>($"{url}/{"GetTransactionID"}/{caseOption}/{jobid}");
            //var data = transID.ToDataSourceResult(request);
            return Json(transID, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }


        public async Task<IActionResult> AddEdit(string Id, bool isApprove)
        {
            var model = new MediaPlanHeader();
            var isCurrentApproval = false;
            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Edit Media Plan Estimates";
            }
            else
            {
                ViewBag.Header = "Add New Media Plan Estimates";
            }


            if (!string.IsNullOrEmpty(Id))
            {
                model = await client.Get<MediaPlanHeader>($"{url}/{"getplanheaderbyid"}/{Id}");
                isCurrentApproval = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
            }
            else
            {
                model = await client.Get<MediaPlanHeader>($"{url}/{"GetTransactionID"}/{"header"}/{"0"}");

                // data = transID.ToDataSourceResult(request);
            }
            ViewBag.processApprove = isApprove && isCurrentApproval;
            model.isApprove = isApprove;
            //var templateFile = await client.Get<TrMediaFileMaster>($"{url}/getTemplateId");
            //if (templateFile != null)
            //{
            //    model.MediaPlanTvFileTemplateId = templateFile.FileMasterId;
            //}

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(MediaPlanHeader model)
        {
            if (model.MediaPlanDetails.Count > 0)
            {
                //if (model.CampaignValue == "1")
                //    model.Campaign = true;
                //else
                //    model.Campaign = false;

                model.StatusID = StatusTransaction.WaitingApproval.ToString();
                return await Process(model, false);
            }
            else
            {
                return Json(new { success = false, message = "Please Enter Media Plan Detail" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Approve(MediaPlanHeader model, IFormFile file)
        {
            var response = await client.PutApiResponse<MediaPlanHeader>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });

        }

        [HttpPost]
        public async Task<IActionResult> Reject(MediaPlanHeader model)
        {
            var response = await client.PutApiResponse<MediaPlanHeader>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public async Task<IActionResult> Finish(MediaPlanHeader model)
        {
            var response = await client.PutApiResponse<MediaPlanHeader>($"{url}/Finish", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        
        [HttpPost]
        public async Task<IActionResult> Save(MediaPlanHeader model)
        {
            if (model.MediaPlanDetails.Count > 0)
            {
                //if (model.CampaignValue == "1")
                //    model.Campaign = true;
                //else
                //    model.Campaign = false;

                model.StatusID = StatusTransaction.Draft.ToString();
                return await Process(model, true);
            }
            else
            {
                return Json(new { success = false, message = "Please Enter Media Plan Detail" });
            }
        }

        public async Task<IActionResult> UploadReport(MediaPlanHeader model, IFormFile reportfiles)
        {
            var response = new ApiResponse();
            if (reportfiles != null)
            {
                response = await client.PutApiResponse<MediaPlanHeader>($"{url}/uploadreport", model, reportfiles);
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
            else
            {
                return Json(new { success = false, message = "Please choose report file" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm(Name = "files")]IFormFile formId)
        {
            var response = new ApiResponse();
            response = await client.PostApiResponse<MediaPlanDetail>($"{url}/uploadfile", null, formId);
            return Json(new { success = response.Success, message = response.Message, data = response.Data });

        }

        private async Task<IActionResult> Process(MediaPlanHeader model, bool isDraft)
        {
            try
            {
                var response = new ApiResponse();
                if (!string.IsNullOrEmpty(model.Id))
                {
                    response = await client.PutApiResponse<MediaPlanHeader>(url, model);//edit
                }
                else
                {
                    response = await client.PostApiResponse<MediaPlanHeader>(url, model);// add new
                }
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });

                //if (isDraft)
                //{
                //    return Json(new { success = response.Success, message = response.Message, data = response.Data as MediaPlanHeader });
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

        public async Task<IActionResult> GetFileTemplate(string typeid)
        {
            var model = new TrMediaFileMaster();
            if (!string.IsNullOrEmpty(typeid))
                model = await client.Get<TrMediaFileMaster>($"{ApiUrl.UploadMasterTemplateUrl}/{typeid}") ?? new TrMediaFileMaster();

            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });

        }
        
    }
}