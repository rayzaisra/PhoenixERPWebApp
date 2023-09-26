using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.ModelShared;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area(nameof(Finance))]
    public class MasterAccountReceivableController : Controller
    {
        string url = ApiUrl.MapCoaTransaksiUrl;
        private readonly ApiClientFactory client;

        public MasterAccountReceivableController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddEdit(string mainServiceCategoryId, bool? pceTypeCrossBilling, string invoiceType)
        {
            var model = await client.Get<VwMasterJournalApAr>($"{url}/GetMasterARById/{(mainServiceCategoryId ?? "0")}/{(pceTypeCrossBilling ?? false)}/{invoiceType ?? "0"}");
            var isEdit = (model != null);
            if (isEdit)
            {
                ViewBag.Header = "Update";
            }
            else
            {
                ViewBag.Header = "Create";
                model = new VwMasterJournalApAr();
            }

            model.isEdit = isEdit;
            model.PceTypeCrossBilling = (model.PceTypeCrossBilling ?? false);
            model.InvoiceType = (model.InvoiceType ?? "Single");
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<VwMasterJournalApAr>>($"{url}/GetMasterAR") ?? new List<VwMasterJournalApAr>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadDetail([DataSourceRequest] DataSourceRequest request, string mainServiceCategoryId, bool? pceTypeCrossBilling, string invoiceType, bool isEdit)
        {
            var model = await client.Get<List<MapCoaTransaksi>>($"{url}/GetMasterDetailAR/{mainServiceCategoryId ?? "0"}/{(pceTypeCrossBilling ?? false)}/{invoiceType}/{isEdit}") ?? new List<MapCoaTransaksi>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public IActionResult UpdateDetail([DataSourceRequest] DataSourceRequest request, MapCoaTransaksi model)
        {
            if (model != null && ModelState.IsValid)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(VwMasterJournalApAr model)
        {

            if (!(model.isEdit ?? false))
            {
                var get = await client.Get<VwMasterJournalApAr>($"{url}/GetMasterARById/{model.MainserviceCategoryId}/{(model.PceTypeCrossBilling ?? false)}/{model.InvoiceType}");
                if (get != null)
                {
                    if (model.MainserviceCategoryId == MainServiceCategoryId.Production)
                    {
                        throw new Exception($"main service category '{model.MainserviceCategoryName}', invoice type '{model.InvoiceType}' and pce type cross billing '{((model.PceTypeCrossBilling ?? false) == false ? "No" : "Yes")}' already exists");
                    }
                    else
                    {
                        throw new Exception($"main service category '{model.MainserviceCategoryName}' already exists");
                    }
                }
            }

            if (model.Detail.Where(x => string.IsNullOrEmpty(x.CoaId)).Count() > 0 && model.Detail.Where(x => string.IsNullOrEmpty(x.Condition)).Count() > 0)
            {
                throw new Exception("Account Name (Level 5 COA) and Condition cannot be empty");
            }
            else
            {

                if (model.Detail.Where(x => string.IsNullOrEmpty(x.CoaId)).Count() > 0)
                {
                    throw new Exception("Account Name (Level 5 COA) cannot be empty");
                }

                if (model.Detail.Where(x => string.IsNullOrEmpty(x.Condition)).Count() > 0)
                {
                    throw new Exception("Condition cannot be empty");
                }
            }

            var result = new List<MapCoaTransaksi>();
            if (model.Detail != null)
            {
                Parallel.ForEach(model.Detail, item =>
                 {
                     var detail = new MapCoaTransaksi()
                     {
                         Id = item.Id,
                         ModuleCode = item.ModuleCode,
                         MainserviceCategoryId = model.MainserviceCategoryId,
                         LineCode = item.LineCode,
                         Description = item.Description,
                         CoaId = item.CoaId,
                         DK = item.DK,
                         IsAsf = (item.IsAsf ?? false),
                         NameSetting = model.NameSetting,
                         PceTypeCrossBilling = (model.PceTypeCrossBilling ?? false),
                         InvoiceType = model.InvoiceType,
                         //AccountName = item.AccountName,
                         //CodeRec = item.CodeRec,
                         CreatedBy = item.CreatedBy,
                         CreatedOn = item.CreatedOn,
                         isEdit = model.isEdit
                     };
                     result.Add(detail);
                 });
            }
            var response = await client.PostApiResponse<MapCoaTransaksi>($"{url}/AddMasterAR", result);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(MapCoaTransaksi model) => Json(await client.DeleteApiResponse<MapCoaTransaksi>($"{url}/DeleteMasterAR/{model.MainserviceCategoryId}/{(model.PceTypeCrossBilling ?? false)}/{model.InvoiceType}"));
    }
}