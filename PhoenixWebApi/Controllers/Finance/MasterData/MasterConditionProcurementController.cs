using Microsoft.AspNetCore.Mvc;
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
    public class MasterConditionProcurementController : ControllerBase
    {
        readonly IMasterConditionProcurementService service;

        public MasterConditionProcurementController(IMasterConditionProcurementService service)
        {
            this.service = service;
        }

        // GET: api/MasterConditionProcurement
        [HttpGet]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get()
        {
            var result = await service.Get();
            return Ok(new ApiResponse(result));
        }

        // GET: api/MasterConditionProcurement/5
        [HttpGet("{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string Id)
        {
            var result = await service.Get(Id);
            return Ok(new ApiResponse(result));
        }

        // POST: api/MasterConditionProcurement
        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromBody]MasterConditionProcurement model)
        {
            await service.AddAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        // PUT: api/MasterConditionProcurement/5
        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromBody]MasterConditionProcurement model)
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

        //private readonly DataContext _context;

        //public MasterConditionProcurementsController(DataContext context)
        //{
        //    _context = context;
        //}

        //// GET: api/MasterConditionProcurements
        //[HttpGet]
        //public IEnumerable<MasterConditionProcurement> GetMasterConditionProcurement()
        //{
        //    return _context.MasterConditionProcurement;
        //}

        //// GET: api/MasterConditionProcurements/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetMasterConditionProcurement([FromRoute] string id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var masterConditionProcurement = await _context.MasterConditionProcurement.FindAsync(id);

        //    if (masterConditionProcurement == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(masterConditionProcurement);
        //}

        //// PUT: api/MasterConditionProcurements/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutMasterConditionProcurement([FromRoute] string id, [FromBody] MasterConditionProcurement masterConditionProcurement)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != masterConditionProcurement.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(masterConditionProcurement).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MasterConditionProcurementExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/MasterConditionProcurements
        //[HttpPost]
        //public async Task<IActionResult> PostMasterConditionProcurement([FromBody] MasterConditionProcurement masterConditionProcurement)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _context.MasterConditionProcurement.Add(masterConditionProcurement);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetMasterConditionProcurement", new { id = masterConditionProcurement.Id }, masterConditionProcurement);
        //}

        //// DELETE: api/MasterConditionProcurements/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteMasterConditionProcurement([FromRoute] string id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var masterConditionProcurement = await _context.MasterConditionProcurement.FindAsync(id);
        //    if (masterConditionProcurement == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.MasterConditionProcurement.Remove(masterConditionProcurement);
        //    await _context.SaveChangesAsync();

        //    return Ok(masterConditionProcurement);
        //}

        //private bool MasterConditionProcurementExists(string id)
        //{
        //    return _context.MasterConditionProcurement.Any(e => e.Id == id);
        //}
    }
}