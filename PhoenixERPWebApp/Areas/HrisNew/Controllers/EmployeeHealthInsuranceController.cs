using System;
using System.Collections.Generic;
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

namespace PhoenixERPWebApp.Areas.HrisNew.Controllers
{
    [Area(nameof(HrisNew))]
    public class EmployeeHealthInsuranceController : Controller
    {
        string url = ApiUrl.EmployeeHealthInsuranceUrl;
        private readonly ApiClientFactory client;

        public EmployeeHealthInsuranceController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View(new EmployeeHealthInsurance());
        }

        public async Task<IActionResult> ReadFamily([DataSourceRequest] DataSourceRequest request, string employeeId)
        {
            var model = await client.Get<List<EmployeeHealthInsuranceFamily>>($"{url}/getFamilyList/{employeeId}") ?? new List<EmployeeHealthInsuranceFamily>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(EmployeeHealthInsurance model)
        {
            var response = await client.PostApiResponse<EmployeeHealthInsurance>(url, model);
            return Json(new { success = response.Success, message = response.Message });
        }

        public async Task<IActionResult> AddEdit(string employeeId)
        {
            var model = await client.Get<EmployeeHealthInsurance>($"{url}/{employeeId}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        public async Task<IActionResult> GetFamily(string familyId)
        {
            var model = await client.Get<EmployeeHealthInsuranceFamily>($"{url}/getFamilyById/{familyId}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        //Family
        [AcceptVerbs("Post")]
        public ActionResult CreateFamily([DataSourceRequest] DataSourceRequest request, EmployeeHealthInsuranceFamily model)
        {
            if (model != null && ModelState.IsValid)
            {
                model.Id = Guid.NewGuid().ToString();
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult UpdateFamily([DataSourceRequest] DataSourceRequest request, EmployeeHealthInsuranceFamily model)
        {
            if (model != null && ModelState.IsValid)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult DestroyFamily([DataSourceRequest] DataSourceRequest request, EmployeeHealthInsuranceFamily model)
        {
            if (model != null)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}