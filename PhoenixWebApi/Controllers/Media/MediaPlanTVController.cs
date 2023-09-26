using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Media.Transaction.MediaPlanTV;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Media.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixWebApi.Controllers.Media
{
    [Produces("application/json")]
    [Route("api/mediaplantv")]
    public class MediaPlanTVController : Controller
    {
        private readonly IMediaPlanTVService _repo;
        public MediaPlanTVController(IMediaPlanTVService repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _repo.GetMediaPlanTVs();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            var result = await _repo.Get(Id);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getTemplateId")]
        public async Task<IActionResult> GetTemplateId(string Id)
        {
            var result = await _repo.GetMediaPlanTvFileTemplateId();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getDetail/{headerId}")]
        public async Task<IActionResult> GetDetail(string headerId)
        {
            var result = await _repo.GetMediaPlanTVDetails(headerId);
            return Ok(new ApiResponse(result));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm]IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains("MediaPlanTV"))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }
            var model = JsonConvert.DeserializeObject<TrMediaPlanTV>(collection["MediaPlanTV"].ToString());
            var file = collection.Files.GetFile("files");
            var result = await _repo.ReadExcel(file);
            var mediaPlan = new TrMediaPlanTV();

            if (model.Id == null)
            {
                mediaPlan = await _repo.AddDetailAsync(model, file, result);
            }
            else
            {
                mediaPlan = await _repo.UpdateDetailAsync(model, file, result);
            }
            
            return Ok(new ApiResponse(mediaPlan, true, Messages.SuccessAdd));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromForm]IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains("MediaPlanTV"))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }
            var model = JsonConvert.DeserializeObject<TrMediaPlanTV>(collection["MediaPlanTV"].ToString());
            await _repo.EditAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        [HttpDelete("{id}")]
        [ActionRole(ActionMethod.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await _repo.Get(id);
            await _repo.DeleteSoftAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessDelete));
        }

        [HttpPost]
        [Route("ReadExcel")]
        public async Task<IActionResult> ReadExcel([FromForm]IFormCollection collection)
        {
            var file = collection.Files.GetFile("files");
            var result = await _repo.ReadExcel(file);
            var periodCampaign = result.Tables[1].Rows[9][4].ToString();
            periodCampaign = periodCampaign.Replace(':', ' ').Replace(" ", string.Empty);
            var mediaPlanId = await _repo.GetMediaPlanTvId(periodCampaign);
            return Ok(new ApiResponse(result));
        }
    }
}
