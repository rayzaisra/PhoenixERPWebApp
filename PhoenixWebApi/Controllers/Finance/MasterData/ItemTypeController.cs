using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;

namespace PhoenixWebApi.Controllers.Finance
{

    [Produces("application/json")]
    [Route("api/itemtype")]
    public class ItemTypeController : Controller
    {
        readonly IItemTypeService service;

        public ItemTypeController(IItemTypeService service)
        {
            this.service = service;
        }
        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromBody]ItemType model)
        {
            await service.AddAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromBody]ItemType model)
        {
            await service.EditAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        [HttpDelete("{Id}")]
        [ActionRole(ActionMethod.Delete)]
        public async Task<IActionResult> Delete(string Id)
        {
            var model = await service.GetAsync(Id);
            await service.DeleteSoftAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessDelete));
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
            var result = await service.GetAsync(Id);
            return Ok(new ApiResponse(result));
        }
    }
}