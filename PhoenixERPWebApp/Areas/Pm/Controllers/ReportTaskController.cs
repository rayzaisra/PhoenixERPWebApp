using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Kendo.Mvc.Extensions;
using System;
using Microsoft.AspNetCore.Http;
using PhoenixERPWebApp.Models.Pm.Report;

namespace PhoenixERPWebApp.Areas.Pm.Controllers
{
    [Area(nameof(Pm))]
    public class ReportTaskController : Controller
    {
        string url = ApiUrl.ReportTask;
        private readonly ApiClientFactory client;
        public ReportTaskController(ApiClientFactory client)
        {
            this.client = client;
        }
        public async Task<IActionResult> Index(string jobId = null, string startDate = null, string endDate = null, string taskName = null, string userTask = null, string divisionId = null, string legalEntityId = null)
        {
            ParamSearchTaskListViewModel model = new ParamSearchTaskListViewModel();
            if (!string.IsNullOrEmpty(startDate))
            {
                model.startDate = DateTime.Parse(startDate);
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                model.endDate = DateTime.Parse(endDate);
            }

            model.jobId = jobId;
            model.taskName = taskName;
            model.userTask = userTask;
            model.divisionId = divisionId;
            model.legalEntityId = legalEntityId;


            return View(model);
        }

        public async Task<IActionResult> ReadTask([DataSourceRequest] DataSourceRequest request, DateTime? startDate , DateTime? endDate, string jobId = null, string taskName = null, string userTask = null, string divisionId = null, string legalEntityId = null)
        {
            //string urls = url + "/get_by_job?jobId="+jobId;
            DateTime? StartDate = null;
            DateTime? EndDate = null;
            if (startDate != null)
            {
                StartDate = startDate.Value;
            }

            if (endDate != null)
            {
                EndDate = endDate.Value;
            }

            
            var model = await client.Get<List<ShowTaskViewModel>>($"{url}/GetListTask?jobId={(jobId ?? "")}&startDate={(StartDate != null ? StartDate.Value.ToString() : "")}&endDate={(EndDate != null ? EndDate.Value.ToString() : "")}&taskName={(taskName ?? "")}&userTask={(userTask ?? "")}&divisionId={(divisionId ?? "")}&legalEntityId={(legalEntityId ?? "")}");
            if (model == null)
            {
                model = new List<ShowTaskViewModel>();
            }
            return Json(model.ToDataSourceResult(request), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }


        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
    }
}
