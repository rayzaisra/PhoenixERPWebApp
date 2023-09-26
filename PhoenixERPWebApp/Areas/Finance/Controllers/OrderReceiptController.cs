using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.WebNetCore;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.Enum;
using PhoenixERPWebApp.Models.Finance;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class OrderReceiptController : BaseController
    {
        string url = ApiUrl.OrderReceipt;
        string urlGoodRequest = ApiUrl.GoodRequest;
        private readonly ApiClientFactory client;

        public OrderReceiptController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail([FromQuery(Name = "id")] string Id)
        {
            var model = new OrderReceipt();
            ViewBag.Header = "Order Receipt";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.RecordID = Id;
                model = await client.Get<OrderReceipt>($"{url}/{Id}");
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await client.Get<List<OrderReceipt>>($"{url}") ?? new List<OrderReceipt>();
                var data = model.ToDataSourceResult(request);
                return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

        }

        public async Task<IActionResult> GetDataGoodRequest(string number)
        {
            try
            {
                var model = await client.Get<GoodRequest>($"{urlGoodRequest}/getByNumber/{number}");
                return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Save(OrderReceipt model, IFormFile ImageAttachmentUpload, IFormFile DocAttachmentUpload)
        {
            try
            {
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                var response = new ApiResponse();
                model.Status = OrderReceiptStatus.Open.ToString();
                if (!string.IsNullOrEmpty(model.Id))
                {   
                    response = await client.PutApiResponse<OrderReceipt>(url, model, ImageAttachmentUpload,DocAttachmentUpload);
                }
                else
                {   
                    response = await client.PostApiResponse<OrderReceipt>(url, model, ImageAttachmentUpload, DocAttachmentUpload);
                }

                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message.ToString() });
            }

        }

        [HttpPost]
        public async Task<IActionResult> Delivered(OrderReceipt model, IFormFile ImageAttachmentUpload, IFormFile DocAttachmentUpload)
        {
            try
            {
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                var response = new ApiResponse();
                model.Status = OrderReceiptStatus.Delivered.ToString();
                if (!string.IsNullOrEmpty(model.Id))
                {   
                    response = await client.PutApiResponse<OrderReceipt>(url, model, ImageAttachmentUpload, DocAttachmentUpload);
                }
                else
                {
                    response = await client.PostApiResponse<OrderReceipt>(url, model, ImageAttachmentUpload, DocAttachmentUpload);
                }

                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message.ToString() });
            }

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(OrderReceipt model) => Json(await client.DeleteApiResponse<OrderReceipt>($"{url}/{model.Id}"));
    }
}