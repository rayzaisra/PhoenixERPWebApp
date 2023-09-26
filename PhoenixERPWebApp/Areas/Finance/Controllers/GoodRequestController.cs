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

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class GoodRequestController : BaseController
    {
        string uniqueDetailInv = "DETAILGOODREQUEST";
        string url = ApiUrl.GoodRequest;
        string urlInv = ApiUrl.Inventory;
        string urlFa = ApiUrl.FixedAssetUrl;
        private readonly ApiClientFactory client;

        public GoodRequestController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        //public async Task<IActionResult> Detail()
        //{
        //    return View();
        //}
        public async Task<IActionResult> Detail([FromQuery(Name = "id")] string Id, bool isApprove)
        {
            ClearCacheDetailInv();
            DateTime _now = DateTime.Now;
            if (isApprove.Equals(null))
            {
                isApprove = false;
            }
            var model = new GoodRequest();

            var isCurrentApproval = false;

            ViewBag.Header = "Inventory Movement & Good Request";

            if (!string.IsNullOrEmpty(Id))
            {
                //if (isApprove == true)
                //    ViewBag.Header = "Approve Good Request";
                //else
                //    ViewBag.Header = "Good Request Detail";
                //ViewBag.RecordID = Id;
                ViewBag.Header = "Inventory Movement & Good Request Detail";
                model = await client.Get<GoodRequest>($"{url}/{Id}"); 
                isCurrentApproval = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}"); 
                ViewBag.processApprove = isCurrentApproval && (model.Status == GoodRequestStatus.WaitingApproval); 
            }
            else
            {
                ViewBag.Header = "Inventory Movement & Good Request";
               // var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
            }
             
            ViewBag.isApprove = isApprove;

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                //List<InventoryGoodsReceipt> model = new List<InventoryGoodsReceipt>();

                var model = await client.Get<List<GoodRequest>>($"{url}") ?? new List<GoodRequest>();
                //if (response != null)
                //{
                //    model = CastResponseData<List<InventoryGoodsReceipt>>(response.Data);
                //}

                var data = model.ToDataSourceResult(request);
                return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

        }

        public async Task<IActionResult> ReadDetail([DataSourceRequest] DataSourceRequest request, string headerId)
        {
            try
            {
                var model = new List<GoodRequestDetail>();
                if (GetCacheDetailInv().Count() > 0)
                {
                    model = GetCacheDetailInv();
                }
                else
                {
                    model = await client.Get<List<GoodRequestDetail>>($"{url}/getDetail/{headerId}") ?? new List<GoodRequestDetail>();
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

        public async Task<IActionResult> ReadInventoryDetail([DataSourceRequest] DataSourceRequest request, string invId)
        {
            try
            {
                var model = await client.Get<List<InventoryDetail>>($"{urlInv}/GetDetailForGoodRequest/{invId}") ?? new List<InventoryDetail>();
                if (model.Count() > 0)
                {
                    model = model.Where(s => s.status_name != null && s.status_name.Contains("On Hand")).ToList();
                }
                var data = model.ToDataSourceResult(request);
                return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

        }

        public async Task<IActionResult> ReadFixedAssetDetail([DataSourceRequest] DataSourceRequest request, string invId)
        {
            try
            {
                var FAList = new List<FixedAsset>();
                var model = await client.Get<FixedAsset>($"{urlFa}/{invId}") ?? new FixedAsset();               
               
                var data = FAList.ToDataSourceResult(request);
                return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

        }

        public IActionResult ReadDetailCache([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = GetCacheDetailInv();
                var data = model.ToDataSourceResult(request);
                return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

        }

        public async Task<IActionResult> GetInvFa(string type)
        {
            try
            {
                ClearCacheDetailInv();
                var model = new List<SelectListItem>();
                if (type == "INVENTORY")
                {
                    var getUrl = $"{ApiUrl.dropdownUrl}/GetInventory";
                    model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
                    return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
                }
                else if(type=="FIXED ASSET")
                {
                    var getUrl = $"{ApiUrl.dropdownUrl}/GetFixedAsset";
                    model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
                    return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
                }
                else
                {
                    return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<IActionResult> GetInventory(string InvId)
        {
            try
            {
                var model = await client.Get<Inventory>($"{urlInv}/getInvAll/{InvId}"); 
                return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            
        }

        public async Task<IActionResult> GetFixedAsset(string FaId)
        {
            try
            {
                var model = await client.Get<FixedAsset>($"{urlFa}/{FaId}");
                return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Save(GoodRequest Header, List<GoodRequestDetail> Details)
        {
            try
            {
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                var response = new ApiResponse();
                var model = new GoodRequestDTO();
                model.Header = Header;
                model.Header.Status = GoodRequestStatus.Open;
                model.Details = Details;

                if (!string.IsNullOrEmpty(Header.Id))
                {
                    response = await client.PutApiResponse<GoodRequestDTO>(url, model);
                }
                else
                {
                    response = await client.PostApiResponse<GoodRequestDTO>(url, model);
                }

                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message.ToString() });
            }

        }

        public async Task<IActionResult> Submit(GoodRequest Header, List<GoodRequestDetail> Details)
        {
            try
            {
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                var response = new ApiResponse();
                var model = new GoodRequestDTO();
                model.Header = Header;
                model.Header.Status = GoodRequestStatus.WaitingApproval;
                model.Details = Details;
                model.Header.LogDescription = model.Header.RemarkLog;

                if (!string.IsNullOrEmpty(Header.Id))
                {
                    response = await client.PutApiResponse<GoodRequestDTO>(url, model);
                }
                else
                {
                    response = await client.PostApiResponse<GoodRequestDTO>(url, model);
                }

                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message.ToString() });
            }

        }


        [HttpPost]
        public async Task<IActionResult> Approve(GoodRequest header, List<GoodRequestDetail> Details)
        {
            header.LogDescription = header.RemarkLog;
            var model = new GoodRequestDTO();
            model.Header = header;
            model.Details = Details;
            var response = await client.PutApiResponse<GoodRequestDTO>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }
        [HttpPost]
        public async Task<IActionResult> Reject(GoodRequest model)
        {
            model.LogDescription = model.RemarkLog;
            var response = await client.PutApiResponse<GoodRequest>($"{url}/RejectData", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }
        //[HttpPost]
        //public async Task<IActionResult> Complete(GoodRequest model)
        //{
        //    var response = await client.PutApiResponse<GoodRequest>($"{url}/Complete", model);
        //    return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        //}

        private void ClearCacheDetailInv()
        {
            CacheStore.Remove<List<GoodRequestDetail>>(uniqueDetailInv);
        }

        public void SetCacheDetailInv(List<GoodRequestDetail> models)
        {
            ClearCacheDetailInv();
            CacheStore.Add<List<GoodRequestDetail>>(uniqueDetailInv, models);
        }

        public List<GoodRequestDetail> GetCacheDetailInv()
        {
            var models = CacheStore.Get<List<GoodRequestDetail>>(uniqueDetailInv);
            if (models == null)
                models = new List<GoodRequestDetail>();
            return models;
        }

        [HttpPost]
        public IActionResult Generate(List<InventoryDetail> datas)
        {
            //ClearCacheDetailInv();
            var DetailInvList = GetCacheDetailInv().ToList();
            var listId = new List<string>();
            foreach (GoodRequestDetail x in DetailInvList)
            {
                listId.Add(x.InvFaDetailId);
            }
            foreach (InventoryDetail data in datas)
            {
                if (!listId.Contains(data.Id))
                {
                    var detail = new GoodRequestDetail();
                    //DetailInvList = DetailInvList.Where(x => x.InvFaDetailId != data.Id).ToList();
                    //detail.Id = data.Id;
                    detail.InvFaDetailId = data.Id;
                    detail.qtyMax = data.Qty;
                    detail.WarehouseIdTo = "";
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
           
        [AcceptVerbs("Post")]
        public ActionResult GRDetail_Update([DataSourceRequest] DataSourceRequest request, GoodRequestDetail grd, string dateTrx)
        {
            if (grd != null && ModelState.IsValid)
            {
                //productService.Update(cad);
            }
            if (!string.IsNullOrEmpty(dateTrx))
            {
                grd.ReturnDate = DateTime.Parse(dateTrx, CultureInfo.InvariantCulture).Date;
            }
            //  return Json(new[] { grd }.ToDataSourceResult(request, ModelState));
            grd.RejectedOn = grd.CreatedOn;
            grd.ModifiedOn = DateTime.Today;
            return Json(new[] { grd }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            //return Json(new[] { fams }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult GRDetail_Destroy([DataSourceRequest] DataSourceRequest request, GoodRequestDetail grd)
        {
            if (grd != null && ModelState.IsValid)
            {
                //productService.Update(cad);
            }
            return Json(new[] { grd }.ToDataSourceResult(request, ModelState));
            //return Json(new[] { fams }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> GetBehalf(string behalf)
        {
            try
            {
                var model = await client.Get<GoodRequest>($"{url}/GetBehalf/{behalf}");
                return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

        }
        public async Task<IActionResult> GetDivLogin(string BasicLogin)
        {
            try
            {
                var model = await client.Get<GoodRequest>($"{url}/GetDivLogin/{BasicLogin}");
                return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

        }
        public async Task<ActionResult> GetInvOutType()
        {
            try
            {
                //ClearCacheDetailInv();
                //var model = new List<SelectListItem>();
                //if (type == "INVENTORY")
                //{
                //    model = new List<SelectListItem>()
                //            {
                //                new SelectListItem() {
                //                Text = "Return", Value ="Return"
                //                },
                //                new SelectListItem() {
                //                Text = "No Return", Value ="No Return"
                //                }
                //            }; 
                //    return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
                //}
                //else if (type == "FIXED ASSET")
                //{
                //    model = new List<SelectListItem>()
                //            {
                //                new SelectListItem() {
                //                Text = "Sale", Value ="Sale"
                //                },
                //                new SelectListItem() {
                //                Text = "Scrap/Broken", Value ="Scrap/Broken"
                //                },
                //                new SelectListItem() {
                //                Text = "Borrowed", Value ="Borrowed"
                //                }
                //            };
                //    return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
                //}
                //else
                //{
                //    return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
                //}

                var getUrl = $"{url}/GetMasterConditionProcurement";
                var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
                return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

    }
}