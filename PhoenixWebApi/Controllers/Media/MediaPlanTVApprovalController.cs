using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Media.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixWebApi.Controllers.Media
{
    [Produces("application/json")]
    [Route("api/mediaplantvapproval")]
    public class MediaPlanTVApprovalController : Controller
    {
        private readonly IMediaPlanTVApprovalService _repo;
        public MediaPlanTVApprovalController(IMediaPlanTVApprovalService repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _repo.GetMediaPlanTVsApproval();
            return Ok(new ApiResponse(result));
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            var result = await _repo.Get(Id);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getDetailList/{headerId}")]
        public async Task<IActionResult> GetDetailList(string headerId)
        {
            var result = await _repo.GetMediaPlanTVDetails(headerId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetDetail/{Id}")]
        public async Task<IActionResult> GetDetail(string Id)
        {
            var result = await _repo.GetDetail(Id);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetScheduleList/{Id}")]
        public async Task<IActionResult> GetSchedule(string Id)
        {
            var result = await _repo.GetDetailScheduleList(Id);
            return Ok(new ApiResponse(result));
        }
    }
}
