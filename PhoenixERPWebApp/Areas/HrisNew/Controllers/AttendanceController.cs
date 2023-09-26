using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.ModelShared;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;

namespace PhoenixERPWebApp.Areas.HrisNew.Controllers
{
    [Area(nameof(HrisNew))]
    public class AttendanceController : Controller
    {	
		readonly string url = ApiUrl.AttendanceUrl;
        private readonly ApiClientFactory client;
		readonly string urlempbasic = ApiUrl.EmployeeBasic;

		public AttendanceController(ApiClientFactory client)
        {
            this.client = client;
        }

        public async Task<IActionResult> Index()
        {
			var model = await client.Get<EmployeeBasicInfo>($"{urlempbasic}/GetInfoByLogin");
			return View(model);
		}

        public IActionResult Report()
        {
            return View();
        }

        public async Task<IActionResult> View(string employeeId, int month, int year)
        {
            ViewBag.Header = "Attendance Report";
            ViewBag.EmployeeId = employeeId;
            ViewBag.Month = month;
            ViewBag.Year = year;
            var model = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getById/{employeeId}");
            return View(model);
        }

        public async Task<IActionResult> Attendance()
        {
            ViewBag.Header = "Attendance";
            var model = await client.Get<Attendance>($"{url}/getbyid");
            ViewBag.IsChecIn = model.TimeAttendance != null;
            ViewBag.IsChecOut = (!ViewBag.IsChecIn) ? true : model.TimeAttendanceOut != null;
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, string dtStart, string dtEnd, int isNotClockOut)
        {
			if (string.IsNullOrEmpty(dtStart))
				dtStart = DateTime.Today.ToString("yyyyMMdd");
			if (string.IsNullOrEmpty(dtEnd))
				dtEnd = DateTime.Today.ToString("yyyyMMdd");

			string requrl = string.Format("{0}/{1}/{2}/{3}", url, dtStart, dtEnd, isNotClockOut);
			//var model = await client.Get<List<Attendance>>(requrl) ?? new List<Attendance>();
			var model = await client.Get<List<AttendanceReport>>(requrl) ?? new List<AttendanceReport>();			
			var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadReport([DataSourceRequest] DataSourceRequest request, string month, string year, string legalEntityId)
        {
			if (string.IsNullOrEmpty(month))
				month = DateTime.Today.ToString("yyyyMMdd");
			if (string.IsNullOrEmpty(year))
				year = DateTime.Today.ToString("yyyyMMdd");

			var model = await client.Get<List<Attendance>>($"{url}/Report/{month}/{year}/{(legalEntityId ?? "")}") ?? new List<Attendance>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadAttendanceReport([DataSourceRequest] DataSourceRequest request, string employeeId, int month, int year)
        {
            var model = await client.Get<List<AttendanceReport>>($"{url}/getreportById/{employeeId}/{month}/{year}") ?? new List<AttendanceReport>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Checkin(Attendance model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            response = await client.PostApiResponse<Attendance>($"{url}/checkin", model);
            var data = response.Data as Attendance;
            if (data != null)
            {
                return Json(new { success = response.Success, message = "<h2>Clock In (" + ((TimeSpan)data.TimeAttendance).ToString(@"hh\:mm\:ss") + ")</h2>", url = Url.Action(nameof(Attendance)) });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Attendance)) });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Attendance model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            response = await client.PostApiResponse<Attendance>($"{url}/checkout", model);
            var data = response.Data as Attendance;
            if (data != null)
            {
                return Json(new { success = response.Success, message = "<h2>Clock Out (" + ((TimeSpan)data.TimeAttendance).ToString(@"hh\:mm\:ss") + ")</h2>", url = Url.Action(nameof(Attendance)) });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Attendance)) });
            }
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
    }
}