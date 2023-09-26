using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataTables.AspNet.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Finance.Transaction;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace PhoenixWebApi.Controllers.Finance
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ReconcileController : ControllerBase
    {
        readonly IReconcileService service;
        public ReconcileController(IReconcileService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpPost("GetList")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetList([FromBody] ReconcileSearch search)
        {
            var result = await service.GetList(search);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string Id)
        {
            var result = await service.Get(Id);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromBody] Reconcile model)
        {
            await service.AddAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SucessSave));
        }
    }
}
