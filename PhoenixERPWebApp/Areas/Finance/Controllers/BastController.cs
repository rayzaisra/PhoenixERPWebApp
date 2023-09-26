using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CodeMarvel.Infrastructure;
using CodeMarvel.Infrastructure.Sessions;
using CodeMarvel.Infrastructure.WebNetCore;
using Microsoft.Extensions.Configuration;
using CodeMarvel.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models.Finance;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PhoenixERPWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering; 
using PhoenixERPWebApp.Models.Finance.Transaction.ServiceReceipt;


namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class BASTController : BaseController
    {
        string url = ApiUrl.BAST;
        string urlPO = ApiUrl.PurchaseOrderUrl;
        private readonly ApiClientFactory client;

        public BASTController(ApiClientFactory client)
        {
            this.client = client;
        }
                
        public ActionResult Index()
        {
            return View();
        }
                
        public async Task<ActionResult> Detail([FromQuery(Name = "id")] string Id)
        {
            var model = new Bast();
            ViewBag.Header = "BAST";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.RecordID = Id;
                model = await client.Get<Bast>($"{url}/{Id}");
            }
            return View(model);
        }

        public async Task<IActionResult> GetDataPO(string poId)
        {
            var model = await client.Get<PurchaseOrder>($"{urlPO}/{poId}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> GetBastLog([DataSourceRequest] DataSourceRequest request, string bastId)
        {
            var model = await client.Get<List<Models.Finance.BAST.BastLog>>($"{url}/getBASTLog?Id=" + bastId) ?? new List<Models.Finance.BAST.BastLog>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                //List<InventoryGoodsReceipt> model = new List<InventoryGoodsReceipt>();

                var model = await client.Get<List<Bast>>($"{url}") ?? new List<Bast>();
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

        public async Task<IActionResult> ReadDetail([DataSourceRequest] DataSourceRequest request, string bastId, string type)
        {
            try
            {   
                var model = await client.Get<List<BastDetail>>($"{url}/getDetail/{bastId}/{type}") ?? new List<BastDetail>();
                var data = model.ToDataSourceResult(request);
                return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

        }

        public async Task<IActionResult> GetDDLServiceReceiptDetailByPO(string poId)
        {
            try
            {
                var model = await client.Get<List<SelectListItem>>($"{url}/getDDLServiceReceiptDetailByPO/{poId}") ?? new List<SelectListItem>();
                return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

        }

        public async Task<IActionResult> GetDDLGoodReceiptDetailByPO(string poId)
        {
            try
            {
                var model = await client.Get<List<SelectListItem>>($"{url}/getDDLGoodReceiptDetailByPO/{poId}") ?? new List<SelectListItem>();
                return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

        }

        public async Task<IActionResult> GetSRDetailData(string srdId)
        {
            var data = await client.Get<ServiceReceiptDetail>($"{url}/GetSRDetailData/{srdId}") ?? new ServiceReceiptDetail();
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> GetGRDetailData(string grdId)
        {
            var data = await client.Get<InventoryGoodsReceiptDetail>($"{url}/GetGRDetailData/{grdId}") ?? new InventoryGoodsReceiptDetail();
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult GoodReceiptBast_Create([DataSourceRequest] DataSourceRequest request, BastDetail bd)
        {
            if (bd != null && ModelState.IsValid)
            {
                bd.Id = Guid.NewGuid().ToString();
            }
            //return Json(new[] { bd }.ToDataSourceResult(request, ModelState));
            return Json(new[] { bd }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult GoodReceiptBast_Update([DataSourceRequest] DataSourceRequest request, BastDetail bd)
        {
            if (bd != null && ModelState.IsValid)
            {
                //productService.Update(cad);
            }
            return Json(new[] { bd }.ToDataSourceResult(request, ModelState));
            //return Json(new[] { bd }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult GoodReceiptBast_Destroy([DataSourceRequest] DataSourceRequest request, BastDetail bd)
        {
            if (bd != null && ModelState.IsValid)
            {
                //productService.Update(cad);
            }
            return Json(new[] { bd }.ToDataSourceResult(request, ModelState));
            //return Json(new[] { bd }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult ServiceReceiptBast_Create([DataSourceRequest] DataSourceRequest request, BastDetail bd)
        {
            if (bd != null && ModelState.IsValid)
            {
                bd.Id = Guid.NewGuid().ToString();
            }
            //return Json(new[] { bd }.ToDataSourceResult(request, ModelState));
            return Json(new[] { bd }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult ServiceReceiptBast_Update([DataSourceRequest] DataSourceRequest request, BastDetail bd)
        {
            if (bd != null && ModelState.IsValid)
            {
                //productService.Update(cad);
            }
            return Json(new[] { bd }.ToDataSourceResult(request, ModelState));
            //return Json(new[] { bd }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult ServiceReceiptBast_Destroy([DataSourceRequest] DataSourceRequest request, BastDetail bd)
        {
            if (bd != null && ModelState.IsValid)
            {
                //productService.Update(cad);
            }
            return Json(new[] { bd }.ToDataSourceResult(request, ModelState));
            //return Json(new[] { bd }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Save(Bast Header, List<BastDetail> Details)
        {
            try
            {
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                var response = new ApiResponse();
                var model = new BastDTO();
                model.Header = Header;
                model.Details = Details;

                if (!string.IsNullOrEmpty(Header.Id))
                {
                    model.Header.CreatedBy = requestor.EmployeeId;
                    model.Header.CreatedOn = DateTime.Now;
                    response = await client.PutApiResponse<BastDTO>(url, model);
                }
                else
                {
                    response = await client.PostApiResponse<BastDTO>(url, model);
                }

                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message.ToString() });
            }

        }

        [HttpPost]
        public async Task<IActionResult> Closed(Bast Header, List<BastDetail> Details)
        {
            try
            {
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                var response = new ApiResponse();
                var model = new BastDTO();
                Header.ApprovedOn = DateTime.Now;
                Header.ApprovedBy = requestor.EmployeeId;
                model.Header = Header;
                model.Details = Details;
                response = await client.PutApiResponse<BastDTO>($"{url}/closed", model);
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message.ToString() });
            }

        }

        [HttpPost]
        public async Task<IActionResult> Rejected(Bast Header)
        {
            try
            {
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                var response = new ApiResponse();
                var model = new BastDTO();
                Header.RejectedOn = DateTime.Now;
                Header.RejectedBy = requestor.EmployeeId;
                model.Header = Header;
                //model.Details = Details;
                response = await client.PutApiResponse<BastDTO>($"{url}/rejected", Header);
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message.ToString() });
            }

        }
    }
}