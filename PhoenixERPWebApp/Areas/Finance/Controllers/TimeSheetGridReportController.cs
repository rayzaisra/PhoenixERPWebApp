using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.WebNetCore;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.Finance.FinanceReport;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class TimeSheetGridReportController : BaseController
    {
        string url = ApiUrl.TimeSheetByJobUrl;
        private readonly ApiClientFactory client;

        public TimeSheetGridReportController(ApiClientFactory client)
        {
            this.client = client;
        }

        public async Task<IActionResult> Index(string startDate = null, string endDate = null, string userid = null, string divisionid = null, string isNonJob = null)
        {
            TimeSheetReportByBusinessUnit model = new TimeSheetReportByBusinessUnit();
            ViewBag.isNonJob = "0";

            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                DateTime now = DateTime.Now;
                model.StartDate = DateTime.Parse(startDate);
                model.EndDate = DateTime.Parse(endDate);
                model.EmployeeBasicInfoId = userid;
                model.DivisionId = divisionid;
            }

            if (!string.IsNullOrEmpty(isNonJob))
            {
                if (isNonJob == "1")
                {
                    ViewBag.isNonJob = "1";
                }
                else if (isNonJob == "2")
                {
                    ViewBag.isNonJob = "2";
                }
            
            }
            
            return View(model);
        }

        public async Task<IActionResult> IndexReportByBusinessUnit()
        {
            return View();
        }

        public async Task<IActionResult> ReadReportByBusinessUnit([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<TimeSheetReportByBusinessUnit>>($"{url}/GetListTimeSheetByBusinessUnit") ?? new List<TimeSheetReportByBusinessUnit>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReportByClient()
        {
            return View();
        }

        public async Task<IActionResult> ReportByBusinessUnit(string Id)
        {
            var model = new TimeSheetReportByBusinessUnit();
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<TimeSheetReportByBusinessUnit>($"{url}/ReportTimeSheetByBusinessUnit/{Id}");
                //model.Is_Default = model.IsDefault ?? false;
                //model.Is_Salary_Report = model.Is_Salary_Report ?? false;
            }

            return View(model);
        }

        public async Task<IActionResult> ReadTimeSheetByJob([DataSourceRequest] DataSourceRequest request, string jobId, string userId, string clientId, DateTime startDate, DateTime endDate, string legalEntityId, string divisionId, string isNonJob)
        {
            //string urls = url + "/get_by_job?jobId="+jobId;
            var model = await client.Get<List<TimeSheetByJob>>($"{url}/get_by_job?job_id={(jobId ?? "")}&user_id={(userId ?? "")}&client_id={(clientId ?? "")}&startDate={(startDate.ToString() ?? "")}&endDate={(endDate.ToString() ?? "")}&legalEntityId={(legalEntityId ?? "")}&divisionId={(divisionId ?? "")}&isNonJob={(isNonJob ?? "0")}");
            if (model == null)
            {
                model = new List<TimeSheetByJob>();
            }
            return Json(model.ToDataSourceResult(request), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadTimeSheetByClientId([DataSourceRequest] DataSourceRequest request, string ClientId)
        {
            //string urls = url + "/get_by_job?jobId=" + jobId;
            var model = await client.Get<List<ReportTimeSheetClient>>($"{url}/get_by_client?client_id={(ClientId ?? "0")}");
            if (model == null)
            {
                model = new List<ReportTimeSheetClient>();
            }
            return Json(model.ToDataSourceResult(request), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadTimeSheetByBusinessUnit([DataSourceRequest] DataSourceRequest request, string jobId, string clientId, DateTime startDate, DateTime endDate, string legalEntityId)
        {
            var model = await client.Get<List<TimeSheetReportByBusinessUnitDetail>>($"{url}/get_by_business_unit?jobId={(jobId ?? "0")}&clientId={(clientId ?? "0")}&startDate={(startDate.ToString() ?? "0")}&endDate={(endDate.ToString() ?? "0")}&legalEntityId={(legalEntityId ?? "0")}");
            if (model == null)
            {
                model = new List<TimeSheetReportByBusinessUnitDetail>();
            }
            return Json(model.ToDataSourceResult(request), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteReportTimeSheetByBusinessUnit(TimeSheetReportByBusinessUnit entyty) => Json(await client.DeleteApiResponse<TimeSheetReportByBusinessUnit>($"{url}/DeleteReportTimeSheetByBusinessUnit/{entyty.Id}"));

        public async Task<IActionResult> ProcessReportTimeSheetByBusinessUnit(TimeSheetReportByBusinessUnit model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<TimeSheetReportByBusinessUnit>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<TimeSheetReportByBusinessUnit>(url, model);
            }

            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(IndexReportByBusinessUnit)) });
        }
    }
}