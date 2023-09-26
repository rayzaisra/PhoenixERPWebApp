using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Media;
using Phoenix.Data.Models.Media.Transaction.MediaOrder;
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
    [Route("api/mediaorder")]
    public class MediaOrderController : ControllerBase
    {
        private readonly IMediaOrderService _repo;
        readonly INotificationCenter notify;
        readonly ITransactionUserApprovalService trUserApproval;

        public MediaOrderController(IMediaOrderService repo, INotificationCenter notify, ITransactionUserApprovalService trUserApproval)
        {
            _repo = repo;
            this.notify = notify;
            this.trUserApproval = trUserApproval;
        }

        [HttpGet("getdata/{status}")]
        public async Task<IActionResult> Get(string status)
        {
            var result = await _repo.GetMediaOrderHeaders(status);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getorderbyid/{Id}")]
        public async Task<IActionResult> GetMediaOrderByID(string Id)
        {
            var result = await _repo.Get(Id);
            return Ok(new ApiResponse(result));
        }


        [HttpGet("getvendordata/{vendoroid}")]
        public async Task<IActionResult> GetDataVendor(string vendoroid)
        {
            var result = await _repo.GetdataVendor(vendoroid);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getmediaorderdetails/{Id}")]
        public async Task<IActionResult> GetMediaPlanDetails(string Id)
        {
            var result = await _repo.GetMediaOrderDetails(Id);
            return Ok(new ApiResponse(result));
        }

        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromBody]MediaOrderHeader model)
        {
            try
            {
                model.Id = Guid.NewGuid().ToString();
                await _repo.AddAsync(model); // add new detail order and edit header status
                if (model.StatusID != StatusTransaction.Draft.ToString())// send notif to user approval
                {
                    var listUserApproval = await trUserApproval.GetAppIdFromReferenceId(model.Id);
                    foreach (var data in listUserApproval)
                    {
                        await Task.Run(() => notify.AddNotification(NotificationTypeCode.MediaOrderRequestApproval, data.AppIdUser, NotificationModuleType.ModuleMEDIA));
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
        public async Task<IActionResult> Put([FromBody]MediaOrderHeader model)
        {
            try
            {
                await _repo.EditAsync(model); //edit header/details
                if (model.StatusID != StatusTransaction.Draft.ToString())//submit existing draft doc
                {
                    var listUserApproval = await trUserApproval.GetAppIdFromReferenceId(model.Id);
                    foreach (var data in listUserApproval)
                    {
                        await Task.Run(() => notify.AddNotification(NotificationTypeCode.MediaOrderRequestApproval, data.AppIdUser, model.Id, NotificationModuleType.ModuleMEDIA));
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
        public async Task<IActionResult> Approve([FromBody]MediaOrderHeader model)
        {
            try
            {
                await _repo.Approve(model); // set status to published
                return Ok(new ApiResponse(model, true, Messages.SuccessApprove));
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse(false, ex.Message.ToString()));
            }

        }

        [HttpPut("Finish")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Finish([FromBody]MediaOrderHeader model)
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

        [HttpPost("saveinvoice")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> SaveInvoice([FromBody]MediaOrderHeader model)
        {
            try
            {
                await _repo.SaveInvoice(model); // set status to published
                return Ok(new ApiResponse(model, true, Messages.SuccessApprove));
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse(false, ex.Message.ToString()));
            }

        }

        [HttpPut("Cancel")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Reject([FromBody]MediaOrderHeader model)
        {
            try
            {
                model.StatusID = StatusTransaction.Rejected.ToString();// need revise
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

        [HttpPut("uploadreport")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> UploadReport([FromForm]IFormCollection collection)
        {
            try
            {
                if (collection.Count == 0 || !collection.Keys.Contains(nameof(MediaOrderHeader)))
                {
                    return Ok(new ApiResponse(null, false, "bad request"));
                }

                var model = JsonConvert.DeserializeObject<MediaOrderHeader>(collection[nameof(MediaOrderHeader)].ToString());
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


    }
}