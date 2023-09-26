using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.Attributes;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models.Finance.Transaction.PaymentRequest;
using System.Xml;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.Enum;
using CodeMarvel.Infrastructure.WebNetCore;
using PhoenixERPWebApp.Models.Finance;
using PhoenixERPWebApp.Models.Finance.Transaction;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class BankReceivedController : Controller
    {
        string url = ApiUrl.BankReceivedUrl;
        private readonly ApiClientFactory client;

        public BankReceivedController(ApiClientFactory client)
        {
            this.client = client;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(string Id)
        {
            var model = new BankReceived();
            ViewBag.processApprove = false;
            if (!string.IsNullOrEmpty(Id))
            {
                model = await client.Get<BankReceived>($"{url}/{Id}");

                //jika sumber AP
                if (model.SourceDoc == "AR")
                {
                    //multiple ap
                    if (!string.IsNullOrEmpty(model.Id))
                    {
                        var detail = model.Details;
                        if (detail != null)
                        {
                            model.pd_NetAmount = detail.Sum(x => x.Amount);
                            model.pd_VatPercent = 10;
                            model.pd_Vat = model.pd_NetAmount * (model.pd_VatPercent / 100);
                            model.pd_GrandTotal = model.pd_NetAmount + model.pd_Vat;
                        }
                    }
                }

                ViewBag.processApprove = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.isRequestor = ViewBag.processApprove == false && (model.Status == PaymentRequestStatus.Open || model.Status == PaymentRequestStatus.Reject);
                if (ViewBag.processApprove == true)
                {
                    ViewBag.Header = "Approve Bank Received";
                }
                else
                {
                    ViewBag.Header = "Update Bank Received";
                }
            }
            else
            {
                ViewBag.isRequestor = ViewBag.processApprove == false;
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                model.SourceDoc = SourceDoc.AccountReceivable;
                ViewBag.Header = "Create Bank Received";
                model.PaymentRequestDate = DateTime.Now;
                model.Status = PaymentRequestStatus.Open;
                model.PayToType = "4";//default typenya others permintaan mas daus 24/07/2020 
                model.Type = "2"; //bank
                if (!string.IsNullOrEmpty(requestor.BusinessUnitId))
                {
                    model.BusinessUnitId = requestor.BusinessUnitId;
                }
            }
            ViewBag.CreateJournal = ViewBag.isRequestor;
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, FilterBankReceived Filter)
        {
            var model = new List<BankReceivedList>();
            var response = new ApiResponse();
            response = await client.PostApiResponse<List<BankReceivedList>>($"{url}/GetBankReceivedList/", Filter);
            if (response.Data != null)
            {
                model = response.Data as List<BankReceivedList>;
            }

            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadDetailAR([DataSourceRequest] DataSourceRequest request, string bankReceivableId)
        {
            var response = new ApiResponse();
            var model = await client.Get<List<AccountReceivable>>($"{url}/GetListDetailAR/{bankReceivableId}") ?? new List<AccountReceivable>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }


        public async Task<IActionResult> ReadARPosted([DataSourceRequest] DataSourceRequest request, PaymentRequestFilter Filter)
        {
            var response = new ApiResponse();
            List<AccountReceivable> model = new List<AccountReceivable>();
            response = await client.PostApiResponse<List<AccountReceivable>>($"{url}/GetListARPosted/", Filter);
            if (response.Data != null) { model = response.Data as List<AccountReceivable>; }
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadJournal([DataSourceRequest] DataSourceRequest request, string bankReceivableId)
        {
            var model = await client.Get<List<Journal>>($"{url}/GetBankReceivedJournal/{bankReceivableId}") ?? new List<Journal>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> updateJournal([DataSourceRequest] DataSourceRequest request, Journal model)
        {
            if (model != null && ModelState.IsValid)
            {
                var response = await client.PutApiResponse<Journal>($"{url}/UpdateJournal", model);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountNumberData(string Id)
        {
            var model = await client.Get<Banks>($"{url}/GetAccountNumberData/{Id}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        public async Task<IActionResult> GetVendorData(string Id)
        {
            var model = await client.Get<Banks>($"{url}/GetVendorData/{Id}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public IActionResult GetPayToType()
        {
            var result = new List<SelectListItem>()
                {
                    new SelectListItem() { Text = "Petty Cash", Value = "1" },
                    new SelectListItem() { Text = "Bank", Value = "2" },
                    new SelectListItem() { Text = "Client", Value = "3"},
                    new SelectListItem() { Text = "Other", Value = "4"}
                };
            return Json(result, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public IActionResult GetType()
        {
            var result = new List<SelectListItem>()
                {
                    new SelectListItem() { Text = "Petty Cash", Value = "1" },
                    new SelectListItem() { Text = "Bank", Value = "2" }
                };
            return Json(result, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Save(BankReceived model)
        {
            model.Status = PaymentRequestStatus.Open;
            return await Process(model);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(BankReceived model)
        {
            model.Status = PaymentRequestStatus.WaitingApproval;
            return await Process(model);
        }

        [HttpPut]
        public async Task<IActionResult> Approve(BankReceived model)
        {
            var response = await client.PutApiResponse<BankReceived>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPut]
        public async Task<IActionResult> Reconsiliations(BankReceived model)
        {
            var response = await client.PutApiResponse<BankReceived>($"{url}/Reconsiliation", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPut]
        public async Task<IActionResult> Reject(BankReceived model)
        {
            var response = await client.PutApiResponse<BankReceived>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public async Task<IActionResult> CreateJournal(BankReceived model)
        {
            if (string.IsNullOrEmpty(model.Status)) { model.Status = PaymentRequestStatus.Open; }
            model.Details = SetDetail(model);
            var response = await client.PostApiResponse<BankReceived>($"{url}/CreateJournal", model);
            var data = response.Data as BankReceived;
            var setHiddenValue = new[]{ new { id = "InvoiceReceivableId", value = data.InvoiceReceivableId },
                                        new { id = "LegalEntityId", value = data.LegalEntityId },
                                        new { id = "JobId", value = data.JobId },
                                        new { id = "PurchaseRequestId", value = data.PurchaseRequestId },
                                        new { id = "ReferenceNumberId", value = data.ReferenceNumberId },
                                        new { id = "DueDate", value = Convert.ToDateTime(data.DueDate).ToString("yyyy-MM-dd") },
                                        new { id = "PaymentRequestNumber", value = data.PaymentRequestNumber },
                                        new { id = "PaymentRequestDate", value =  Convert.ToDateTime(data.PaymentRequestDate).ToString("yyyy-MM-dd") },
                                        new { id = "SourceDoc", value = data.SourceDoc },
                                        new { id = "Ca_CashAdvanceId", value = data.Ca_CashAdvanceId },
                                        new { id = "PairingGroupId", value = data.PairingGroupId },
                                        new { id = "isAdvancePayment", value = data.isAdvancePayment.ToString() },
                                        new { id = "Balance", value = data.Balance.ToString() },
                                        new { id = "Inv_VendorId", value = data.Inv_VendorId },
                                        new { id = "Status", value = data.Status }};
            return Json(new { success = response.Success, message = response.Message, refreshGrid = "gridjournal;gridstatuslogHistoryList", setHiddenValue = setHiddenValue, data = data });
        }


        public async Task<IActionResult> Process(BankReceived model)
        {
            var response = new ApiResponse();
            model.Details = SetDetail(model);
            if (!string.IsNullOrEmpty(model.Id))
            {
                if (model.isAdvancePayment == true)
                {
                    model.Status = PaymentRequestStatus.Open;
                    response = await client.PostApiResponse<BankReceived>(url, model);
                }
                else
                    response = await client.PutApiResponse<BankReceived>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<BankReceived>(url, model);
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        private List<BankReceivedDetail> SetDetail(BankReceived model)
        {
            List<BankReceivedDetail> modeldetail = new List<BankReceivedDetail>();
            model.ReferenceNumberId = "";
            model.JobId = "";
            model.AffilitionId = "";
            //if (model.PayToType == "3")  { model.SourceDoc = "AR"; } else { model.SourceDoc = "GL"; }

            if (model.ARList != null)
            {
                if (model.ARList.Count > 0)
                {
                    model.JobId = model.ARList[0].JobId;
                    model.ReferenceNumberId = model.ARList[0].AccountReceivableNumber;

                    foreach (AccountReceivable data in model.ARList)
                    {
                        BankReceivedDetail pd = new BankReceivedDetail();
                        pd.AccountReceivebleNumber = data.AccountReceivableNumber;
                        pd.LegalEntityId = data.LegalEntityId;
                        pd.BusinessUnitId = data.BusinessUnitId;
                        pd.AffiliationId = data.AffilitionId;
                        pd.AccountId = data.AccountId;
                        pd.BrandId = data.BrandId;
                        pd.JobId = data.JobId;
                        pd.TransactionDate = data.TransactionDate;
                        pd.DueDate = data.DueDate;
                        pd.CurrencyId = data.CurrencyId;
                        pd.Amount = data.Amount;
                        pd.ExchangeRate = data.ExchangeRate;
                        pd.BankAccountDestination = data.BankAccountDestination;
                        pd.Charges = data.Charges;
                        pd.ClientId = data.ClientId;
                        pd.SourceDoc = "AR";
                        modeldetail.Add(pd);
                    }
                }
            }

            return modeldetail;
        }


        public async Task<IActionResult> ReadJournalEntry([DataSourceRequest] DataSourceRequest request, string id)
        {
            var model = await client.Get<List<BankReceivedDetailJournal>>($"{url}/EntryJournalList/{id}") ?? new List<BankReceivedDetailJournal>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult CreateJournalEntry([DataSourceRequest] DataSourceRequest request, BankReceivedDetailJournal model)
        {
            if (model != null && ModelState.IsValid)
            {
                model.Id = Guid.NewGuid().ToString();
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult UpdateJournalEntry([DataSourceRequest] DataSourceRequest request, BankReceivedDetailJournal model)
        {
            if (model != null && ModelState.IsValid)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult DestroyJournalEntry([DataSourceRequest] DataSourceRequest request, BankReceivedDetailJournal model)
        {
            if (model != null)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}
