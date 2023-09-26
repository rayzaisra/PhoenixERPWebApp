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
using Phoenix.WebExtension.Extentions;


namespace PhoenixERPWebApp.Areas.HrisNew.Controllers
{
    [Area(nameof(Hrisnew))]
    public class TrainingRegistrationPaymentReqController : Controller
    {
        string url = ApiUrl.TrainingRegistrationPaymentReqUrl;
        string urlMTR = ApiUrl.MasterTrainingRequestUrl;
        private readonly ApiClientFactory client;

        public TrainingRegistrationPaymentReqController(ApiClientFactory client)
        {
            this.client = client;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ReadTrainingRequest([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<TrainingRequisition>>($"{url}/TrainingRequest") ?? new List<TrainingRequisition>();
            int counting = 0;
            foreach (TrainingRequisition trainingRequisition in model)
            {
                counting = 0;
                foreach (TrainingRequisitionDetail jml in trainingRequisition.ParticipantList)
                {
                    counting = counting + 1;
                }
                trainingRequisition.Participant = counting;
            }
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }


        [HttpGet]
        public async Task<IActionResult> GetDetail([DataSourceRequest] DataSourceRequest request, string Id)
        {
            try
            {
                var model = await client.Get<List<TrainingRegpaymentDetail>>($"{url}/gettrainingregpaydetail/{Id}") ?? new List<TrainingRegpaymentDetail>();
                var data = model.ToDataSourceResult(request);
                return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<IActionResult> Detail(string Id, bool isApprove)
        {
            ClearCacheDetailTrainingRegPayment();
            var model = new TrainingRequisition();
            var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
            model.RequesterEmployeeBasicInfoId = requestor.EmployeeId;
            model.BusinessUnitId = requestor.DivisionId;
            if (isApprove.Equals(null))
            {
                isApprove = false;
            }

            ViewBag.Header = "Create";
            var isCurrentApproval = false;
            if (!string.IsNullOrEmpty(Id))
            {
                if (isApprove == true)
                    ViewBag.Header = "Approve";
                else
                    ViewBag.Header = "Update";

                model = await client.Get<TrainingRequisition>($"{url}/{Id}");

                isCurrentApproval = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.isRequestor = isApprove == false && (model.Status == TrainingStatus.Open);
            }

            ViewBag.processApprove = isCurrentApproval;
            model.isApprove = isApprove;
            if (isApprove)
            {
                ViewBag.isHcAdmin = true;//belum di set
            }

            return View(model);
        }

        [HttpPost]
                            
        public async Task<IActionResult> AddDetail(string Id, string MasterTrainingRequestId, DateTime startDate, DateTime endDate,string StatusRequest, string remarks)
        {
            Id = Guid.NewGuid().ToString();
            var objRequestName = new MasterTrainingRequest();
            var model = GetCacheDetailTrainingRegPayment().Where(x => x.Id != Id).ToList();
            var response = new ApiResponse();
            var obj = new TrainingRequisition();
            obj.Id = Id;

            obj.TrainingPaymentDetail.MasterTrainingRequestId = MasterTrainingRequestId;
            if (!string.IsNullOrEmpty(MasterTrainingRequestId))
            objRequestName = await client.Get<MasterTrainingRequest>($"{urlMTR}/{MasterTrainingRequestId}");
            obj.TrainingPaymentDetail.MasterTrainingRequestName = objRequestName.RequestName;
            obj.TrainingPaymentDetail.StartDate = Convert.ToDateTime(startDate);
            obj.TrainingPaymentDetail.EndDate = Convert.ToDateTime(endDate);
            obj.TrainingPaymentDetail.StatusRequest = StatusRequest;
            obj.TrainingPaymentDetail.Remarks = remarks;
          
            model.Add(obj.TrainingPaymentDetail);
            SetCacheDetailTrainingRegPayment(model);
            return Json(new { Data = model, refreshGrid = "grid_detail_reg_payment", hideModalPopUp = "modalDetail" }, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        string uniqueDetailPayment = "DETAILREGPAYMENT";
        public List<TrainingRegpaymentDetail> GetCacheDetailTrainingRegPayment()
        {
            var models = CacheStore.Get<List<TrainingRegpaymentDetail>>(uniqueDetailPayment);
            if (models == null)
                models = new List<TrainingRegpaymentDetail>();
            return models;
        }

        private void ClearCacheDetailTrainingRegPayment()
        {
            CacheStore.Remove<List<TrainingRegpaymentDetail>>(uniqueDetailPayment);
        }

        public void SetCacheDetailTrainingRegPayment(List<TrainingRegpaymentDetail> models)
        {
            ClearCacheDetailTrainingRegPayment();
            CacheStore.Add<List<TrainingRegpaymentDetail>>(uniqueDetailPayment, models);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(TrainingRequisition entity,  List<TrainingRegpaymentDetail> Details)
        {
            var response = new ApiResponse();
            if (entity != null)
            {
                TrainingRegPayment trRegPayment = new TrainingRegPayment();
                trRegPayment.Id = entity.RegPaymentId;
                trRegPayment.TrainingId = entity.TrainingId;
                trRegPayment.EmployeeBasicInfoId = entity.RequesterEmployeeBasicInfoId;
                trRegPayment.TaskName = entity.TrainingName + "-" + "Task";
                trRegPayment.JobName = entity.TrainingName + "-" + "job";
                trRegPayment.Note = entity.Notes;
                trRegPayment.Venue = entity.Venue;
                trRegPayment.Status = "2";
                trRegPayment.ApprovalId = entity.ApprovalId;
                trRegPayment.TotalAddFee = Convert.ToDecimal(entity.TrainingCost);
                trRegPayment.BusinessUnitDivisionId = entity.BusinessUnitDivisionId;
                trRegPayment.BusinessUnitDepartementId = entity.BusinessUnitDepartementId;
                trRegPayment.Code = entity.Code;
                trRegPayment.Action = "Submit";

                trRegPayment.AddRecomendation = entity.AddRecomendation;
                trRegPayment.trainingRegPaymentDeail = Details;

                if (!string.IsNullOrEmpty(entity.RegPaymentId))
                {
                    trRegPayment.Id = entity.RegPaymentId;
                    response = await client.PutApiResponse<TrainingRegPayment>(url, trRegPayment);
                }
                else
                {
                    response = await client.PostApiResponse<TrainingRegPayment>(url, trRegPayment);
                }
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public async Task<IActionResult> Save(TrainingRequisition entity, List<TrainingRegpaymentDetail> Details)
        {
            var response = new ApiResponse();
            if (entity != null)
            {

                TrainingRegPayment trRegPayment = new TrainingRegPayment();
                trRegPayment.Id = entity.TrainingPayment.Id;
                trRegPayment.TrainingId = entity.TrainingId;
                trRegPayment.EmployeeBasicInfoId = entity.RequesterEmployeeBasicInfoId;
                trRegPayment.TaskName = entity.TrainingName + "-" + "Task";
                trRegPayment.JobName = entity.TrainingName + "-" + "job";
                trRegPayment.Note = entity.Notes;
                trRegPayment.Venue = entity.Venue;
                trRegPayment.Status = "1";
                trRegPayment.ApprovalId = entity.ApprovalId;
                trRegPayment.TotalAddFee = Convert.ToDecimal(entity.TrainingCost);
                trRegPayment.BusinessUnitDivisionId = entity.BusinessUnitDivisionId;
                trRegPayment.BusinessUnitDepartementId = entity.BusinessUnitDepartementId;
               
                trRegPayment.AddRecomendation = entity.AddRecomendation;
                trRegPayment.trainingRegPaymentDeail = Details;
                trRegPayment.Code = entity.Code;
                trRegPayment.Action = "Save";
               

                if (!string.IsNullOrEmpty(entity.RegPaymentId))
                {
                    trRegPayment.Id = entity.RegPaymentId;
                    response = await client.PutApiResponse<TrainingRegPayment>(url, trRegPayment);
                }else
                {
                    response = await client.PostApiResponse<TrainingRegPayment>(url, trRegPayment);
                }
            }

            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }
    }
}