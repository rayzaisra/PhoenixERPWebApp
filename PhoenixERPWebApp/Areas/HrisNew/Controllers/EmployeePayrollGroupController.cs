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
    public class EmployeePayrollGroupController : Controller
    {
        string url = ApiUrl.EmployeePayrollGroupUrl;
        private readonly ApiClientFactory client;

        public EmployeePayrollGroupController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new EmployeePayrollGroup();
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<EmployeePayrollGroup>($"{url}/{Id}");
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<EmployeePayrollGroup>>(url) ?? new List<EmployeePayrollGroup>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadAdditional([DataSourceRequest] DataSourceRequest request, string id)
        {
            var model = await client.Get<List<GroupSubcomponentAdditional>>($"{url}/additional/{id}") ?? new List<GroupSubcomponentAdditional>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadDeduction([DataSourceRequest] DataSourceRequest request, string id)
        {
            var model = await client.Get<List<GroupSubcomponentDeduction>>($"{url}/deducation/{id}") ?? new List<GroupSubcomponentDeduction>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> save(EmployeePayrollGroup model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<EmployeePayrollGroup>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<EmployeePayrollGroup>(url, model);
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(EmployeePayrollGroup model) => Json(await client.DeleteApiResponse<EmployeePayrollGroup>($"{url}/{model.Id}"));

        [HttpGet]
        public async Task<IActionResult> GetAllowanceComponent(string id)
        {
            var model = await client.Get<AllowanceComponent>($"{ApiUrl.AllowanceComponentUrl}/{id}");
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
    }
}