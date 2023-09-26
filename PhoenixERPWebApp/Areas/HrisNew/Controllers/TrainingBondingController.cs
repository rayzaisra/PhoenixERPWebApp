using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.Enum;

namespace PhoenixERPWebApp.Areas.Hrisnew.Controllers
{
    [Area(nameof(Hrisnew))]
    public class TrainingBondingController : Controller
    {
        string url = ApiUrl.TrainingBondingUrl;
        private readonly ApiClientFactory client;

        public TrainingBondingController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(string Id, bool isApprove)
        {
            var model = new TrainingBonding();
            if (isApprove.Equals(null))
            {
                isApprove = false;
            }
            ViewBag.Header = "Create";
            var isCurrentApproval = false;


            var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
            if (string.IsNullOrEmpty(model.EmployeeBasicInfoId))
            {
                model.EmployeeBasicInfoId = requestor.EmployeeId;
            }
            if (string.IsNullOrEmpty(model.BusinessUnitId))
            {
                model.BusinessUnitId = requestor.BusinessUnitId;
            }
            
            if (!string.IsNullOrEmpty(Id))
            {
                if (isApprove == true)
                    ViewBag.Header = "Approve";
                else
                ViewBag.Header = "Update";

                model = await client.Get<TrainingBonding>($"{url}/{Id}");


                if (!StatusTransaction.StatusName(int.Parse(model.Status)).Equals(StatusApproved.Approved.ToString()) && model.EmployeeBasicInfoId == requestor.EmployeeId && !StatusTransaction.StatusName(int.Parse(model.Status)).Equals(StatusApproved.Rejected.ToString())) {
                    isCurrentApproval = true;
                }
                ViewBag.isRequestor = isApprove == false;
            }

            

            ViewBag.processApprove = isApprove && isCurrentApproval;//(model.Status == StatusTransaction.WaitingApproval || model.Status == StatusTransaction.CurrentApproval);
            model.isApprove = isApprove;
            if (isApprove)
            {
                ViewBag.isHcAdmin = true;//belum di set
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<TrainingBonding>>(url) ?? new List<TrainingBonding>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadTrainingRequest([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<TrainingRequisition>>($"{url}/TrainingRequest") ?? new List<TrainingRequisition>();
            int counting = 0;
            foreach (TrainingRequisition trainingRequisition in model) {
                counting = 0;
                foreach (TrainingRequisitionDetail jml in trainingRequisition.ParticipantList) {
                    counting = counting + 1;
                }
                trainingRequisition.Participant = counting;
            }
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadDetail([DataSourceRequest] DataSourceRequest request, string Id)
        {
             var model = new List<TrainingRequisitionDetail>();
            if (!string.IsNullOrWhiteSpace(Id))
            {
                model = await client.Get<List<TrainingRequisitionDetail>>($"{ApiUrl.TrainingRequisitionDetailUrl}/GetDetailWithIdRequest/{Id}") ?? new List<TrainingRequisitionDetail>();
            }
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        static T CastResponseData<T>(object entity) where T : class
        {
            return entity as T;
        }
        public async Task<IActionResult> ReadEmployeeChoiceList([DataSourceRequest] DataSourceRequest request, TrainingRequisitionDetailFilter Filter)
        {
            var response = new ApiResponse(); 
            List<TrainingRequisitionDetail> model = new List<TrainingRequisitionDetail>();
            if (string.IsNullOrEmpty(Filter.FilterGroupId)) {
                Filter.FilterGroupId = "0";
            }
            if (string.IsNullOrEmpty(Filter.FilterSubgroupId))
            {
                Filter.FilterSubgroupId = "0";
            }
            if (string.IsNullOrEmpty(Filter.FilterDivisionId))
            {
                Filter.FilterDivisionId = "0";
            }
            if (string.IsNullOrEmpty(Filter.FilterDepartementId))
            {
                Filter.FilterDepartementId = "0";
            }
            response = await client.PostApiResponse<List<TrainingRequisitionDetail>>($"{ApiUrl.TrainingRequisitionDetailUrl}/GetEmployeeListWithFilter/", Filter);
            if (response.Data != null)
            {
                model = CastResponseData<List<TrainingRequisitionDetail>>(response.Data);
            }

            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Save(TrainingRequisition model, List<TrainingRequisitionDetail> Details)
        {
            var response = new ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                if (Details.Count <= 0)
                {
                    return Json(new { success = false, message = "Please Enter Participant" });
                }
                else
                {
                    if (string.IsNullOrEmpty(model.Id))
                    {
                        model.Status = TrainingStatus.Open;
                    }
                    if (string.IsNullOrEmpty(model.Status))
                    {
                        model.Status = TrainingStatus.Open;
                    }
                    return await Submit(model, Details);
                }
             
            }
            else
            {

                if (string.IsNullOrEmpty(model.Id))
                {
                    model.Status = TrainingStatus.Open;
                }
                if (string.IsNullOrEmpty(model.Status))
                {
                    model.Status = TrainingStatus.Open;
                }
                return await Submit(model, Details);

            }
        }

        [HttpPost]
        public async Task<IActionResult> Submit(TrainingRequisition model, List<TrainingRequisitionDetail> Details)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            //check date
            if (model.PurposeEndDate < model.PurposeStartDate)
            {
                throw new Exception("Purpuse End Date Training Cannot More Than Purpose Start Date Training");
            }

            var training = new TrainingRequisitionDTO();
            var trainingheader = new TrainingRequisition();
            trainingheader = model;
            var trainingdetail = new List<TrainingRequisitionDetail>();
            trainingdetail = Details;
            training.Header = trainingheader;
            training.Details = trainingdetail;
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<TrainingRequisitionDTO>($"{ApiUrl.TrainingRequisitionUrl}/EditWithDetail/", training);
            }
            else
            {
                response = await client.PostApiResponse<TrainingRequisitionDTO>($"{ApiUrl.TrainingRequisitionUrl}/AddWithDetail/", training);
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public async Task<IActionResult> Approve(TrainingBonding model)
        {
            model.Status = TrainingStatus.Approved;
            var response = await client.PutApiResponse<TrainingBonding>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public async Task<IActionResult> Reject(TrainingBonding model)
        {
            model.Status = TrainingStatus.Reject;
            var response = await client.PutApiResponse<TrainingBonding>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(TrainingRequisition model) => Json(await client.DeleteApiResponse<TrainingRequisition>($"{url}/{model.Id}"));
    }
}