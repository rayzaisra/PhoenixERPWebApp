using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.ModelShared;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.HrisNew.Transaction.Payroll;

namespace PhoenixERPWebApp.Areas.HrisNew.Controllers
{
    [Area(nameof(HrisNew))]
    public class EmployeePayrollProfileController : Controller
    {
        string url = ApiUrl.EmployeePayrollProfileUrl;
        private readonly ApiClientFactory client;
        string menuUnique = "PAYROLLEMPPROFILE";
        public EmployeePayrollProfileController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddEdit(string employeeId)
        {
            var model = new InfoEmployee();
            ViewBag.Header = "Employee Payroll Profile";
            ViewBag.IsAllowOpenEncryptEmployeePayroll = await client.Get<bool>($"{ApiUrl.ManageUserUrl}/IsAllowOpenEncryptEmployeePayroll/{menuUnique}");
            if (!string.IsNullOrEmpty(employeeId))
            {
                model = await client.Get<InfoEmployee>($"{url}/{employeeId}");
                var modelHistory = await client.Get<EmployeeHistoryPayroll>($"{url}/payrollHistory/{employeeId}") ?? new EmployeeHistoryPayroll();
                model.EmployeeHistoryPayroll = modelHistory;
            }
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, string empName)
        {
            var model = await client.Get<List<InfoEmployee>>($"{url}/gets/{empName}") ?? new List<InfoEmployee>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        //public async Task<IActionResult> GetHistoryPayroll([DataSourceRequest] DataSourceRequest request, string empId)
        //{
        //    var model = await client.Get<EmployeeHistoryPayroll>($"{url}/payrollHistory/{empId}") ?? new EmployeeHistoryPayroll();

        //    return View(model);
        //}

        public async Task<IActionResult> ReadAdditional([DataSourceRequest] DataSourceRequest request, string employeeId, string employeePayrollGroupId)
        {
            var model = new List<EmployeeSubcomponentAdditional>();
            if (string.IsNullOrEmpty(employeePayrollGroupId))
            {
                model = await client.Get<List<EmployeeSubcomponentAdditional>>($"{url}/additional/{employeeId}") ?? new List<EmployeeSubcomponentAdditional>();
            }
            else
            {
                model = await client.Get<List<EmployeeSubcomponentAdditional>>($"{ApiUrl.EmployeePayrollGroupUrl}/additional/{employeePayrollGroupId}") ?? new List<EmployeeSubcomponentAdditional>();
            }

            var IsAllowOpenEncryptEmployeePayroll = await client.Get<bool>($"{ApiUrl.ManageUserUrl}/IsAllowOpenEncryptEmployeePayroll");
            if (!IsAllowOpenEncryptEmployeePayroll)
            {
                foreach (var item in model)
                {
                    item.EncryptAmount = "***********";
                }
            }

            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadDeduction([DataSourceRequest] DataSourceRequest request, string employeeId, string employeePayrollGroupId)
        {
            var model = new List<EmployeeSubcomponentDeduction>();
            if (string.IsNullOrEmpty(employeePayrollGroupId))
            {
                model = await client.Get<List<EmployeeSubcomponentDeduction>>($"{url}/deducation/{employeeId}") ?? new List<EmployeeSubcomponentDeduction>();
            }
            else
            {
                model = await client.Get<List<EmployeeSubcomponentDeduction>>($"{ApiUrl.EmployeePayrollGroupUrl}/deducation/{employeePayrollGroupId}") ?? new List<EmployeeSubcomponentDeduction>();
            }
            var IsAllowOpenEncryptEmployeePayroll = await client.Get<bool>($"{ApiUrl.ManageUserUrl}/IsAllowOpenEncryptEmployeePayroll");
            if (!IsAllowOpenEncryptEmployeePayroll)
            {
                foreach (var item in model)
                {
                    item.EncryptAmount = "***********";
                }
            }
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> save(EmployeePayrollProfile model)
        {
            var response = await client.PostApiResponse<EmployeePayrollProfile>(url, model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllowanceComponent(string id, string employeeId)
        {
            //var model = await client.Get<AllowanceComponent>($"{ApiUrl.AllowanceComponentUrl}/{id}");
            var model = await client.Get<AllowanceComponent>($"{ApiUrl.AllowanceComponentUrl}/GetAllowanceComponent/{id}/{employeeId}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        public async Task<IActionResult> GetBPJSComponent(string id)
        {
            var model = await client.Get<BpjsComponent>($"{ApiUrl.BpjsComponentUrl}/{id}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        //Additional
        [AcceptVerbs("Post")]
        public ActionResult CreateAdditional([DataSourceRequest] DataSourceRequest request, GroupSubcomponentAdditional model)
        {
            if (model != null && ModelState.IsValid)
            {
                model.Id = Guid.NewGuid().ToString();
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult UpdateAdditional([DataSourceRequest] DataSourceRequest request, GroupSubcomponentAdditional model)
        {
            if (model != null && ModelState.IsValid)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult DestroyAdditional([DataSourceRequest] DataSourceRequest request, GroupSubcomponentAdditional model)
        {
            if (model != null)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        //Deduction
        [AcceptVerbs("Post")]
        public ActionResult CreateDeduction([DataSourceRequest] DataSourceRequest request, GroupSubcomponentDeduction model)
        {
            if (model != null && ModelState.IsValid)
            {
                model.Id = Guid.NewGuid().ToString();
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult UpdateDeduction([DataSourceRequest] DataSourceRequest request, GroupSubcomponentDeduction model)
        {
            if (model != null && ModelState.IsValid)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult DestroyDeduction([DataSourceRequest] DataSourceRequest request, GroupSubcomponentDeduction model)
        {
            if (model != null)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        public async Task<IActionResult> getPph21(string id, string employeeId)
        {
            var model = await client.Get<pph21>($"{ApiUrl.EmployeePayrollProfileUrl}/getpph21/{id}/{employeeId}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }


    }
}

public class pph21
{
    public string result { get; set; }
}