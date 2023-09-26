using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;

namespace PhoenixERPWebApp.Areas.HrisNew.Controllers
{
    [Area(nameof(HrisNew))]
    public class EmployeeInfoController : Controller
    {
        private readonly ApiClientFactory client;

        public EmployeeInfoController(ApiClientFactory client)
        {
            this.client = client;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetEmployeeInfoById(string employeeId)
        {
            var model = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getById/{employeeId}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeInfoByIdAndLeaveRequestId(string employeeId, string leaverequestId)
        {
            var model = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByIdAndLeaveRequest/{employeeId}/{leaverequestId}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployeByLeaveRequest(string leaverequestId)
        {
            var model = await client.Get<List<InfoEmployee>>($"{ApiUrl.EmployeeInfoUrl}/getAllEmployeByLeaveRequest/{leaverequestId}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeLoanInfoById(string employeeId, string isCorporateloan)
        {
            var model = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getLoanInfoById/{employeeId}/{isCorporateloan}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}