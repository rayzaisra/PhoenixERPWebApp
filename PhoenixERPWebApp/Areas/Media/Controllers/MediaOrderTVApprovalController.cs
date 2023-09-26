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
using PhoenixERPWebApp.Models.Media;

namespace PhoenixERPWebApp.Areas.Media.Controllers
{
    [Area(nameof(Media))]
    public class MediaOrderTVApprovalController : BaseController
    {
        string url = ApiUrl.MediaOrderTVurl;
        string empUrl = ApiUrl.EmployeeInfoUrl;
        private readonly ApiClientFactory client;

        public MediaOrderTVApprovalController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(string Id)
        {
            var model = new MediaOrderTVApprovalDetail();
            var empModel = new InfoEmployee();
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Approval";
                model = await client.Get<MediaOrderTVApprovalDetail>($"{url}/{Id}");
                empModel = await client.Get<InfoEmployee>($"{empUrl}/getById/{model.EmployeeBasicInfoId}");
                model.RequestBy = empModel.EmployeeName;
                model.JobTitle = empModel.JobTitle;
                model.RequestDate = model.DatePrepared;
                model.RequestCode = model.MediaOrderNo;
                model.RequestStatus = model.TmMediaOrderStatusId;
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<MediaOrderTVList>>($"{url}/GetApprovalList") ?? new List<MediaOrderTVList>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadDetail([DataSourceRequest] DataSourceRequest request, string Id)
        {
            var model = await client.Get<List<MediaOrderTVDetailList>>($"{url}/GetDetailList/{Id}") ?? new List<MediaOrderTVDetailList>();
            //var model = new List<MediaOrderTVDetailList>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Approve(string Id)
        {
            var mediaOrder = await client.Get<MediaOrderTVDetail>($"{url}/{Id}");
            mediaOrder.TmMediaOrderStatusId = MediaOrderStatusName.Approved;
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            response = await client.PutApiResponse<MediaOrderTVDetail>($"{url}/ChangeStatus", mediaOrder);

            return Json(new { success = response.Success, message = response.Message, url = @Url.Action("Index", "MediaOrderTVApproval") });
        }

        [HttpPost]
        public async Task<IActionResult> Reject(string Id)
        {
            var mediaOrder = await client.Get<MediaOrderTVDetail>($"{url}/{Id}");
            mediaOrder.TmMediaOrderStatusId = MediaOrderStatusName.Rejected;
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            response = await client.PutApiResponse<MediaOrderTVDetail>($"{url}/ChangeStatus", mediaOrder);

            return Json(new { success = response.Success, message = response.Message, url = @Url.Action("Index", "MediaOrderTVApproval") });
        }
    }
}