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
    public class ManageLeaveBalanceController : Controller
    {
        string url = ApiUrl.ManageLeaveBalanceUrl;
        private readonly ApiClientFactory client;

        public ManageLeaveBalanceController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<ManageLeaveBalanceView>>(url) ?? new List<ManageLeaveBalanceView>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        public async Task<IActionResult> getAnnualLeave(string employeeId, string leaveType) {
            if (!string.IsNullOrEmpty(employeeId))
            {
                var model = await client.Get<LeaveBalanceViewModel>($"{url}/getAnnualLeave/{employeeId}/{leaveType}");
                return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            else
            {
                LeaveBalanceViewModel model = new LeaveBalanceViewModel()
                {
                    EmployeeId = "",
                    ExpiredDate = "",
                    LeaveBalance = ""

                };
                return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> saveManageLeaveBalance(ManageLeaveBalanceView model)
        {
            LeaveBalanceViewModel ent = new LeaveBalanceViewModel()
            {
                EmployeeId = model.EmployeeId,
                LeaveBalance = model.AnnualLeave.ToString()
            };
            var response = await client.PostApiResponse<int>($"{url}/saveManageLeaveBalance", ent);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }


    }
}