using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.WebNetCore;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.Finance.Transaction.FixedAssetOutMovement;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area(nameof(Finance))]
    public class FixedAssetController : BaseController
    {
        string url = ApiUrl.FixedAssetUrl;
        string urlGR = ApiUrl.InventoryGoodsReceipt;
        string urlFaMovement = ApiUrl.FixedAssetMovementUrl;
        string urlFaOutMovement = ApiUrl.FixedAssetOutMovement;
        private readonly ApiClientFactory client;

        public FixedAssetController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        } 

        public async Task<IActionResult> Detail(string Id)
        {
            var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
            var model = new FixedAsset();
            ViewBag.Header = "Fixed Asset";
            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.RecordID = Id;
                model = await client.Get<FixedAsset>($"{url}/{Id}");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(FixedAsset entity, IFormFile file)
        {
            var response = new ApiResponse();
            if (!string.IsNullOrEmpty(entity.Id))
            {
                response = await client.PutApiResponse<FixedAsset>(url, entity, file);
            }
            else
            {
                response = await client.PostApiResponse<FixedAsset>(url, entity, file);
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<FixedAsset>>($"{url}") ?? new List<FixedAsset>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadLog([DataSourceRequest] DataSourceRequest request, string id)
        {
            var model = await client.Get<List<FixedAssetOutMovementDetail>>($"{urlFaOutMovement}/getlogoutmovement/{id}") ?? new List<FixedAssetOutMovementDetail>();
            string host = $"{this.Request.Scheme}://{this.Request.Host}";
            foreach (var item in model)
            {
                string[] link = item.UrlLink.Split(new string[] { "?" }, StringSplitOptions.None);
                var detailLink = "/finance/FixedAsset/Detail?";
                var hostOld = link[0];
                var DetailId = link[1];
                item.UrlLink = host + detailLink + DetailId;
            }
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(FixedAsset entyty) => Json(await client.DeleteApiResponse<FixedAsset>($"{url}/{entyty.Id}"));

        public async Task<IActionResult> GetDataGR(string grId)
        {
            var model = await client.Get<InventoryGoodsReceipt>($"{urlGR}/{grId}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult FAStatus_Create([DataSourceRequest] DataSourceRequest request, FixedAssetMovement fams)
        {
            if (fams != null && ModelState.IsValid)
            {
                fams.Id = Guid.NewGuid().ToString();
            }
            return Json(new[] { fams }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult FAStatus_Update([DataSourceRequest] DataSourceRequest request, FixedAssetMovement fams)
        {
            if (fams != null && ModelState.IsValid)
            {
            }
            return Json(new[] { fams }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult FAStatus_Destroy([DataSourceRequest] DataSourceRequest request, FixedAssetMovementStatus fams)
        {
            if (fams != null && ModelState.IsValid)
            {
            }
            return Json(new[] { fams }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadFAStatus([DataSourceRequest] DataSourceRequest request, string faId)
        {
            var model = await client.Get<List<FixedAssetMovement>>($"{urlFaMovement}/getByFixedAsset/{faId}") ?? new List<FixedAssetMovement>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });

        }

        public IActionResult DDLAmortizationPeriod()
        {
            var result = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "1", Value = "1" },
                new SelectListItem() { Text = "2", Value = "2" },
                new SelectListItem() { Text = "3", Value = "3"},
                new SelectListItem() { Text = "4", Value = "4"},
                new SelectListItem() { Text = "5", Value = "5"},
                new SelectListItem() { Text = "6", Value = "6"},
                new SelectListItem() { Text = "7", Value = "7"},
                new SelectListItem() { Text = "8", Value = "8"},
                new SelectListItem() { Text = "9", Value = "9"},
                new SelectListItem() { Text = "10", Value = "10"},
                new SelectListItem() { Text = "11", Value = "11"},
                new SelectListItem() { Text = "12", Value = "12"},
            };
            return Json(result, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        
        [HttpGet]
        public async Task<IActionResult> GetFixedAssetById(string id)
        {
            var model = await client.Get<FixedAsset>($"{url}/{id}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}