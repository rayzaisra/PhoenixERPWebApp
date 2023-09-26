using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Media;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Media.Transaction;
using Phoenix.Data.Services.Um;
using PhoenixWebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixWebApi.Controllers.Media
{
    [Produces("application/json")]
    [Route("api/mediaplanestimates")]
    public class MediaPlanEstimatesController : ControllerBase
    {
        private readonly IMediaPlanEstimatesService _repo;
        readonly INotificationCenter notify;
        readonly ITransactionUserApprovalService trUserApproval;
        public MediaPlanEstimatesController(IMediaPlanEstimatesService repo, INotificationCenter notify, ITransactionUserApprovalService trUserApproval)
        {
            _repo = repo;
            this.notify = notify;
            this.trUserApproval = trUserApproval;
        }

        [HttpGet("getdata/{status}")]
        public async Task<IActionResult> Get(string status)
        {
            var result = await _repo.GetMediaPlanHeaders(status);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getplanheaderbyid/{Id}")]
        public async Task<IActionResult> GetMediaPlanHeaderById(string Id)
        {
            var result = await _repo.Get(Id);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetTransactionID/{caseOption}/{jobid}")]
        public async Task<IActionResult> GetTransactionID(string caseOption, string jobid)
        {
            var result = await _repo.GetMediaTransactionID(caseOption, jobid);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getdetailmediaplan/{Id}/{option}")]
        public async Task<IActionResult> GetMediaPlanDetails(string Id, string option)
        {
            var result = await _repo.GetMediaPlanDetails(Id, option);
            return Ok(new ApiResponse(result));
        }

        
        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromBody]MediaPlanHeader model)
        {
            try
            {
                model.Id = Guid.NewGuid().ToString();
                await _repo.AddAsync(model);
                if (model.StatusID != StatusTransaction.Draft.ToString())
                {
                    var listUserApproval = await trUserApproval.GetAppIdFromReferenceId(model.Id);
                    foreach (var data in listUserApproval)
                    {
                        await Task.Run(() => notify.AddNotification(NotificationTypeCode.MediaPlanRequestApproval, data.AppIdUser, NotificationModuleType.ModuleMEDIA));
                    }
                }

                return Ok(new ApiResponse(true, Messages.SuccessAdd));
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse(false, ex.Message.ToString()));
            }

        }

        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromBody]MediaPlanHeader model)
        {
            
            //return Ok(new ApiResponse(true, Messages.SuccessEdit));

            try
            {
                //model.Id = Guid.NewGuid().ToString();
                await _repo.EditAsync(model);
                if (model.StatusID != StatusTransaction.Draft.ToString())
                {
                    var listUserApproval = await trUserApproval.GetAppIdFromReferenceId(model.Id);
                    foreach (var data in listUserApproval)
                    {
                        await Task.Run(() => notify.AddNotification(NotificationTypeCode.MediaPlanRequestApproval, data.AppIdUser, NotificationModuleType.ModuleMEDIA));
                    }
                }

                return Ok(new ApiResponse(true, Messages.SuccessAdd));
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse(false, ex.Message.ToString()));
            }
        }

        [HttpPut("Approve")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Approve([FromBody]MediaPlanHeader model)
        {
            try
            {
                await _repo.Approve(model);
                return Ok(new ApiResponse(model, true, Messages.SuccessApprove));
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse(false, ex.Message.ToString()));
            }

        }

        [HttpPut("uploadreport")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> UploadReport([FromForm]IFormCollection collection)
        {
            try
            {
                if (collection.Count == 0 || !collection.Keys.Contains(nameof(MediaPlanHeader)))
                {
                    return Ok(new ApiResponse(null, false, "bad request"));
                }

                var model = JsonConvert.DeserializeObject<MediaPlanHeader>(collection[nameof(MediaPlanHeader)].ToString());
                if (collection.Files.Count() > 0)
                {
                    model.reportfile = collection.Files[0];
                }

                await _repo.UploadReport(model);
                return Ok(new ApiResponse(model, true, Messages.SuccessApprove));
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse(false, ex.Message.ToString()));
            }

        }

        [HttpPut("Finish")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Finish([FromBody]MediaPlanHeader model)
        {
            try
            {
                await _repo.Finish(model);
                return Ok(new ApiResponse(model, true, Messages.SuccessApprove));
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse(false, ex.Message.ToString()));
            }

        }

        [HttpPut("Reject")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Reject([FromBody]MediaPlanHeader model)
        {
            try
            {
                model.StatusID = StatusTransaction.CurrentApproval.ToString();// need revise
                await _repo.Reject(model);
                return Ok(new ApiResponse(model, true, Messages.SuccessReject));
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse(false, ex.Message.ToString()));
            }

        }

        [HttpPost("uploadfile")]
        public async Task<IActionResult> UploadFile([FromForm]IFormCollection collection)
        {
            try
            {
                var files = collection.Files[0];
                var result = await _repo.UploadFile(files);
                return Ok(new ApiResponse(result));
                
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse(false, ex.Message.ToString()));
            }

        }


    }
}