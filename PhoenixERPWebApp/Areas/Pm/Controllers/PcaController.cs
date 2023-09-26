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
using Microsoft.AspNetCore.Http;

namespace PhoenixERPWebApp.Areas.Pm.Controllers
{
    [Area(nameof(Pm))]
    public class PcaController : Controller
    {
        string url = ApiUrl.PcaUrl;
        private readonly ApiClientFactory client;


        public PcaController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddEdit(string Id, string IsRead)
        {
            var model = new Pca();
            ViewBag.processApprove = false;
            ViewBag.IsRead = !string.IsNullOrEmpty(IsRead);
            if (!string.IsNullOrEmpty(Id))
            {
              
                model = await client.Get<Pca>($"{url}/{Id}");
                ViewBag.processApprove = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.isRequestor = ViewBag.processApprove == false && (model.Status == StatusTransactionName.Draft || model.Status == StatusTransactionName.Revised);
                ViewBag.isProcessRFQAndCA = (model.StatusPCE ?? "").ToUpper() == PCEStatus.Approved.ToUpper(); //Status Open di PCE //model.Status == StatusTransaction.Approved;
                
                if (ViewBag.processApprove == true)
                    ViewBag.Header = "Approve";
                else
                    ViewBag.Header = "Update";
            }
            else
            {
                ViewBag.Header = "Create";
                model.Revision = "0";
                model.OtherFeePercentage = 0;
                model.CurrencyId = CurrencyIDR.CurrencyId;

                var currency = await client.Get<Currency>($"{ApiUrl.CurrencyUrl}/{model.CurrencyId}");
                model.CurrencySymbol = currency.CurrencySymbol;

                ViewBag.isRequestor = ViewBag.processApprove == false;
                model.Status = StatusTransactionName.Draft;
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, string status)
        {
            var model = await client.Get<List<Pca>>($"{url}/GetList/{status}") ?? new List<Pca>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        //public async Task<IActionResult> ReadApproval([DataSourceRequest] DataSourceRequest request)
        //{
        //    var model = await client.Get<List<Pca>>($"{url}/approvalList") ?? new List<Pca>();
        //    var data = model.ToDataSourceResult(request);
        //    return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        //}

        [HttpPut]
        public async Task<IActionResult> Approve(Pca model)
        {
            model.StatusApproval = StatusTransaction.Approved;
            var response = await client.PutApiResponse<Pca>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPut]
        public async Task<IActionResult> Reject(Pca model)
        {
            model.StatusApproval = StatusTransaction.Rejected;
            var response = await client.PutApiResponse<Pca>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(Pca model)
        {
            if (model.StatusApproval == StatusTransaction.Rejected)
            {
                model.Revision = (int.Parse(model.Revision ?? "0") + 1).ToString();
            }
            model.StatusApproval = StatusTransaction.WaitingApproval;
            return await Process(model, false);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Pca model)
        {
            //if (model.Status == StatusTransaction.Rejected)
            //{
            //    model.Revision = (int.Parse(model.Revision ?? "0") + 1).ToString();
            //}
            model.StatusApproval = StatusTransaction.Draft;
            return await Process(model, true);
        }

        private async Task<IActionResult> Process(Pca model, bool isDraft)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<Pca>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<Pca>(url, model);
            }

            if (isDraft)
            {
                var data = response.Data as Pca;
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
        public async Task<IActionResult> Delete(Pca model)
        {
            var response = await client.DeleteApiResponse<Pca>($"{url}/{model.Id}");
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });

        }

        public async Task<IActionResult> ReadTask([DataSourceRequest] DataSourceRequest request, string pceId)
        {
            var model = await client.Get<List<PcaTask>>($"{url}/PcaTaskList/{pceId}") ?? new List<PcaTask>();
            return Json(model.ToTreeDataSourceResult(request), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public IActionResult CreateTask([DataSourceRequest] DataSourceRequest request, PcaTask model)
        {
            if (model != null && ModelState.IsValid)
            {
                model.Id = Guid.NewGuid().ToString();
            }
            return Json(new[] { model }.ToTreeDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public IActionResult UpdateTask([DataSourceRequest] DataSourceRequest request, PcaTask model)
        {
            if (model != null && ModelState.IsValid)
            {
            }
            return Json(new[] { model }.ToTreeDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public IActionResult DeleteTask([DataSourceRequest] DataSourceRequest request, PcaTask model)
        {
            if (model != null)
            {
            }
            return Json(new[] { model }.ToTreeDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetClient(string jobid)
        {
            var isPMTeam = await client.Get<bool>($"{ApiUrl.PMTeamUrl}/IsPMTeam/{jobid}");
            var getUrl = $"{url}/GetClientByJobId/{jobid}";
            var model = await client.Get<Models.JobDetail>(getUrl) ?? new Models.JobDetail();
            return Json(new { model.CompanyId, model.CompanyName, model.BrandName, model.MainServiceCategory, isPMTeam }, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetMotherPca(string motherPcaId)
        {
            var getUrl = $"{ApiUrl.MotherPCAUrl}/{motherPcaId}";
            var model = await client.Get<Models.MotherPca>(getUrl) ?? new Models.MotherPca();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetTaskRateCard(string rateCardId, string taskRateCardName)
        {
            var getUrl = $"{ApiUrl.RateCardUrl}/GetTaskRateCard/{rateCardId}/{taskRateCardName}";
            var model = await client.Get<TaskRateCard>(getUrl) ?? new TaskRateCard();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetSubTaskRateCard(string rateCardId, string taskRateCardName, string subTaskRateCardName)
        {
            var getUrl = $"{ApiUrl.RateCardUrl}/GetSubTaskRateCard/{rateCardId}/{taskRateCardName}/{subTaskRateCardName}";
            var model = await client.Get<SubTaskRateCard>(getUrl) ?? new SubTaskRateCard();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPut]
        public IActionResult CA(Pca model)
        {
            if (model.PcaTasks.Where(x => x.TotalPriceBalance == 0).Count() > 0)
            {
                var taskName = string.Join(", ", model.PcaTasks.Where(x => x.TotalPriceBalance == 0).Select(x => x.TaskName).ToArray());
                throw new Exception($"for tasks {taskName} cannot be processed because there is no budget");
            }
            else
            {
                GlobalCache.SetPCA(model);
                return Json(new { success = true, url = Url.Action("Detail", "CashAdvance", new { area = "Finance" }) });
            }
        }

        [HttpPut]
        public async Task<IActionResult> RFQ(Pca model, IFormFile file)
        {
            //if (model.PcaTasks.Where(x => x.TotalPriceBalance == 0).Count() > 0)
            //{
            //    var taskName = string.Join(", ", model.PcaTasks.Where(x => x.TotalPriceBalance == 0).Select(x => x.TaskName).ToArray());
            //    throw new Exception($"for tasks {taskName} cannot be processed because there is no budget");
            //}
            //else
            //{ 
            var response = await client.PutApiResponse<RequestForQuotation>($"{url}/RFQ", model, file);
            var data = response.Data as RequestForQuotation;
            return Json(new { success = response.Success, message = response.Message, url = Url.Action("AddEdit", "PCA", new { area = "PM", Id = model.Id }) });
            //}
        }

        [AcceptVerbs("Post")]
        public IActionResult CreateRFQ([DataSourceRequest] DataSourceRequest request, PcaTaskRequest model)
        {
            return Json(new[] { model }.ToTreeDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}