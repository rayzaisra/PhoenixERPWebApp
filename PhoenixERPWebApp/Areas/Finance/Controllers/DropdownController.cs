using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area(nameof(Finance))]
    public class DropdownController : Controller
    {
        private readonly ApiClientFactory client;

        public DropdownController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetDivision()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/Division";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDivisionHris()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/DivisionHris";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDivisionFinance()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/DivisionFinance";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetPNLReportDivisionFinance()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/DivisionFinance";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            model.Add(new SelectListItem { Text = "All Business Unit", Value = "0000000000" });
            return Json(model.OrderBy(x => x.Value), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetApNumber()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/ApNumber";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetArNumber(string bankReceivedId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/ArNumber/{bankReceivedId ?? "0"}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetMainServiceCategory()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetMainServiceCategory";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDepartment(string id)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/Departments/{id}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetSubgroup()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/SubGroups";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public ActionResult GetDDLLeaveType()
        {
            var model = new List<SelectListItem>() {
                            new SelectListItem() { Text = LeaveType.LeaveTypeName(LeaveType.Annual), Value = LeaveType.Annual.ToString() },
                            new SelectListItem() { Text = LeaveType.LeaveTypeName(LeaveType.Compensatory), Value = LeaveType.Compensatory.ToString() },
                            new SelectListItem() { Text = LeaveType.LeaveTypeName(LeaveType.Clean), Value = LeaveType.Clean.ToString() }};
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLCurrency()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/Currency";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            //var defaultValue = model.Where(x => x.Value == CurrencyIDR.CurrencyId).ToList();
            //var ListValue = model.Where(x => x.Value != CurrencyIDR.CurrencyId).ToList();
            //var result = new List<SelectListItem>();
            //result.AddRange(defaultValue);
            //result.AddRange(ListValue);
            //return Json(result, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLCurrencyParamsAndDefault(string currencyId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/CurrencyParamsAndDefault/{currencyId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            //var defaultValue = model.Where(x => x.Value == CurrencyIDR.CurrencyId).ToList();
            //var ListValue = model.Where(x => x.Value != CurrencyIDR.CurrencyId).ToList();
            //var result = new List<SelectListItem>();
            //result.AddRange(defaultValue);
            //result.AddRange(ListValue);
            //return Json(result, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetEfaktur(string legalEntityId, string invoiceDate, string vatNo)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/efaktur/{legalEntityId ?? "0"}/{invoiceDate ?? "0"}/{vatNo}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLCurrencyIsNoDefault()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/currencyIsNoDefault";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLClient(string id = null)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/client/{id}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLReference()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/TAPDDLList";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLVendor(string id = null)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/vendor/{id}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLRFQVendors(string rfqId, string vendorId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/rfqvendor/{rfqId}/{vendorId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetPOClient(string clientId, string invoiceId, string text)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/PoClient/{clientId}/{invoiceId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            if (!string.IsNullOrEmpty(text)) model = model.Where(x => x.Text.ToLower().Contains((text ?? "").ToLower())).ToList();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetPOByVendorInGoodReceipt(string clientId, string grId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetPoByVendorInGoodReceipt/{clientId}/{grId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetInvoiceVendorPO(string poId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/invoiceVendor/{poId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLCompany()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/company";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        public async Task<ActionResult> GetDDLBrand()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/brand";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLCOA(string param1, string level)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/coa/{param1}/{level}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLItemType()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/itemtype";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetCOALevel(string level)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/COALevel/{level}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetCOAByNameLv5()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/COAByNameLv5";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetCOABank()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/COABank";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetCOAOpeningBalance()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetCOAOpeningBalance";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetCOAByNameAPLv5()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/COAByNameAPLv5";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetCOAByNameARLv5()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/COAByNameARLv5";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetCOALV5()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/coalv5";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        //2.1.03.01.
        public async Task<ActionResult> GetCOALV5Like(string search)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/coalv5like/{search}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetLegalEntity()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/LegalEntity";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetJobId()
        {
            var getUrl = $"{ApiUrl.financedropdownUrl}/get_job_id";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetCompanys()
        {
            var getUrl = $"{ApiUrl.financedropdownUrl}/get_companys";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        public async Task<ActionResult> GetPaymentType()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/PaymentType";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLItemCode(string ItemTypeId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetItemCode/{ItemTypeId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        public async Task<ActionResult> GetDDLItemInventoryAndFixedAsset(string ItemTypeId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetItemInventoryAndFixedAsset/{ItemTypeId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLJobId()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/JobId";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetJobByPCEApproved()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetJobByPCEApproved";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLPurcasheRequest(string purchaseId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/PurchaseRequestNumber/{purchaseId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        public async Task<ActionResult> GetDDLUnit()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/unit";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLJobInRFQ()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetJobDetailInRFQTask";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLRFQOpenOP()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetRFQOpenPO";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLSpecialRequest()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetSpecialRequestPOType";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLBrandWithCompany(string company_id)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetBrands/{company_id}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLBusinessUnitByAffId(string affiliationid)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetBusiness/{affiliationid}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLBusinessUnitFinanceByAffId(string affiliationid)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetBusinessFinance/{affiliationid}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLBusinessUnitHRISByAffId(string affiliationid)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetBusinessHris/{affiliationid}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetAffiliationByIdAsset(string id)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetAffiliationByIdAsset/{id}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLStatus()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetStatus";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetStatusRFQ()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetStatusRFQ";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLStatusPO()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetStatusPO";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetStatusInvoiceReceived()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetStatusInvoiceReceived";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetStatusTAP()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetStatusTAP";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }


        public async Task<ActionResult> GetStatusAP()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetStatusAP";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetStatusPaymentRequest()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetStatusPaymentRequest";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetInvoicing()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetInvoicing";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLInventoryGoodsReceipt(string poId, string text)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetInventoryGoodsReceipt/{poId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            if (!string.IsNullOrEmpty(text)) model = model.Where(x => x.Text.ToLower().Contains((text ?? "").ToLower())).ToList();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetServiceReceipt(string poId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetServiceReceipt/{poId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLGoodRequest()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetGoodRequest";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLOrderReceipt(string goodrequest_id)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetOrderReceipt/{goodrequest_id}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetCoaInvoice()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetCoaInvoice";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetCoaInvoiceTaxPayable()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetCoaInvoiceTaxPayable";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetInvoiceClientStatus()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetInvoiceClientStatus";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetArStatus()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetArStatus";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetInvoiceDeliveryNoteStatus()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetInvoiceDeliveryNoteStatus";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetCoaInvoiceClient()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetCoaInvoiceClient";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLBanksList()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetBanks";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLBankByLegalEntity(string legalEntityId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetBankByLegalEntity/{legalEntityId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLVendorInv(string id)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/vendorinv/{id}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        public async Task<ActionResult> GetDDLClientInv(string id = null)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/clientinv/{id}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDLLTypeOfExpense(string id = null)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/TypeOfExpense/{id}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        //public async Task<ActionResult> GetDLLBankReceivedType(string id = null)
        //{
        //    var getUrl = $"{ApiUrl.dropdownUrl}/GetBankReceivedType/{id}";
        //    var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
        //    return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        //}

        public async Task<ActionResult> GetPCA()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetPCA";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetPCE()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetPCE";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        public async Task<ActionResult> GetYearList()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/YearList";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetYearPeriod(string legalEntityId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetYearPeriod/{legalEntityId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetFiscalYear()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetFiscalYear";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetFiscalYearByLegalId(string legalId)
        {
            if (string.IsNullOrEmpty(legalId))
                legalId = "-";

            var getUrl = $"{ApiUrl.dropdownUrl}/GetFiscalYearByLegalId/{legalId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });

        }

        public async Task<ActionResult> GetFiscalPeriod(string Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                var getUrl = $"{ApiUrl.dropdownUrl}/GetFiscalPeriod/{Id}";
                var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
                return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }

            else
            {
                return Json(new List<SelectListItem>(), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
        }


        public async Task<ActionResult> GetMasterLocationProcurement()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetMasterLocationProcurement";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetMasterDepreciation()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetMasterDepreciation";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLOrganization()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/organizationlist";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetAffiliation()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetAffiliation";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetFrom()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetFrom";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLChartofaccountReport()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/chartofaccountreportlist";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLAccouningCondition()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/accountingcondition";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLAccouningConditionDK()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/accountingconditionDK";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLCOACodeByNameLv1()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/COACodeByNameLv1";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLCOACodeByNameLv2(string CodeRec)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/COACodeByNameLv2/{CodeRec}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLCOACodeByNameLv3(string CodeRec)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/COACodeByNameLv3/{CodeRec}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetFixedAssetByLocation(string locationId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetFixedAssetByLocation/{locationId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLCOACodeByNameLv4(string CodeRec)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/COACodeByNameLv4/{CodeRec}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLCOACodeByNameLv5(string CodeRec)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/COACodeByNameLv5/{CodeRec}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLCountry()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/countrylist";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLProvince(string CountryId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/provincelist/{CountryId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLCity(string ProvinceId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/citylist/{ProvinceId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLClientList()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/clientlist";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLVendorList()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/vendorlist";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        public async Task<ActionResult> GetIsActive()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/isActive";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        public async Task<ActionResult> GetDDLAllocationRatio()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/AllocationRatio";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLStatusBudget()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/StatusBudget";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLMasterBudgetCode()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/MasterBudgetCode";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLBudgetCode(string itemTypeId, int year, string hrisDivisionId, string budgetOwnerId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/budgetcode/{itemTypeId}/{year}/{hrisDivisionId ?? "0"}/{budgetOwnerId ?? "0"}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLMasterBudgetCodeCapex()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/masterbudgetcodecapex";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLMasterBudgetCodeOpex()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/masterbudgetcodeopex";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLMasterMonthlyBudgetSpreads()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/MasterMonthlyBudgetSpreads";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLVersionBudgetCapex(int year)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/VersionBudgetCapex/{year}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLVersionBudgetOpex(int year)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/VersionBudgetOpex/{year}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetTaskCAS(string casId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetTaskCAS/{casId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetReportType()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetReportType";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetPCETypeCrossBilling()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetPCETypeCrossBilling";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public ActionResult GetMonth()
        {
            var result = DateTimeFormatInfo
                   .InvariantInfo
                   .MonthNames
                   .Select((monthName, index) => new SelectListItem
                   {
                       Value = (index + 1).ToString(),
                       Text = monthName
                   }).Where(x => !string.IsNullOrEmpty(x.Text));
            return Json(result, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLCOALevel()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/getcoalevel";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        #region PM
        public async Task<ActionResult> GetDDLJobNotExistsInPCAandJobId(string status, string jobId)
        {
            var getUrl = "";
            if (string.IsNullOrEmpty(status) && string.IsNullOrEmpty(jobId))
                getUrl = $"{ApiUrl.dropdownUrl}/JobId";
            else
                getUrl = $"{ApiUrl.dropdownUrl}/JobNotExistsInPCAandJobId/{status}/{jobId}";

            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLJobNotExistsInPCEandJobId(string status, string jobId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/JobNotExistsInPCEandJobId/{status}/{jobId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLMotherPca()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/MotherPca";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLRateCard()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/RateCard";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLTaskRateCard(string rateCardId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/TaskRateCard/{rateCardId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model.OrderBy(x => x.Value), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLSubTaskRateCard(string rateCardId, string taskRateCardName)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/SubTaskRateCard/{rateCardId}/{taskRateCardName}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model.OrderBy(x => x.Value), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLTypeOfExpense()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/TypeOfExpense";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLMainserviceCategory()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/MainserviceCategory";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLMainserviceCategoryNotOthers()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/MainserviceCategory";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            var other = model.Where(x => x.Value == MainServiceCategoryId.Others).FirstOrDefault();
            model.Remove(other);
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLMainserviceCategoryPCA()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/MainserviceCategoryPCA";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetAllDDLShareservices()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/Shareservices";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLShareservices(string MainserviceCategoryId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/Shareservices/{MainserviceCategoryId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLDepartment()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/Departments";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLOtherFee()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/OtherFee";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLPceReference(string pceId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/PCEReference/{pceId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLStatusPCE()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/StatusPCE";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLMasterOutlet(string clientId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/masteroutletlist/{clientId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLConditionProcurement()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetMasterConditionProcurement";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLCostSharingAllocation()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetCostSharingAllocation";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetEmployee()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/EmployeeBasicInfo";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetInventory()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetInventory";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetFixedAsset()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetFixedAsset";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetFixedAssetAP(string journalId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetFixedAssetAP/{journalId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetFixedAssetMovementStatus()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/FixedAssetMovementStatus";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetFixedAssetCombine()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetFixedAssetCombine";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetFixedAssetCombineAll()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetFixedAssetCombineAll";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetCOAID()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetCOAID";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }


        public async Task<ActionResult> GetDDLBankVendor()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/vendorbank";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetPettyCash()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetPettyCash";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetPettyCashByLegalEntity(string legalEntityId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetPettyCashByLegalEntity/{legalEntityId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetBankandPettyCash()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetBankandPettyCash";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> InventoryDetailstatus()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/InventoryDetailstatus";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> Job()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/Job";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> MoveLocation()
        {
            var getUrl = $"{ApiUrl.financedropdownUrl}/MoveLocation";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> LocationWarehouse()
        {
            var getUrl = $"{ApiUrl.financedropdownUrl}/LocationWarehouse";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> InvGetform(string Type)
        {
            var param = Type == "INVENTORY" ? "2" : "1";
            var getUrl = $"{ApiUrl.financedropdownUrl}/InvGetform/{param}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetVendorNames()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/get_vendornames";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetItemCategory()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/get_itemcategories";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetJournalAdjustmentType()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetJournalAdjustmentType";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetSalaryAllocationType()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetSalaryAllocationType";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetJournalAdjustmentNumber(JournalAdjustmentSearchNumber search)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetJournalAdjustmentNumber";
            var model = await client.PostApiResponse<List<SelectListItem>>(getUrl, search);
            var result = new List<SelectListItem>();
            if (model.Data != null) result = model.Data as List<SelectListItem>;
            return Json(result, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetSalaryAllocationNumber(SalaryAllocationSearchNumber search)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetSalaryAllocationNumber";
            var model = await client.PostApiResponse<List<SelectListItem>>(getUrl, search);
            var result = new List<SelectListItem>();
            if (model.Data != null) result = model.Data as List<SelectListItem>;
            return Json(result, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        #endregion
    }
}