using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.WebNetCore;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models.Media.MediaPlanTV;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Http;
using PhoenixERPWebApp.Models.Media.UploadMasterTemplate;
using PhoenixERPWebApp.Models;

namespace PhoenixERPWebApp.Areas.Media.Controllers
{
    [Area(nameof(Media))]
    public class MediaPlanTVController : BaseController
    {
        string url = ApiUrl.MediaPlanTV;
        private readonly ApiClientFactory client;

        public MediaPlanTVController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<MediaPlanTV>>(url) ?? new List<MediaPlanTV>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> AddEdit(string Id, bool isApprove)
        {
            var model = new MediaPlanTV();
            ViewBag.Header = "Media Plan Estimates";

            if (!string.IsNullOrEmpty(Id))
            {
                model = await client.Get<MediaPlanTV>($"{url}/{Id}");
            }

            var templateFile = await client.Get<TrMediaFileMaster>($"{url}/getTemplateId");
            if (templateFile != null)
            {
                model.MediaPlanTvFileTemplateId = templateFile.FileMasterId;
            }

            return View(model);
        }

        public async Task<IActionResult> ReadDetail([DataSourceRequest] DataSourceRequest request, string headerId)
        {
            var model = await client.Get<List<MediaPlanTVDetail>>($"{url}/getDetail/{headerId}") ?? new List<MediaPlanTVDetail>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Process(MediaPlanTV model, IFormFile file)
        {
            var response = new ApiResponse();
            response = await client.PostApiResponse<MediaPlanTV>(url, model, file);
            var media = response.Data as MediaPlanTV;
            return Json(new { success = response.Success, message = response.Message, url = @Url.Action("addedit", "MediaPlanTV")+"?Id=" + media.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Save(MediaPlanTV model, IFormFile file)
        {
            var response = new ApiResponse();
            model.TmMediaPlanStatusId = MediaPlanStatusName.Draft;
            response = await client.PutApiResponse<MediaPlanTV>(url, model, file);
            return Json(new { success = response.Success, message = response.Message, url = @Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(MediaPlanTV model, IFormFile file)
        {
            var response = new ApiResponse();
            model.TmMediaPlanStatusId = MediaPlanStatusName.WaitingApproval;
            response = await client.PutApiResponse<MediaPlanTV>(url, model, file);
            return Json(new { success = response.Success, message = response.Message, url = @Url.Action(nameof(Index)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(MediaPlanTV model) => Json(await client.DeleteApiResponse<MediaPlanTV>($"{url}/{model.Id}"));
    }
}