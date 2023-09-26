using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Attributes;
using Phoenix.Data.Services.Finance.Transaction;
using Phoenix.Data.RestApi;
using Phoenix.Data.Models.Finance.Transaction.GoodReturn;

namespace PhoenixWebApi.Controllers.Finance
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsReturnController : ControllerBase
    {
        private readonly IGoodsReturnServices services;
        public GoodsReturnController(IGoodsReturnServices goodsReturnServices)
        {
            this.services = goodsReturnServices;
        }

        [HttpGet]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get()
        {
            var result = await services.Get();
            return Ok(new ApiResponse(result));
        }


        [HttpGet("{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string Id)
        {
            var result = await services.Get(Id);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromBody]InventoryGoodsReturn model)
        {

            await services.EditWithDetailAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }


        [HttpPut("Approve")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Approve([FromBody]InventoryGoodsReturn model)
        {
            await services.Approve(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessApprove));
        }

    }
}