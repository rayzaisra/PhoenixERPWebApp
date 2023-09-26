using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.WebNetCore;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models.Media;
using PhoenixERPWebApp.Models.Media.MediaPlanTV;
using NPoco.fastJSON;
using PhoenixERPWebApp.Models;

namespace PhoenixERPWebApp.Areas.Media.Controllers
{
    [Area(nameof(Media))]
    public class MediaOrderTVController : BaseController
    {
        string url = ApiUrl.MediaOrderTVurl;
        string urlDownloadExcel = ApiUrl.DownloadExcelUrl;
        private readonly ApiClientFactory client;

        public MediaOrderTVController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(string Id)
        {
            var model = new MediaOrderTVDetail();
            model.DatePrepared = DateTime.Now;
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<MediaOrderTVDetail>($"{url}/{Id}");
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<MediaOrderTVList>>(url) ?? new List<MediaOrderTVList>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadDetail([DataSourceRequest] DataSourceRequest request, string Id)
        {
            var model = await client.Get<List<MediaOrderTVDetailList>>($"{url}/GetDetailList/{Id}") ?? new List<MediaOrderTVDetailList>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadDetailSchedule([DataSourceRequest] DataSourceRequest request, string Id)
        {
            var model = await client.Get<List<MediaOrderTVDetailScheduleList>>($"{url}/GetScheduleList/{Id}") ?? new List<MediaOrderTVDetailScheduleList>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> GetMediaPlanTVHeader(string Id)
        {
            var model = new MediaPlanTV();
            model = await client.Get<MediaPlanTV>($"{url}/GetMediaPlanTVHeader/{Id}");
            //var data = model.ToDataSourceResult(request);
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> GetMediaPlanTVDetail(string Id)
        {
            var model = new List<MediaOrderTVDetailList>();
            model = await client.Get<List<MediaOrderTVDetailList>>($"{url}/GetMediaPlanTVDetail/{Id}");
            //var data = model.ToDataSourceResult(request);
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Save(MediaOrderTVDetail model, List<MediaOrderTVDetailList> details)
        {
            MediaOrderTVDTO dto = new MediaOrderTVDTO();
            dto.Header = model;
            dto.Details = details;
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<MediaOrderTVDTO>(url, dto);
            }
            else
            {
                dto.Header.MediaOrderNo = "TEST.MO.001";
                response = await client.PostApiResponse<MediaOrderTVDTO>(url, dto);
            }

            dto = response.Data as MediaOrderTVDTO;

            return Json(new { success = response.Success, message = response.Message, url = @Url.Action("Detail", "MediaOrderTV") + "?Id=" + dto.Header.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(MediaOrderTVDetail model, List<MediaOrderTVDetailList> TrMediaOrderTVDetails)
        {
            model.TmMediaOrderStatusId = MediaOrderStatusName.WaitingApproval;
            MediaOrderTVDTO dto = new MediaOrderTVDTO();
            dto.Header = model;
            dto.Details = TrMediaOrderTVDetails;
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            response = await client.PutApiResponse<MediaOrderTVDTO>(url, dto);

            return Json(new { success = response.Success, message = response.Message, url = @Url.Action("Index", "MediaOrderTV") });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string Id) => Json(await client.DeleteApiResponse<MediaPlanTV>($"{url}/{Id}"));

        public async Task<IActionResult> IndexSchedule(string Id)
        {
            var model = await client.Get<MediaOrderTVDetailList>($"{url}/GetDetail/{Id}") ?? new MediaOrderTVDetailList();
            return PartialView("_IndexSchedule", model);
        }

        public async Task<IActionResult> Arrange(string Id)
        {
            ViewBag.Header = "Arrange";
            var model = new MediaOrderTVArrange();
            model = await client.Get<MediaOrderTVArrange>($"{url}/{Id}");
            return View(model);
        }

        public async Task<FileContentResult> DownloadTemplate()
        {
            var list = await client.Get<List<MediaOrderTVList>>(url) ?? new List<MediaOrderTVList>();
            //var model = await client.Get<byte[]>(urlDownloadExcel);
            string fileName = $"Template_{DateTime.Now.ToString("yyyyMMddHHmmss")}";
            string sheetName = $"Sheet1";
            var model = await client.Post<byte[]>(urlDownloadExcel, new
            {
                list,
                fileName,
                sheetName
            });
            return await Task.FromResult(new FileContentResult(model, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = $"{fileName}.xlsx" }); ;
        }
    }
}