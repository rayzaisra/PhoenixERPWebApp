using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CodeMarvel.Infrastructure.ModelShared;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.Enum;
using PhoenixERPWebApp.Models.HrisNew.Report;

namespace PhoenixERPWebApp.Areas.HrisNew.Controllers
{
    [Area(nameof(HrisNew))]
    public class DeclarationPayrollController : Controller
    {
        string url = ApiUrl.DeclarationPayrollUrl;
        string urlEmployee = ApiUrl.EmployeePayrollProfileUrl;
        private readonly ApiClientFactory client;
        string menuUnique = "DECPAYREQ";
        public DeclarationPayrollController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult IndexApproval()
        {
            return View();
        }
        public async Task<IActionResult> GetInfoEmployee(string Id)
        {
            var model = new InfoEmployee();
            try
            {
                model = await client.Get<InfoEmployee>($"{urlEmployee}/{Id}");

                return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<IActionResult> AddEdit(string Id, bool isApprove, bool isReadonly)
        {
            var model = new DeclarationPayrollHeader();
            var isCurrentApproval = false;
            if (!string.IsNullOrEmpty(Id))
            {
                if (isApprove == true)
                    ViewBag.Header = "Approve";
                else
                    ViewBag.Header = "Update";
                model = await client.Get<DeclarationPayrollHeader>($"{url}/{Id}");
                isCurrentApproval = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.isRequestor = isApprove == false && (model.Status == StatusTransaction.Draft || model.Status == StatusTransaction.Rejected);
                ViewBag.isReadonly = isReadonly;
            }
            else
            {
                ViewBag.Header = "Create";
                ViewBag.isRequestor = isApprove == false;
                ViewBag.isReadonly = false;
                model.Status = StatusTransaction.Draft;
            }

            ViewBag.processApprove = isApprove && isCurrentApproval;
            model.IsApprove = isApprove;
            //ViewBag.isHcAdmin = await client.Get<bool>($"{ApiUrl.ManageUserUrl}/IsHCAdmin");
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<DeclarationPayrollHeader>>(url) ?? new List<DeclarationPayrollHeader>();
            if (model.Count>0)
            {
                foreach (var item in model)
                {
                    item.StatusName = StatusTransaction.StatusName(item.Status);
                }
            }
            var data = model.ToDataSourceResult(request);
            
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadApproval([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<DeclarationPayrollHeader>>($"{url}/approvalList") ?? new List<DeclarationPayrollHeader>();
            if (model.Count > 0)
            {
                foreach (var item in model)
                {
                    item.StatusName = StatusTransaction.StatusName(item.Status);
                }
            }
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Approve(DeclarationPayrollHeader model)
        {
            model.Status = StatusTransaction.Approved;
            model.IsTmp = false;
            var response = await client.PutApiResponse<DeclarationPayrollHeader>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(IndexApproval)) });
        }

        [HttpPost]
        public async Task<IActionResult> Reject(DeclarationPayrollHeader model)
        {
            model.Status = StatusTransaction.Rejected;
            model.IsTmp = false;
            var response = await client.PutApiResponse<DeclarationPayrollHeader>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(IndexApproval)) });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(DeclarationPayrollHeader model)
        {
            model.Status = StatusTransaction.WaitingApproval;
            model.IsTmp = false;
            return await Process(model, false);
        }

        private async Task<IActionResult> Process(DeclarationPayrollHeader model, bool isDraft)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<DeclarationPayrollHeader>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<DeclarationPayrollHeader>(url, model);
            }

            if (isDraft)
            {
                return Json(new { success = response.Success, message = response.Message, data = response.Data as DeclarationPayrollHeader });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        public async Task<IActionResult> ReadDetail([DataSourceRequest] DataSourceRequest request, string month, string year, string componentId)
        {
            try
            {
                var model = new List<DeclarationPayrollDetail>();
                model = await client.Get<List<DeclarationPayrollDetail>>($"{url}/getDeclarationDetail/{month}/{year}/{componentId}") ?? new List<DeclarationPayrollDetail>();
                var data = model.ToDataSourceResult(request);
                return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

        }

        [AcceptVerbs("Post")]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, DeclarationPayrollDetail model)
        {
            if (model != null && ModelState.IsValid)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, DeclarationPayrollDetail model)
        {
            if (model != null)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Save(DeclarationPayrollHeader Header, List<DeclarationPayrollDetail> Details)
        {
            try
            {
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                var response = new Phoenix.WebExtension.RestApi.ApiResponse();
                var model = new DeclarationPayrollDto();
                model.Header = Header;
                model.Header.Status = GoodRequestStatus.Open;
                model.Details = Details;
                if (!string.IsNullOrEmpty(Details[0].DeclarationPayrollHeaderId))
                {
                    Header.Id = Details[0].DeclarationPayrollHeaderId;
                }

                if (!string.IsNullOrEmpty(Header.Id))
                {
                    response = await client.PutApiResponse<DeclarationPayrollDto>(url, model);
                }
                else
                {
                    response = await client.PostApiResponse<DeclarationPayrollDto>(url, model);
                }

                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message.ToString() });
            }

        }

        //Additional
        [AcceptVerbs("Post")]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, DeclarationPayrollDetail model)
        {
            if (model != null && ModelState.IsValid)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(DeclarationPayrollHeader model) => Json(await client.DeleteApiResponse<DeclarationPayrollHeader>($"{url}/{model.Id}"));

    }
}