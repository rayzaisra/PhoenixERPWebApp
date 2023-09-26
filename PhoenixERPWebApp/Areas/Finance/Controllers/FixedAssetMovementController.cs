using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.WebNetCore;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.Extentions;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.Finance;
using PhoenixERPWebApp.Models.Enum;
using PhoenixERPWebApp.Models.Finance.Transaction.Inventory;
using System.Globalization;
using System.Data;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class FixedAssetMovementController : Controller
    {
        string uniqueDetailInv = "FAMOVMNT";
        string url = ApiUrl.FixedAssetMovementUrl;
        string urlGoodRequest = ApiUrl.GoodRequest;
        string urlFixedAsset = ApiUrl.FixedAssetUrl;
        private readonly ApiClientFactory client;

        public FixedAssetMovementController(ApiClientFactory client)
        {
            this.client = client;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail([FromQuery(Name = "id")] string Id, bool isApprove)
        {
            DateTime _now = DateTime.Now;
            if (isApprove.Equals(null))
            {
                isApprove = false;
            }
            var model = new FixedAssetMovement();

            var isCurrentApproval = false;

            ViewBag.Header = "Fixed Asset Movement";
            ViewBag.processApprove = isCurrentApproval;

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Fixed Asset Movement Detail";
                model = await client.Get<FixedAssetMovement>($"{url}/GetData/{Id}");

                isCurrentApproval = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.processApprove = isCurrentApproval && (model.Status == GoodRequestStatus.WaitingApproval);
            }
            else
            {
                ViewBag.Header = "Fixed Asset Movement Request";
            }

            ViewBag.isApprove = isApprove;

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<FixedAssetMovement>>($"{url}") ?? new List<FixedAssetMovement>();
            //if (response != null)
            //{
            //    model = CastResponseData<List<InventoryGoodsReceipt>>(response.Data);
            //}

            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadDetail([DataSourceRequest] DataSourceRequest request, string id)
        {
            var model = await client.Get<List<FixedAssetMovementDetail>>($"{url}/GetListFixedAssetmovementDetail/{id}") ?? new List<FixedAssetMovementDetail>();
            //if (response != null)
            //{
            //    model = CastResponseData<List<InventoryGoodsReceipt>>(response.Data);
            //}

            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> GetDivLogin(string BasicLogin)
        {
            try
            {
                var model = await client.Get<GoodRequest>($"{urlGoodRequest}/GetDivLogin/{BasicLogin}");
                return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<IActionResult> GetDataFixedAsset(string Id)
        {
            try
            {
                var model = await client.Get<FixedAssetMovementDetail>($"{urlFixedAsset}/{Id}");

                return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }


        public async Task<IActionResult> GRDetail_Update([DataSourceRequest] DataSourceRequest request, FixedAssetMovementDetail grd)
        {
            return Json(new[] { grd }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            //return Json(new[] { fams }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Save(FixedAssetMovement Header, List<FixedAssetMovementDetail> Details)
        {
            try
            {
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                var response = new ApiResponse();
                var model = new FixedAssetMovementDto();
                model.Header = Header;
                model.Header.Status = GoodRequestStatus.Open;
                model.Details = Details;

                if (!string.IsNullOrEmpty(Header.Id))
                {
                    response = await client.PutApiResponse<FixedAssetMovementDto>(url, model);
                }
                else
                {
                    response = await client.PostApiResponse<FixedAssetMovementDto>(url, model);
                }

                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message.ToString() });
            }

        }

        public async Task<IActionResult> Submit(FixedAssetMovement Header, List<FixedAssetMovementDetail> Details)
        {
            try
            {
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                var response = new ApiResponse();
                var model = new FixedAssetMovementDto();
                model.Header = Header;
                model.Header.Status = GoodRequestStatus.WaitingApproval;
                model.Details = Details;

                if (!string.IsNullOrEmpty(Header.Id))
                {
                    response = await client.PutApiResponse<FixedAssetMovementDto>(url, model);
                }
                else
                {
                    response = await client.PostApiResponse<FixedAssetMovementDto>(url, model);
                }

                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message.ToString() });
            }

        }

        [HttpPost]
        public async Task<IActionResult> Approve(FixedAssetMovement header, List<FixedAssetMovementDetail> Details)
        {
            var model = new FixedAssetMovementDto();
            model.Header = header;
            model.Details = Details;
            var response = await client.PutApiResponse<FixedAssetMovementDto>($"{url}/ApproveCustom", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }
        [HttpPost]
        public async Task<IActionResult> Reject(FixedAssetMovement model)
        {
            var response = await client.PutApiResponse<FixedAssetMovement>($"{url}/RejectData", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }
    }
}