using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.WebNetCore;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.Media.MediaPlanTV;
using PhoenixERPWebApp.Models.Media.MediaPlanTVApproval;

namespace PhoenixERPWebApp.Areas.Media.Controllers
{
    [Area(nameof(Media))]
    public class MediaPlanTVApprovalController : BaseController
    {
        string url = ApiUrl.MediaPlanTVApproval;
        string empUrl = ApiUrl.EmployeeInfoUrl;
        private readonly ApiClientFactory client;

        public MediaPlanTVApprovalController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(string Id)
        {
            var model = new MediaPlanTVApprovalDetail();
            var empModel = new InfoEmployee();

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Media Plan Estimate Approval";
                model = await client.Get<MediaPlanTVApprovalDetail>($"{url}/{Id}");
                empModel = await client.Get<InfoEmployee>($"{empUrl}/getById/{model.EmployeeBasicInfoId}");
                model.RequestBy = empModel.EmployeeName;
                model.JobTitle = empModel.JobTitle;
                model.RequestDate = model.DatePrepared;
                model.RequestCode = model.MediaPlanNo;
                model.RequestStatus = model.TmMediaPlanStatusId;
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<MediaPlanTVApproval>>(url) ?? new List<MediaPlanTVApproval>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        
        public async Task<IActionResult> ReadDetail([DataSourceRequest] DataSourceRequest request, string headerId)
        {
            var model = await client.Get<List<MediaPlanTVDetail>>($"{url}/getDetailList/{headerId}") ?? new List<MediaPlanTVDetail>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> IndexSchedule(string Id)
        {
            var model = await client.Get<MediaPlanTVDetailList>($"{url}/GetDetail/{Id}") ?? new MediaPlanTVDetailList();
            return PartialView("_IndexSchedule", model);
        }

        public async Task<IActionResult> ReadDetailSchedule([DataSourceRequest] DataSourceRequest request, string Id)
        {
            var model = await client.Get<List<MediaPlanTVDetailScheduleList>>($"{url}/GetScheduleList/{Id}") ?? new List<MediaPlanTVDetailScheduleList>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}