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
    public class TimeSheetController : Controller
    {
        string url = ApiUrl.TimeSheetUrl;
        string urlApproval = ApiUrl.ManageTransApproval;
        private readonly ApiClientFactory client;

        public TimeSheetController(ApiClientFactory _client)
        {
            this.client = _client;
        }
        public async Task<IActionResult> Index(string id = null,string date = null)

        {
            Timesheet data = new Timesheet();
            string dateToday;
            string timeSheetId = "";
            string status = "";
            dateToday = DateTime.Now.ToString("yyyy-MM-dd");
            if (!string.IsNullOrEmpty(date))
            {
                dateToday = date;
            }

            ViewBag.DateToday = dateToday;
            ViewBag.IsUserLoginApprovalStatus = "false";
            if (string.IsNullOrEmpty(id))
            {
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                ViewBag.Name = requestor.EmployeeName;
                ViewBag.BusineesUnit = requestor.BusinessUnitId;
                ViewBag.Division = requestor.Division;
                ViewBag.DivisionId = requestor.DivisionId;
                ViewBag.CreatedOn = dateToday;
                ViewBag.Email = requestor.Email;
                var dt = await client.Get($"{url}/ListTimeSheet/{dateToday}");
                DataSet result = (DataSet)JsonConvert.DeserializeObject(dt, (typeof(DataSet)));
                timeSheetId = result.Tables[1].Rows[0]["TimesheetHeaderId"].ToString();
                status = result.Tables[1].Rows[0]["Status"].ToString();
                data = new Timesheet()
                {
                    id = timeSheetId,
                    Status = status,
                    ApprovalId = ""
                };
                ViewBag.IsUserLoginApprovalStatus = "false";
                ViewBag.IsCreatorTimesheet = "true";
            }
            else
            {
                var isUserLoginApprovalStatus = await client.Get($"{urlApproval}/IsCurrentApproval/{id}");
                var userLogin = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                var dt = await client.Get($"{url}/GetTimeSheetDataById/{id}");
                DataSet result = (DataSet)JsonConvert.DeserializeObject(dt, (typeof(DataSet)));
                ViewBag.Name = result.Tables[0].Rows[0]["EmployeeName"].ToString();
                ViewBag.BusineesUnit = result.Tables[0].Rows[0]["BusinessUnitId"].ToString();
                ViewBag.Division = result.Tables[0].Rows[0]["division"].ToString();
                ViewBag.DivisionId = result.Tables[0].Rows[0]["divisionId"].ToString();
                ViewBag.CreatedOn = result.Tables[0].Rows[0]["createdOn"].ToString();
                ViewBag.Email = result.Tables[0].Rows[0]["email"].ToString();
                ViewBag.IsCreatorTimesheet = "false";
                if (ViewBag.Email == userLogin.Email)
                {
                    ViewBag.IsCreatorTimesheet = "true";
                }
                if (result.Tables[2].Rows.Count > 0)
                {
                    timeSheetId = result.Tables[2].Rows[0]["TimesheetHeaderId"].ToString();
                    status = result.Tables[2].Rows[0]["Status"].ToString();
                }
                

                data = new Timesheet()
                {
                    id = timeSheetId,
                    Status = status,
                    ApprovalId = id
                };
                ViewBag.IsUserLoginApprovalStatus = isUserLoginApprovalStatus;

            }
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
            //return RedirectToAction("Index", "TimeSheet", new { id = "", date = date });
            return Json(data);
        }

        public async Task<IActionResult> Submit(string id, string status)
        {
            SaveTimeSheetViewModel model = new SaveTimeSheetViewModel()
            {
                Id = id,
                Status = 2
            };
            var data = await client.Post($"{url}/SubmitTimeSheet/", model);

            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Save(SaveTimeSheetViewModel model)
        {
            model.Status = 0;
            var data = await client.Post($"{url}/SaveTimeSheet/", model);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNonJob(Timesheet model)
        {

            var data = await client.Post($"{url}/DeleteNonJob/", model);
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> Approval(SaveTimeSheetViewModel model)
        {
            model.Status = 3;
            var data = await client.Post($"{url}/Approval/", model);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Reject(SaveTimeSheetViewModel model)
        {
            model.Status = 4;
            var data = await client.Post($"{url}/Approval/", model);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });

        }

        public IActionResult IndexApproval()
        {
            return View();
        }

        public async Task<IActionResult> ReadApproval([DataSourceRequest] DataSourceRequest request)
        {
            //var model = await client.Get<List<TimesheetApprovalList>>($"{url}/approvalList") ?? new List<TimesheetApprovalList>();
            var data = await client.Get($"{url}/approvalList");
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(data, (typeof(DataTable)));
            //var data = model.ToDataSourceResult(request);
            string json = JsonConvert.SerializeObject(dt);
            List<TimesheetApprovalList> convertData = JsonConvert.DeserializeObject<List<TimesheetApprovalList>>(json);
            foreach (var item in convertData)
            {
                if (item.StatusApprove != null)
                {
                    if (item.StatusApprove == 0)
                    {
                        item.StatusDesc = StatusTransaction.StatusName(StatusTransaction.Draft);
                    } else if (item.StatusApprove == 1)
                    {
                        item.StatusDesc = StatusTransaction.StatusName(StatusTransaction.CurrentApproval);
                    } else if (item.StatusApprove == 2)
                    {
                        item.StatusDesc = StatusTransaction.StatusName(StatusTransaction.WaitingApproval);
                    } else if (item.StatusApprove == 3)
                    {
                        item.StatusDesc = StatusTransaction.StatusName(StatusTransaction.Approved);
                    }
                    else if (item.StatusApprove == 4)
                    {
                        item.StatusDesc = StatusTransaction.StatusName(StatusTransaction.Rejected);
                    }
                    else if (item.StatusApprove == 5)
                    {
                        item.StatusDesc = StatusTransaction.StatusName(StatusTransaction.Revised);
                    }

                }
            }
            var result = convertData.ToDataSourceResult(request);
            return Json(result, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }


    }
}