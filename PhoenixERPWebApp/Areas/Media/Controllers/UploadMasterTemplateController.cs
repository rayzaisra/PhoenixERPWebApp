using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.WebNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models.Media.UploadMasterTemplate;

namespace PhoenixERPWebApp.Areas.Media.Controllers
{
    [Area(nameof(Media))]
    public class UploadMasterTemplateController : BaseController
    {
        string url = ApiUrl.UploadMasterTemplateUrl;
        string urlReadExcel = ApiUrl.ReadExcelMediaPlanUrl;
        private readonly ApiClientFactory client;

        public UploadMasterTemplateController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(TrMediaFileMaster model, IFormFile file)
        {
            return await Process(model, file);

            //Test Read Excel
            //return await ProcessTest(model, file);
        }

        private async Task<IActionResult> Process(TrMediaFileMaster model, IFormFile file)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            response = await client.PostApiResponse<TrMediaFileMaster>(url, model, file);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        //Example Method to read excel, return Data Set.
        private async Task<IActionResult> ProcessTest(TrMediaFileMaster model, IFormFile file)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            response = await client.PostApiResponse<DataSet>(urlReadExcel, null, file);
            //To Get any rows in Excel
            var dataSet = response.Data as DataSet;
            //Tables -> sheet, Rows -> get the data
            var data = dataSet.Tables[0].Rows[1][2];

            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }
    }
}