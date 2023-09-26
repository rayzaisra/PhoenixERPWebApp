using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Media;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Media;

namespace PhoenixWebApi.Controllers.Media
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaOrderTVController : ControllerBase
    {
        readonly IMediaOrderTVService service;

        /// <summary>
        /// And endpoint to manage Termination
        /// </summary>
        /// <param name="context">Database context</param>
        public MediaOrderTVController(IMediaOrderTVService service)
        {
            this.service = service;
        }
        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await service.GetList();
            return Ok(new ApiResponse(result));
        }
        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetApprovalList")]
        public async Task<IActionResult> GetApprovalList()
        {
            var result = await service.GetApprovalList();
            return Ok(new ApiResponse(result));
        }
        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            var result = await service.Get(Id);
            return Ok(new ApiResponse(result));
        }
        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("GetDetail/{Id}")]
        public async Task<IActionResult> GetDetail(string Id)
        {
            var result = await service.GetDetail(Id);
            return Ok(new ApiResponse(result));
        }
        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("GetDetailList/{Id}")]
        public async Task<IActionResult> GetDetailList(string Id)
        {
            var result = await service.GetDetailList(Id);
            return Ok(new ApiResponse(result));
        }
        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("GetScheduleList/{Id}")]
        public async Task<IActionResult> GetSchedule(string Id)
        {
            var result = await service.GetDetailScheduleList(Id);
            return Ok(new ApiResponse(result));
        }
        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("GetMediaPlanTVHeader/{Id}")]
        public async Task<IActionResult> GetMediaPlanTV(string Id)
        {
            var result = await service.GetMediaPlanTV(Id);
            return Ok(new ApiResponse(result));
        }
        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("GetMediaPlanTVDetail/{Id}")]
        public async Task<IActionResult> GetMediaPlanTVDetail(string Id)
        {
            var result = await service.GetMediaPlanTVDetail(Id);
            return Ok(new ApiResponse(result));
        }
        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("GetTemplate/{Id}")]
        public async Task<IActionResult> GetTemplate(string Id)
        {
            var result = await service.GetMediaPlanTVDetail(Id);
            return Ok(new ApiResponse(result));
        }
        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="collection">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost]
        public async Task<IActionResult> Post(MediaOrderTVDTO model)
        {
            var header = model.Header;
            var details = model.Details.ToArray();
            var planDetails = await service.GetMediaPlanTVDetail(header.TrMediaPlanTvId);
            var orderDetailSchedules = new List<TrMediaOrderTVDetailSchedule>();

            await service.AddAsync(header);

            foreach(var detail in details) { 
                detail.Id = Guid.NewGuid().ToString();
                detail.TrMediaOrderId = header.Id;
                foreach (var planDetail in planDetails)
                {
                    var planSchedules = service.GetMediaPlanTVDetailSchedule(planDetail.Id);
                    foreach(var planSchedule in planSchedules) { 
                        if (planSchedule.TrMediaPlanDetailId == detail.TrMediaPlanDetailId)
                        {
                            orderDetailSchedules.Add(new TrMediaOrderTVDetailSchedule
                            {
                                Id = Guid.NewGuid().ToString(),
                                TrMediaOrderDetailId = detail.Id,
                                Bulan = planSchedule.Bulan,
                                Tanggal = planSchedule.Tanggal,
                                Tahun = planSchedule.Tahun,
                                SpotValue = planSchedule.SpotValue
                            });
                        }
                    };
                }
            }

            await service.AddRangeDetailAsync(details);
            
            await service.AddRangeDetailScheduleAsync(orderDetailSchedules.ToArray());

            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="collection">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut]
        public async Task<IActionResult> Put(MediaOrderTVDTO model)
        {
            var header = model.Header;
            var details = model.Details.ToArray();
            await service.EditAsync(header);
            await service.EditRangeDetailAsync(details);
            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        /// <summary>
        /// Deletes specific record based on given id parameter.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await service.Get(id);
            await service.DeleteSoftAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessDelete));
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="collection">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(TrMediaOrderTV model)
        {
            if (model.TmMediaOrderStatusId == MediaOrderStatusName.Approved)
            {
                await service.ApproveAsync(model);
            }
            else
            {
                await service.EditAsync(model);
            }
            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }
    }
}