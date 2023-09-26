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
using Phoenix.Data.Models.Finance.Transaction;
using Phoenix.WebExtension.Extentions;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.Finance.Transaction.Inventory;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class InventoryController : BaseController
    {
        string uniqueDetailInv = "DETAILINV";
        string url = ApiUrl.Inventory;
        private readonly ApiClientFactory client;

        public InventoryController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            ClearCacheDetailInv();
            return View();
        }

        public async Task<IActionResult> Detail(string Id, string remark = null)
        {
            ClearCacheDetailInv();
            var model = new Inventory();
            ViewBag.Header = "Inventory Form";
            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.RecordID = Id;
                model = await client.Get<Inventory>($"{url}/{Id}");
            }
            model.InventoryDetail = new InventoryDetail();
            if (!string.IsNullOrEmpty(remark))
            {
                model.Remark = remark;
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> submit(Inventory Header, List<InventoryDetail> Details, IFormFile file)
        {
            try
            {
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                var response = new ApiResponse();
                var model = new InventoryDTO();
                model.Header = Header;
                model.Header.InventoryInDateTime = DateTime.Now;
                model.Header.InventoryDetails = new List<InventoryDetail>();
                model.InventoryDetails = Details;


                if (!string.IsNullOrEmpty(Header.Id))
                {
                    response = await client.PutApiResponse<InventoryDTO>(url, model, file);
                }
                else
                {
                    response = await client.PostApiResponse<InventoryDTO>(url, model, file);

                }

                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message.ToString() });
            }

        }


        /// <summary>
        /// Ini di gunakan untuk Mengolah data detail Inventory
        /// </summary>
        /// <param name="model"></param>
        /// <param name="FileAttachmentUpload"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> SaveDetailPersist(Inventory model, IFormFile FileAttachmentUpload)
        {
            try
            {
                var response = new ApiResponse();

                if (!string.IsNullOrEmpty(model.InventoryDetail.Id))
                {
                    var data = model.InventoryDetail;
                    var DetailInvList = GetCacheDetailInv().Where(x => x.Id != model.InventoryDetail.Id).ToList();
                    var detail = new InventoryDetail();
                    detail.Id = data.Id;
                    detail.DateReceipt = data.DateReceipt;
                    detail.Manufacturer = data.Manufacturer;
                    detail.Brand = data.Brand;
                    detail.Model = data.Model;
                    detail.ProductNumber = data.ProductNumber;
                    detail.Color = data.Color;
                    detail.Condition = data.Condition;
                    detail.Qty = data.Qty;
                    detail.Location = data.Location;
                    detail.UomId = data.UomId;
                    detail.UnitName = data.UnitName;
                    detail.Location = data.Location;
                    detail.ConditionName = data.ConditionName;
                    detail.LocationName = data.LocationName;
                    detail.Remark = data.Remark;
                    detail.LeadTime = data.LeadTime;
                    detail.Status = data.Status;
                    detail.status_name = data.status_name;
                    detail.ItemName = data.ItemName;
                    detail.VendorBrandId = data.VendorBrandId;
                    detail.VendorNameId = data.VendorNameId;
                    detail.UnitOfMeasure = data.UnitOfMeasure;
                    if (FileAttachmentUpload != null)
                    {
                        detail.FileUpload = FileAttachmentUpload;
                        // detail.FileAttachmentName = FileAttachmentUpload.FileName;
                    }
                    DetailInvList.Add(detail);
                    SetCacheDetailInv(DetailInvList);
                    return Json(new { success = true, message = "success", refreshGrid = "grid", hideModalPopUp = "modalDetailInv" });
                }
                else
                {
                    var data = model.InventoryDetail;
                    data.Id = Guid.NewGuid().ToString();
                    var DetailInvList = GetCacheDetailInv().Where(x => x.Id != data.Id).ToList();
                    var detail = new InventoryDetail();
                    detail.Id = data.Id;
                    detail.DateReceipt = data.DateReceipt;
                    detail.Manufacturer = data.Manufacturer;
                    detail.Brand = data.Brand;
                    detail.Model = data.Model;
                    detail.ProductNumber = data.ProductNumber;
                    detail.Color = data.Color;
                    detail.Condition = data.Condition;
                    detail.Qty = data.Qty;
                    detail.Location = data.Location;
                    detail.UomId = data.UomId;
                    detail.UnitName = data.UnitName;
                    detail.Location = data.Location;
                    detail.ConditionName = data.ConditionName;
                    detail.LocationName = data.LocationName;
                    detail.Remark = data.Remark;
                    detail.LeadTime = data.LeadTime;
                    detail.Status = data.Status;
                    detail.status_name = data.status_name;
                    detail.ItemName = data.ItemName;
                    detail.VendorBrandId = data.VendorBrandId;
                    detail.VendorNameId = data.VendorNameId;
                    detail.UnitOfMeasure = data.UnitOfMeasure;
                    if (FileAttachmentUpload != null)
                    {
                        detail.FileUpload = FileAttachmentUpload;
                        detail.FileAttachmentName = FileAttachmentUpload.FileName;
                    }
                    DetailInvList.Add(detail);
                    SetCacheDetailInv(DetailInvList);
                }

                return Json(new { success = true, message = "success", refreshGrid = "grid", hideModalPopUp = "modalDetailInv" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }

        private void ClearCacheDetailInv()
        {
            CacheStore.Remove<List<InventoryDetail>>(uniqueDetailInv);
        }

        public void SetCacheDetailInv(List<InventoryDetail> models)
        {
            ClearCacheDetailInv();
            CacheStore.Add<List<InventoryDetail>>(uniqueDetailInv, models);
        }

        public List<InventoryDetail> GetCacheDetailInv()
        {
            var models = CacheStore.Get<List<InventoryDetail>>(uniqueDetailInv);
            if (models == null)
                models = new List<InventoryDetail>();
            return models;
        }

        [HttpPost]
        public IActionResult SaveDetail(Inventory model, IFormFile FileAttachmentUpload)
        {
            var data = model.InventoryDetail;
            var DetailInvList = GetCacheDetailInv().Where(x => x.Id != data.Id).ToList();
            var detail = new InventoryDetail();
            detail.Id = data.Id;
            detail.DateReceipt = data.DateReceipt;
            detail.Manufacturer = data.Manufacturer;
            detail.Brand = data.Brand;
            detail.Model = data.Model;
            detail.ProductNumber = data.ProductNumber;
            detail.Color = data.Color;
            detail.Condition = data.Condition;
            detail.Qty = data.Qty;
            detail.Location = data.Location;
            detail.UomId = data.UomId;
            detail.UnitName = data.UnitName;
            detail.Location = data.Location;
            detail.ConditionName = data.ConditionName;
            detail.LocationName = data.LocationName;
            detail.Remark = data.Remark;
            detail.UnitOfMeasure = data.UnitOfMeasure;

            if (FileAttachmentUpload != null)
            {
                detail.FileUpload = FileAttachmentUpload;
                detail.FileAttachmentName = FileAttachmentUpload.FileName;
            }
            DetailInvList.Add(detail);
            SetCacheDetailInv(DetailInvList);

            return Json(new { success = true, message = "success", refreshGrid = "grid", hideModalPopUp = "modalDetailInv" });
        }

        public IActionResult ReadDetailList([DataSourceRequest] DataSourceRequest request)
        {
            var model = GetCacheDetailInv();
            //if (!isRequestor)
            //    model = model.Where(x => x.IsRequestPurchaseOrder == true).ToList();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<Inventory>>($"{url}") ?? new List<Inventory>();

            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadDetail([DataSourceRequest] DataSourceRequest request, string invId)
        {
            var model = new List<InventoryDetail>();
            if (GetCacheDetailInv().Count > 0)
            {
                model = GetCacheDetailInv();
            }
            else
            {
                model = await client.Get<List<InventoryDetail>>($"{url}/getDetail/{invId}") ?? new List<InventoryDetail>();
                SetCacheDetailInv(model);
            }
            // model = await client.Get<List<InventoryDetail>>($"{url}/getDetail/{invId}") ?? new List<InventoryDetail>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadDetailByGoodReceipt([DataSourceRequest] DataSourceRequest request, string grId)
        {
            var model = await client.Get<List<InventoryDetail>>($"{url}/getDetailByGoodReceipt/{grId}") ?? new List<InventoryDetail>();

            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public IActionResult DDlGetFrom()
        {
            var result = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Refurbish", Value = "Refurbish" },
                new SelectListItem() { Text = "Gift", Value = "Gift" },
                new SelectListItem() { Text = "Other", Value = "Other"},

            };
            return Json(result, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        [AcceptVerbs("Post")]
        public ActionResult Editing_Detail_Update([DataSourceRequest] DataSourceRequest request, InventoryDetail igrd)
        {
            if (igrd != null && ModelState.IsValid) { }
            return Json(new[] { igrd }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Inventory entyty) => Json(await client.DeleteApiResponse<Inventory>($"{url}/{entyty.Id}"));

       

    }
}