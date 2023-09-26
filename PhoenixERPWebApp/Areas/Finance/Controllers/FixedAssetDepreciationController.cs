using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class FixedAssetDepreciationController : BaseController
    {
        string url = ApiUrl.FixedAssetDepreciationtUrl;
        string urlFA = ApiUrl.FixedAssetUrl;
        private readonly ApiClientFactory client;

        public FixedAssetDepreciationController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<FixedAssetDepreciation>>(url) ?? new List<FixedAssetDepreciation>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> Detail(string Id)
        {
            var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
            var model = new FixedAssetDepreciation() { FixedAsset = new FixedAsset() };
            ViewBag.Header = "Fixed Asset Depreciation";
            if (!string.IsNullOrEmpty(Id))
            {
                model = await client.Get<FixedAssetDepreciation>($"{url}/{Id}");
            }

            return View(model);
        }

        public async Task<IActionResult> ReadYearDetail([DataSourceRequest] DataSourceRequest request, string Id)
        {
            var model = await client.Get<List<FixedAssetDepreciationYears>>($"{url}/GetByYearDetail/{Id}") ?? new List<FixedAssetDepreciationYears>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadMonthDetail([DataSourceRequest] DataSourceRequest request, string Id)
        {
            var model = await client.Get<List<FixedAssetDepreciationDetail>>($"{url}/GetByMonthDetail/{Id}") ?? new List<FixedAssetDepreciationDetail>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadJournalOpeningBalance([DataSourceRequest] DataSourceRequest request, string Id)
        {
            var model = await client.Get<List<Journal>>($"{url}/GetOpeningBalance/{Id}") ?? new List<Journal>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(FixedAssetDepreciation entyty) => Json(await client.DeleteApiResponse<FixedAssetDepreciation>($"{url}/{entyty.Id}"));

        [HttpPost]
        public async Task<IActionResult> DeleteDetail(FixedAssetDepreciation model)
        {
            var messages = "Data has been successfully canceled.";
            if (string.IsNullOrEmpty(model.Id))
            {
                return Json(new { success = true, message = messages, url = Url.Action(nameof(Index)) });
            }
            else
            {
                var response = await client.DeleteApiResponse<FixedAssetDepreciation>($"{url}/{model.Id}");
                if (!response.Success) messages = response.Message;
                return Json(new { success = response.Success, message = messages, url = Url.Action(nameof(Index)) });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Submit(FixedAssetDepreciation model)
        {
            //model.Status = "Submitted";
            return await Process(model, false);
        }

        [HttpPost]
        public async Task<IActionResult> Calculate(FixedAssetDepreciation model)
        {
            //model.Status = "Draft";
            return await Process(model, true);
        }

        private async Task<IActionResult> Process(FixedAssetDepreciation model, bool isDraft)
        {
            var response = new ApiResponse();
            model.FixedAsset = null;

            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<FixedAssetDepreciation>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<FixedAssetDepreciation>(url, model);
            }

            if (isDraft)
            {
                var data = response.Data as FixedAssetDepreciation;
                //clear remarks log
                var setHiddenValue = new[] { new { id = "Status", value = data.Status } };
                return Json(new { success = response.Success, message = response.Message, refreshGrid = "gridYearList;gridMonthList", data = data, setHiddenValue = setHiddenValue });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostJournal(FixedAssetDepreciationJournal model)
        {
            var response = await client.PostApiResponse<string>($"{url}/PostJournal", model);
            return Json(new { success = response.Success, message = response.Message });
        }

        [HttpGet]
        public async Task<IActionResult> GetPostingDate(string legalEntityId)
        {
            var response = await client.Get<DateTime>($"{url}/GetPostingDate/{legalEntityId}");
            return Json(response);
        }
    }
}