using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Um;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Um;

namespace PhoenixWebApi.Controllers.Um
{
    /// <summary>
    /// Leave API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/emailtemplate")]
    public class EmailTemplateController : Controller
    {
        readonly IEmailTemplateService service;

        /// <summary>
        /// And endpoint to manage Leave
        /// </summary>
        /// <param name="context">Database context</param>
        public EmailTemplateController(IEmailTemplateService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromBody]TmEmailTemplate model)
        {
            model.Id = Guid.NewGuid().ToString();
            await service.AddAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromBody]TmEmailTemplate model)
        {
            await service.EditAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }

        /// <summary>
        /// Deletes specific record based on given id parameter.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpDelete("{Id}")]
        [ActionRole(ActionMethod.Delete)]
        public async Task<IActionResult> Delete(string Id)
        {
            var model = await service.Get(Id);
            await service.DeleteSoftAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessDelete));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get()
        {
            var result = await service.Get();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string Id)
        {
            var result = await service.Get(Id);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [AllowAnonymousRole]
        [HttpGet]
        [ActionRole(ActionMethod.Read)]
        [Route("getlisttemplate")]
        public async Task<IActionResult> GetListTemplate()
        {
            var result = await service.GetListTemplateName();
            return Ok(new ApiResponse(result));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [ActionRole(ActionMethod.Read)]
        [Route("gettemplatebyid/{id}")]
        public async Task<IActionResult> GetTemplateById(string id)
        {
            var result = await service.Get(id);
            return Ok(new ApiResponse(result));
        }

    }
}