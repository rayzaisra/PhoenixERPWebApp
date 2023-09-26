using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;

namespace PhoenixERPWebApp.Areas.Pm.Controllers
{
    [Area(nameof(Pm))]
    public class DropdownController : Controller
    {
        private readonly ApiClientFactory client;

        public DropdownController(ApiClientFactory client)
        {
            this.client = client;
        }

        public async Task<ActionResult> GetDDLJobNotExistsInPCAandJobId(string status, string jobId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/JobNotExistsInPCAandJobId/{status}/{jobId}";
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

        public async Task<ActionResult> GetDDLCurrency()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/Currency";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLCurrencyDetail(string headerCurrencyId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/Currency";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            model = model.Where(x => x.Value != headerCurrencyId).ToList();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLCurrencyParamsAndDefault(string currencyId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/CurrencyParamsAndDefault/{currencyId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLOperator()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/Operator";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
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

        public async Task<ActionResult> GetDDLMainserviceCategoryPCA()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/MainserviceCategoryPCA";
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
        public async Task<ActionResult> GetDDLUser()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/UserApp";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLUnit()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/unit";
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

        public async Task<ActionResult> GetDDLStatus()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetStatus";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLStatusPcaPce()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetStatusPcaPce";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLMasterOutlet(string clientId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/masteroutletlist/{clientId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLBusinessUnitFinance()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/DivisionFinance";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLNonJobByBusinessUnitId()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetDDLNonJobByBusinessUnitId";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLJobList()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetDDLJobList";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLFolderNameMyDam()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetDDLFolderNameMyDam";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLJobNameMyDam()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetDDLJobNameMyDam";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLClientNameMyDam()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetDDLClientNameMyDam";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLCampaignNameMyDam()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetDDLCampaignNameMyDam";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }



    }
}

