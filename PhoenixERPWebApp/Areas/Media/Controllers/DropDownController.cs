using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;

namespace PhoenixERPWebApp.Areas.Media.Controllers
{
    [Area(nameof(Media))]
    public class DropDownController : Controller
    {
        private readonly ApiClientFactory client;

        public DropDownController(ApiClientFactory client)
        {
            this.client = client;
        }
        
        public async Task<ActionResult> GetDDLMediaType()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/MediaType";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLMediaPlanRequestType()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/MediaPlanRequestType";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLClient()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/client";
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

        public async Task<ActionResult> GetDDLMediaPlanNo()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/mediaplanno";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLMediaOrderStatus()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/mediaorderstatus";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLMediaPlanStatus()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/mediaplanstatus";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLProgramCategory()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/programcategory";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLProgramPosition()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/programposition";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLProgramType()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/programtype";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        public async Task<ActionResult> GetCascadeFromJob(string jobid, string caseData)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/cascadejob/{jobid}/{caseData}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });

        }
        public async Task<ActionResult> GetBrandASFValue()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/brandasf";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });

        }
    }
}