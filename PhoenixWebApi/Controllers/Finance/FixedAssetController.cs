using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Finance.Transaction;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Finance.Transaction;

namespace PhoenixWebApi.Controllers.Finance
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class FixedAssetController : ControllerBase
    {
        readonly IFixedAssetService service;

        public FixedAssetController(IFixedAssetService service)
        {
            this.service = service;
        }

        [HttpGet]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get()
        {
            var result = await service.Get();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string Id)
        {
            var result = await service.Get(Id);
            return Ok(new ApiResponse(result));
        }
        
        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromForm]IFormCollection collection)
        {
            var model = JsonConvert.DeserializeObject<FixedAsset>(collection[nameof(FixedAsset)].ToString());

            if (collection.Count == 0 || !collection.Keys.Contains(nameof(FixedAsset)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            if (collection.Files.Count() == 1)
            {
                model.file = collection.Files[0];
            }

            await service.AddEdit(model, false);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromForm]IFormCollection collection)
        {
            var model = JsonConvert.DeserializeObject<FixedAsset>(collection[nameof(FixedAsset)].ToString());
            
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(FixedAsset)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            if (collection.Files.Count() == 1)
            {
                model.file = collection.Files[0];
            }

            await service.AddEdit(model, true);
            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        //[HttpPut]
        //[ActionRole(ActionMethod.Edit)]
        //public async Task<IActionResult> Put([FromBody]FixedAsset entity)
        //{
        //    await service.AddEdit(entity, true);
        //    return Ok(new ApiResponse(true, Messages.SuccessEdit));
        //}

        [HttpDelete("{Id}")]
        [ActionRole(ActionMethod.Delete)]
        public async Task<IActionResult> Delete(string Id)
        {
            var model = await service.Get(Id);
            await service.DeleteAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessDelete));
        }


        [HttpGet("ListFixedAssetByLocationId/{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> ListFixedAssetByLocationId(string Id)
        {
            var result = await service.GetListFixedAssetByLocationId(Id);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("ListFixedAssetOutByLocationId/{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> ListFixedAssetOutByLocationId(string Id)
        {
            var result = await service.GetListFixedAssetOutByLocationId(Id);
            return Ok(new ApiResponse(result));
        }

 
    }
}