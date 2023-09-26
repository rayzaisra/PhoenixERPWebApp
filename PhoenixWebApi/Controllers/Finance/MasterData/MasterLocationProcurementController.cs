using Microsoft.AspNetCore.Mvc;
using Phoenix.Data;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Finance;
using System.Threading.Tasks;

namespace PhoenixWebApi.Controllers.Finance.Master
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class MasterLocationProcurementController : ControllerBase
    {
        readonly IMasterLocationProcurementService service;

        public MasterLocationProcurementController(IMasterLocationProcurementService service)
        {
            this.service = service;
        }

        // GET: api/MasterLocationProcurement
        [HttpGet]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get()
        {
            var result = await service.Get();
            return Ok(new ApiResponse(result));
        }

        // GET: api/MasterLocationProcurement/5
        [HttpGet("{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string Id)
        {
            var result = await service.Get(Id);
            return Ok(new ApiResponse(result));
        }

        // POST: api/MasterLocationProcurement
        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromBody]MasterLocationProcurement model)
        {
            await service.AddAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        // PUT: api/MasterLocationProcurement/5
        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromBody]MasterLocationProcurement model)
        {
            await service.EditAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{Id}")]
        [ActionRole(ActionMethod.Delete)]
        public async Task<IActionResult> Delete(string Id)
        {
            var model = await service.Get(Id);
            await service.DeleteSoftAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessDelete));
        }
    }
}
