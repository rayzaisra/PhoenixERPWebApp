using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services;

namespace PhoenixWebApi.Controllers.Media
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadMasterTemplateController : ControllerBase
    {
        readonly IUploadMasterTemplateService service;

        /// <summary>
        /// And endpoint to manage Termination
        /// </summary>
        /// <param name="context">Database context</param>
        public UploadMasterTemplateController(IUploadMasterTemplateService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm]IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(TrMediaFileMaster)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }
            var model = JsonConvert.DeserializeObject<TrMediaFileMaster>(collection[nameof(TrMediaFileMaster)].ToString());

            if (collection.Files.Count() > 0)
            {
                model.TemplateFile = collection.Files[0];
            }
            

            await service.AddAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await service.GetFileType(id);
            return Ok(new ApiResponse(result));
        }
    }
}