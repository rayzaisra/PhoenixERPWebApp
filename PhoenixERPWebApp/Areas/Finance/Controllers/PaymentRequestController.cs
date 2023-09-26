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
using PhoenixERPWebApp.Models.Finance;
using PhoenixERPWebApp.Models.Finance.Transaction;
using PhoenixERPWebApp.Models.Finance.Transaction.BankPayment;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class PaymentRequestController : Controller
    {
        string url = ApiUrl.PaymentRequestUrl;
        private readonly ApiClientFactory client;

        public PaymentRequestController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(string Id, bool isAdvancePayment, bool isBankPayment)
        {
            var model = new PaymentRequestList();
            ViewBag.processApprove = false;
            ViewBag.IsBankPayment = isBankPayment;
            if (!string.IsNullOrEmpty(Id))
            {
                model = await client.Get<PaymentRequestList>($"{url}/{Id}");
                //jika sumber AP
                if (model.SourceDoc == SourceDoc.AccountPayable)
                {
                    if (model.PayToType == "3")//vendor
                    {
                        var vendorModel = await client.Get<Banks>($"{ApiUrl.PaymentRequestUrl}/GetVendorData/{model.PayToVendor}");
                        if (vendorModel != null)
                        {
                            model.PaytoBankName = vendorModel.BankName;
                            model.SwiftCode = vendorModel.SwiftCode;
                        }
                    }

                    //multiple ap
                    if (!string.IsNullOrEmpty(model.Id))
                    {
                        var detail = await client.Get<List<PaymentRequestDetail>>($"{ApiUrl.PaymentRequestUrl}/GetDetail/{model.Id}");
                        if (detail != null)
                        {
                            decimal? subtotal = 0;
                            foreach (PaymentRequestDetail data in detail)
                            {
                                subtotal += data.PayUnitPrice;
                            }
                            model.pd_NetAmount = subtotal;
                            model.pd_VatPercent = 10;
                            model.pd_Vat = model.pd_NetAmount * (model.pd_VatPercent / 100);
                            model.pd_GrandTotal = model.pd_NetAmount + model.pd_Vat;
                        }
                    }

                }
                else if (model.SourceDoc == SourceDoc.PettyCash)
                {
                }
                else if (model.SourceDoc == SourceDoc.SalaryPaymentRequest)
                {
                }
                else if (!string.IsNullOrEmpty(model.ReferenceNumberId))//sumber CA
                {
                    model.SourceDoc = SourceDoc.CashAdvance;
                    var modelca = await client.Get<CashAdvance>($"{ApiUrl.CashAdvanceUrl}/{model.ReferenceNumberId}");
                    if (modelca != null)
                    {
                        model.cashAdvanceList = modelca;
                        model.Ca_JobId = modelca.JobId;
                        model.Ca_CaPeriodFrom = modelca.CaPeriodFrom;
                        model.Ca_CaPeriodTo = modelca.CaPeriodTo;
                        model.Ca_CashAdvanceId = modelca.Id;
                        model.Ca_ClientName = modelca.ClientId;
                        model.Ca_JobNumber = modelca.PcaId;
                        model.Ca_LegalEntityId = modelca.LegalEntityId;
                        model.Ca_PayeeAccountName = modelca.PayeeAccountName;
                        model.Ca_PayeeAccountNumber = modelca.PayeeAccountNumber;
                        model.Ca_PayeeBankId = modelca.PayeeBankId;
                        model.Ca_ProjectActivity = modelca.MainserviceCategoryId;
                        model.Ca_VendorName = modelca.VendorName;
                        model.MainServiceCategoryId = modelca.MainserviceCategoryId;
                        model.Ca_pceId = modelca.PceId;
                        model.Ca_vendorId = modelca.VendorId;
                        model.Ca_VendorPIC = modelca.VendorPIC;
                        model.Ca_VendorTaxNumber = modelca.VendorTaxNumber;
                        model.Ca_VendorAddress = modelca.VendorAddress;
                        model.CaTotalAmount = modelca.Amount;
                        model.Ca_BusinessUnit = modelca.BusinessUnitId;
                        model.Ca_BrandId = modelca.ClientBrandId;
                        model.Ca_PCETypeId = modelca.PceTypeId;
                        model.Ca_ShareservicesId = modelca.ShareServiceId;
                        model.Ca_AffiliationId = modelca.AffiliationId;
                        model.PayToType = "3";//vendor
                        model.PayToVendor = modelca.VendorId;
                        model.PaytoBankName = modelca.PayeeAccountName;
                        model.PayToAccountNumber = modelca.PayeeAccountNumber;
                        model.Ca_CurrencyId = modelca.CurrencyId;
                        model.Ca_ExchangeRate = modelca.ExchangeRate;
                        model.Ca_Operator = modelca.Operator;
                        model.CurrencySymbolPca = modelca.CurrencySymbol;
                        model.CurrencySymbolCA = modelca.CurrencySymbolExchange;
                    }
                }

                ViewBag.processApprove = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.isRequestor = ViewBag.processApprove == false && (model.Status == PaymentRequestStatus.Open || model.Status == PaymentRequestStatus.Reject);

                if (ViewBag.processApprove == true)
                {
                    ViewBag.Header = "Approve Payment Request";
                }
                else if (isAdvancePayment == true)
                {
                    ViewBag.Header = "Update Payment Request";
                    model.isAdvancePayment = true;
                }
                else if (isBankPayment == true)
                {
                    ViewBag.Header = "Update Bank Payment";
                }
                else
                {
                    ViewBag.Header = "Update Payment Request";
                }
            }
            else
            {
                ViewBag.isRequestor = ViewBag.processApprove == false;
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                model.SourceDoc = SourceDoc.AccountPayable;
                ViewBag.Header = "Create Payment Request";
                model.PaymentRequestDate = DateTime.Now;
                if (!string.IsNullOrEmpty(requestor.BusinessUnitId))
                {
                    model.BusinessUnitId = requestor.BusinessUnitId;
                }
            }

            ViewBag.CreateJournal = ViewBag.isRequestor;
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, FilterPaymentRequest Filter)
        {
            var model = new List<BankPaymentList>();
            var response = new ApiResponse();
            response = await client.PostApiResponse<List<BankPaymentList>>($"{url}/GetPaymentRequestList/", Filter);
            if (response.Data != null)
            {
                model = response.Data as List<BankPaymentList>;
            }

            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        //Account Payable
        public async Task<IActionResult> ReadDetailAP([DataSourceRequest] DataSourceRequest request, string Id)
        {
            var model = await client.Get<List<AccountPayable>>($"{url}/GetListDetailAP/{Id}") ?? new List<AccountPayable>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        //Petty Cash
        public async Task<IActionResult> ReadDetailPC([DataSourceRequest] DataSourceRequest request, string Id)
        {
            var model = await client.Get<List<AccountPayable>>($"{url}/GetListDetailPC/{Id}") ?? new List<AccountPayable>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        //Salary Payment Request
        public async Task<IActionResult> ReadDetailSPR([DataSourceRequest] DataSourceRequest request, string Id)
        {
            var model = await client.Get<List<AccountPayable>>($"{url}/GetListDetailSPR/{Id}") ?? new List<AccountPayable>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadJournal([DataSourceRequest] DataSourceRequest request, string Id)
        {
            var model = await client.Get<List<Journal>>($"{ApiUrl.PaymentRequestUrl}/GetBankPaymentJournal/{Id}") ?? new List<Journal>();
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

        public IActionResult GetPayToType()
        {
            var result = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Petty Cash", Value = "1" },
                new SelectListItem() { Text = "Bank", Value = "2" },
                new SelectListItem() { Text = "Vendor", Value = "3", Disabled= true},
                new SelectListItem() { Text = "Other", Value = "4"}
            };
            return Json(result, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadAPPosted([DataSourceRequest] DataSourceRequest request, PaymentRequestFilter Filter)
        {
            var response = new ApiResponse();
            List<AccountPayable> model = new List<AccountPayable>();
            response = await client.PostApiResponse<List<AccountPayable>>($"{url}/GetListAPPosted/", Filter);
            if (response.Data != null) model = response.Data as List<AccountPayable>;
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountNumberData(string Id)
        {
            var model = await client.Get<Banks>($"{ApiUrl.PaymentRequestUrl}/GetAccountNumberData/{Id}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        public async Task<IActionResult> GetVendorData(string Id)
        {
            var model = await client.Get<Banks>($"{ApiUrl.PaymentRequestUrl}/GetVendorData/{Id}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Save(PaymentRequestList model)
        {
            model.Status = PaymentRequestStatus.Open;
            return await Process(model);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(PaymentRequestList model)
        {
            model.Status = PaymentRequestStatus.WaitingApproval;
            return await Process(model);
        }

        [HttpPut]
        public async Task<IActionResult> Approve(PaymentRequestList model)
        {
            var response = await client.PutApiResponse<PaymentRequestList>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPut]
        public async Task<IActionResult> Reconsiliations(PaymentRequestList model)
        {
            var response = await client.PutApiResponse<PaymentRequestList>($"{url}/Reconsiliation", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPut]
        public async Task<IActionResult> Reject(PaymentRequestList model)
        {
            var response = await client.PutApiResponse<PaymentRequestList>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public async Task<IActionResult> CreateJournal(PaymentRequestList model)
        {
            if (string.IsNullOrEmpty(model.Status)) { model.Status = PaymentRequestStatus.Open; }
            model.Details = SetDetail(model);
            var response = await client.PostApiResponse<PaymentRequestList>($"{url}/CreateJournal", model);
            var data = response.Data as PaymentRequestList;
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

        public async Task<IActionResult> Process(PaymentRequestList model)
        {
            var response = new ApiResponse();
            model.Details = SetDetail(model);
            if (!string.IsNullOrEmpty(model.Id))
            {
                if (model.isAdvancePayment == true)
                {
                    model.Status = PaymentRequestStatus.Open;
                    response = await client.PostApiResponse<PaymentRequestList>(url, model);
                }
                else
                    response = await client.PutApiResponse<PaymentRequestList>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<PaymentRequestList>(url, model);
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        private List<PaymentRequestDetail> SetDetail(PaymentRequestList model)
        {
            List<PaymentRequestDetail> modeldetail = new List<PaymentRequestDetail>();
            //if (model.PayToType == "1" || model.PayToType == "2" || model.PayToType == "4"){ model.SourceDoc = "GL"; }
            if (model.APList != null)
            {
                if (model.APList.Count > 0)
                {
                    foreach (AccountPayable data in model.APList)
                    {
                        PaymentRequestDetail pd = new PaymentRequestDetail();
                        pd.ReferenceId = data.Id;
                        pd.APNumber = data.AccountPayableNumber;
                        pd.Qty = 1;
                        pd.PayUnitPrice = data.Amount;
                        pd.CurrencyId = model.CurrencyId;
                        pd.Exchange = model.ExchangeRate;
                        pd.SouceDoc = SourceDoc.AccountPayable;
                        pd.JobId = data.JobId;
                        pd.Vat = data.Vat;
                        pd.TaxPayable = data.TaxPayable;
                        pd.JobName = data.JobName;
                        pd.TaxPayableCoaId = data.TaxPayableCoaId;
                        modeldetail.Add(pd);
                    }
                }
            }
            return modeldetail;
        }

        public async Task<IActionResult> ReadJournalEntry([DataSourceRequest] DataSourceRequest request, string id)
        {
            var model = await client.Get<List<PaymentRequestDetailJournal>>($"{url}/EntryJournalList/{id}") ?? new List<PaymentRequestDetailJournal>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }


        [AcceptVerbs("Post")]
        public ActionResult CreateJournalEntry([DataSourceRequest] DataSourceRequest request, PaymentRequestDetailJournal model)
        {
            if (model != null && ModelState.IsValid)
            {
                model.Id = Guid.NewGuid().ToString();
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult UpdateJournalEntry([DataSourceRequest] DataSourceRequest request, PaymentRequestDetailJournal model)
        {
            if (model != null && ModelState.IsValid)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult DestroyJournalEntry([DataSourceRequest] DataSourceRequest request, PaymentRequestDetailJournal model)
        {
            if (model != null)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}