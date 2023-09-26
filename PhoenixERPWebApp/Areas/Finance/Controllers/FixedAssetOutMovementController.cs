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
using PhoenixERPWebApp.Models.Finance.Transaction.FixedAssetOutMovement;
using System.Data;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class FixedAssetOutMovementController : Controller
    {
        string uniqueDetailInv = "FAOUTMOVMNT";
        private readonly ApiClientFactory client;
        string url = ApiUrl.FixedAssetOutMovement;
        string urlGoodRequest = ApiUrl.GoodRequest;
        string urlFixedAsset = ApiUrl.FixedAssetUrl;
        string urlFixedAssetMovement = ApiUrl.FixedAssetMovementUrl;

        public FixedAssetOutMovementController(ApiClientFactory client)
        {
            this.client = client;
        }
        public IActionResult Index()
        {

            return View();
        }

        public async Task<IActionResult> Detail([FromQuery(Name = "id")] string Id, bool isApprove)
        {
            ClearCacheDetailInv();
            DateTime _now = DateTime.Now;
            if (isApprove.Equals(null))
            {
                isApprove = false;
            }
            var model = new FixedAssetOutMovement();

            var isCurrentApproval = false;

            ViewBag.Header = "Fixed Asset Out Movement";
            ViewBag.processApprove = isCurrentApproval;
            ViewBag.Id = "";
            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Fixed Asset Out Movement Detail";
                model = await client.Get<FixedAssetOutMovement>($"{url}/{Id}");
                ViewBag.Id = Id;
                isCurrentApproval = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.processApprove = isCurrentApproval && (model.Status == GoodRequestStatus.WaitingApproval);
            }
            else
            {
                ViewBag.Header = "Fixed Asset Out Movement Request";
            }

            ViewBag.isApprove = isApprove;

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<FixedAssetOutMovement>>($"{url}") ?? new List<FixedAssetOutMovement>();
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

        public async Task<IActionResult> GetBehalf(string behalf)
        {
            try
            {
                var model = await client.Get<GoodRequest>($"{urlGoodRequest}/GetBehalf/{behalf}");
                return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<IActionResult> ReadDataFixedAsset([DataSourceRequest] DataSourceRequest request, string Id)
        {
            try
            {
                var model = await client.Get<List<FixedAssetOutMovementDetail>>($"{urlFixedAsset}/ListFixedAssetOutByLocationId/{Id}") ?? new List<FixedAssetOutMovementDetail>();
                string host = $"{this.Request.Scheme}://{this.Request.Host}";
                foreach (var item in model)
                {
                    item.UrlLink = item.UrlLink.Replace("[Link]", host);
                }
                var data = model.ToDataSourceResult(request);
                return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        [AcceptVerbs("Post")]
        public ActionResult GRDetail_Destroy([DataSourceRequest] DataSourceRequest request, FixedAssetOutMovementDetail grd)
        {
            if (grd != null && ModelState.IsValid)
            {
                //productService.Update(cad);
            }
            return Json(new[] { grd }.ToDataSourceResult(request, ModelState));
            //return Json(new[] { fams }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult GRDetail_Update([DataSourceRequest] DataSourceRequest request, FixedAssetOutMovementDetail grd, string dateTrx)
        {
            if (grd != null && ModelState.IsValid)
            {
                //productService.Update(cad);
            }
            if (!string.IsNullOrEmpty(dateTrx))
            {
                grd.RequestDate = DateTime.Parse(dateTrx, CultureInfo.InvariantCulture).Date;
            }

            return Json(new[] { grd }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            //return Json(new[] { fams }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadDetail([DataSourceRequest] DataSourceRequest request, string headerId)
        {
            try
            {
                var model = new List<FixedAssetOutMovementDetail>();
                if (GetCacheDetailInv().Count() > 0)
                {
                    model = GetCacheDetailInv();
                }
                else
                {
                    model = await client.Get<List<FixedAssetOutMovementDetail>>($"{url}/GetListFixedAssetOutDetail/{headerId}") ?? new List<FixedAssetOutMovementDetail>();
                    string host = $"{this.Request.Scheme}://{this.Request.Host}";
                    foreach (var item in model)
                    {
                        item.UrlLink = item.UrlLink.Replace("[Link]", host);
                    }
                    SetCacheDetailInv(model);
                }
                var data = model.ToDataSourceResult(request);
                return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

        }

        public List<FixedAssetOutMovementDetail> GetCacheDetailInv()
        {
            var models = CacheStore.Get<List<FixedAssetOutMovementDetail>>(uniqueDetailInv);
            if (models == null)
                models = new List<FixedAssetOutMovementDetail>();
            return models;
        }

        private void ClearCacheDetailInv()
        {
            CacheStore.Remove<List<FixedAssetOutMovementDetail>>(uniqueDetailInv);
        }

        public void SetCacheDetailInv(List<FixedAssetOutMovementDetail> models)
        {
            ClearCacheDetailInv();
            CacheStore.Add<List<FixedAssetOutMovementDetail>>(uniqueDetailInv, models);
        }

        [HttpPost]
        public IActionResult Generate(List<FixedAssetOutMovementDetail> datas)
        {
            ClearCacheDetailInv();
            var DetailInvList = GetCacheDetailInv().ToList();
            var listId = new List<string>();
            foreach (FixedAssetOutMovementDetail x in DetailInvList)
            {
                listId.Add(x.FixedAssetId);
            }

            foreach(FixedAssetOutMovementDetail data in datas)
            {
                if (!listId.Contains(data.Id))
                {
                    var detail = new FixedAssetOutMovementDetail();
                    DetailInvList = DetailInvList.Where(x => x.FixedAssetId != data.Id).ToList();
                    detail.Id = data.Id;
                    detail.FixedAssetId = data.Id;
                    detail.QtyMax = data.QtyMax;
                    detail.UnitName = data.UnitName;
                    detail.UrlLink = data.UrlLink;
                    detail.LegalEntityId = data.LegalEntityId;
                    detail.LocationId = data.LocationId;
                    detail.Condition = data.Condition;
                    detail.Brand = data.Brand;
                    detail.ItemCategoryId = data.ItemCategoryId;
                    detail.Qty = 1;
                    detail.RequestDate = DateTime.Now;
                    //detail.RegistrationDate = data.RegistrationDate;
                    //if (fileDetail != null)
                    //{
                    //    vendor.File = fileDetail;
                    //    vendor.FilemasterName = fileDetail.FileName;
                    //}
                    DetailInvList.Add(detail);
                }
            }
            SetCacheDetailInv(DetailInvList);

            //var vendorDetailList = GetCacheDetailVendor().Where(x => x.VendorId != data.VendorId).ToList();
            //vendorDetailList.AddRange(model.RequestForQuotationDetails);
            //SetCacheDetailVendor(vendorDetailList);
            return Json(new { success = true, message = "success", refreshGrid = "grid", hideModalPopUp = "modalDetailInv" });
        }

        [HttpPost]
        public async Task<IActionResult> Save(FixedAssetOutMovement Header, List<FixedAssetOutMovementDetail> Details)
        {
            try
            {
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                var response = new ApiResponse();
                var model = new FixedAssetOutMovementDto();
                model.Header = Header;
                model.Header.Status = GoodRequestStatus.Open;
                model.Details = Details;

                if (!string.IsNullOrEmpty(Header.Id))
                {
                    response = await client.PutApiResponse<FixedAssetOutMovementDto>(url, model);
                }
                else
                {
                    response = await client.PostApiResponse<FixedAssetOutMovementDto>(url, model);
                }

                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message.ToString() });
            }

        }

        public async Task<IActionResult> Submit(FixedAssetOutMovement Header, List<FixedAssetOutMovementDetail> Details)
        {
            try
            {
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                var response = new ApiResponse();
                var model = new FixedAssetOutMovementDto();
                model.Header = Header;
                model.Header.Status = GoodRequestStatus.WaitingApproval;
                model.Details = Details;

                if (!string.IsNullOrEmpty(Header.Id))
                {
                    response = await client.PutApiResponse<FixedAssetOutMovementDto>(url, model);
                }
                else
                {
                    response = await client.PostApiResponse<FixedAssetOutMovementDto>(url, model);
                }

                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message.ToString() });
            }

        }

        [HttpPost]
        public async Task<IActionResult> Approve(FixedAssetOutMovement Header, List<FixedAssetOutMovementDetail> Details)
        {
            var model = new FixedAssetOutMovementDto();
            model.Header = Header;
            model.Details = Details;
            var response = await client.PutApiResponse<FixedAssetOutMovementDto>($"{url}/ApproveCustom", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }
        [HttpPost]
        public async Task<IActionResult> Reject(FixedAssetOutMovement model)
        {
            var response = await client.PutApiResponse<FixedAssetOutMovement>($"{url}/RejectData", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }
    }
}