using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.ProjectManagement;

namespace PhoenixERPWebApp.Areas.Pm.Controllers
{
    [Area(nameof(Pm))]
    public class TimeSheetActivatedController : Controller
    {
        string url = ApiUrl.TimeSheetUrl;
        string urlApproval = ApiUrl.ManageTransApproval;
        private readonly ApiClientFactory client;
        public TimeSheetActivatedController(ApiClientFactory _client)
        {
            this.client = _client;
        }

        public async Task<IActionResult> Index()
        {
            Timesheet data = new Timesheet();
            string dateToday;

            dateToday = DateTime.Now.ToString("yyyy-MM-dd");

            ViewBag.IsUserLoginApprovalStatus = "false";

            var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
            ViewBag.Name = requestor.EmployeeName;
            ViewBag.BusineesUnit = requestor.BusinessUnitId;
            ViewBag.Division = requestor.Division;
            ViewBag.DivisionId = requestor.DivisionId;

            ViewBag.CreatedOn = dateToday;
            var dt = await client.Get($"{url}/ListTimeSheet/{dateToday}");
            DataSet result = (DataSet)JsonConvert.DeserializeObject(dt, (typeof(DataSet)));
            var timeSheetId = result.Tables[1].Rows[0]["TimesheetHeaderId"].ToString();
            var status = result.Tables[1].Rows[0]["Status"].ToString();
            data = new Timesheet()
            {
                id = timeSheetId,
                Status = status,
                ApprovalId = ""
            };

            var IsLocked = await client.Get($"{url}/CheckIsUserLocked/{requestor.EmployeeId}");
            var result2 = JsonConvert.DeserializeObject<GeneralResponse>(IsLocked);
            ViewBag.IsLocked = result2.data;


            return View(data);
        }

        public async Task<DataTable> GetDataTable(string date)
        {
            var data = await client.Get($"{url}/ListTimeSheet/{date}");
            DataSet result = (DataSet)JsonConvert.DeserializeObject(data, (typeof(DataSet)));
            return result.Tables[0];
        }

        public async Task<DataTable> GetDataTableByID(string id)
        {
            var data = await client.Get($"{url}/GetTimeSheetDataById/{id}");
            DataSet result = (DataSet)JsonConvert.DeserializeObject(data, (typeof(DataSet)));
            return result.Tables[1];
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, string date)
        {
            DataTable data = await GetDataTable(date);
            string json = JsonConvert.SerializeObject(data);
            List<Timesheet> convertData = JsonConvert.DeserializeObject<List<Timesheet>>(json);
            var result = convertData.ToDataSourceResult(request);
            return Json(result, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadById([DataSourceRequest] DataSourceRequest request, string id)
        {
            DataTable data = await GetDataTableByID(id);
            string json = JsonConvert.SerializeObject(data);
            List<Timesheet> convertData = JsonConvert.DeserializeObject<List<Timesheet>>(json);
            var result = convertData.ToDataSourceResult(request);
            return Json(result, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> Update([DataSourceRequest] DataSourceRequest request, Timesheet data, string start_date, string end_date,
            string Sunday, string Monday, string Tuesday, string Wednesday, string Thursday, string Friday, string Saturday)
        {
            data.start_date = DateTime.ParseExact(start_date, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture).Date;
            end_date = data.start_date.AddDays(6).ToString("M/d/yyyy h:mm:ss tt");
            data.end_date = DateTime.ParseExact(end_date, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture).Date;
            data.Sunday = DateTime.ParseExact(Sunday, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            data.Monday = DateTime.ParseExact(Monday, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            data.Tuesday = DateTime.ParseExact(Tuesday, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            data.Wednesday = DateTime.ParseExact(Wednesday, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            data.Thursday = DateTime.ParseExact(Thursday, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            data.Friday = DateTime.ParseExact(Friday, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            data.Saturday = DateTime.ParseExact(Saturday, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            await client.Post($"{url}/UpdateTimeSheet", data);
            var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
            if (!string.IsNullOrEmpty(ViewBag.DivisionId))
            {
                data.BusinessUnitId = ViewBag.DivisionId;
            }
            else
            {
                data.BusinessUnitId = requestor.DivisionId;
            }

            await client.Post($"{url}/UpdateTimeSheet", data);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> DateChangeApproval(string date)
        {
            var dt = await client.Get($"{url}/ListTimeSheet/{date}");
            DataSet result = (DataSet)JsonConvert.DeserializeObject(dt, (typeof(DataSet)));
            var timeSheetId = result.Tables[1].Rows[0]["TimesheetHeaderId"].ToString();
            var status = result.Tables[1].Rows[0]["Status"].ToString();
            var isUserLoginApprovalStatus = await client.Get($"{urlApproval}/IsCurrentApproval/{timeSheetId}");
            var userStatus = "";
            if (status == "2" && isUserLoginApprovalStatus.ToLower() == "true")
            {
                userStatus = "approval";
            }
            else if (status == "2" && isUserLoginApprovalStatus.ToLower() == "false")
            {
                userStatus = "close";
            }
            else
            {
                userStatus = "default";
            }
            var data = new
            {
                success = "true",
                status = userStatus,
                id = timeSheetId
            };
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> Submit(string id, string status)
        {
            var oStatus = Convert.ToInt32(status);
            SaveTimeSheetViewModel model = new SaveTimeSheetViewModel()
            {
                Id = id,
                Status = oStatus
            };
            var data = await client.Post($"{url}/SubmitTimeSheet/", model);

            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Save(SaveTimeSheetViewModel model)
        {

            var data = await client.Post($"{url}/SaveTimeSheet/", model);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> activated(SaveTimeSheetViewModel model)
        {

            var data = await client.Post($"{url}/ActivatedTimeSheet/", model);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteNonJob(Timesheet model)
        {

            var data = await client.Post($"{url}/DeleteNonJob/", model);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public class GeneralResponse {
            public bool success { get; set; }
            public string message { get; set; }
            public string code { get; set; }
            public string data { get; set; }
        }
    }
}