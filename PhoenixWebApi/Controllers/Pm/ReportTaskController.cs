using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using Phoenix.Data;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Pm.ViewModel;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Pm.Report;

using PhoenixWebApi;

namespace PhoenixWebApi.Controllers.Pm
{
    [Produces("application/json")]
    [Route("api/pm/[controller]")]
    public class ReportTaskController : Controller
    {
        readonly IShowTaskServices service;
        public ReportTaskController(IShowTaskServices service)
        {
            this.service = service;
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetListTask")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetListTask(string jobId, DateTime? startDate, DateTime? endDate, string taskName , string userTask , string divisionId , string legalEntityId )
        {
            ParamSearchTaskListViewModel model = new ParamSearchTaskListViewModel();
            model.startDate = startDate;
            model.endDate = endDate;
            model.jobId = jobId;
            model.taskName = taskName;
            model.userTask = userTask;
            model.divisionId = divisionId;
            model.legalEntityId = legalEntityId;

            var result = await service.GetShowTask(model);
            return Ok(new ApiResponse(result));
        }
    }
}
