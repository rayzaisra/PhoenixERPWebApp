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
    public class MasterJobClosingController : Controller
    {
        string url = ApiUrl.MapCoaTransaksiUrl;
        private readonly ApiClientFactory client;

        public MasterJobClosingController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddEdit(string mainServiceCategoryId, string businessUnitId)
        {
            var model = new VwMasterJobClosing();
            ViewBag.Header = "Create";
            model.isEdit = false;

            if (!string.IsNullOrEmpty(mainServiceCategoryId) && !string.IsNullOrEmpty(businessUnitId))
            {
                ViewBag.Header = "Update";
                model = new VwMasterJobClosing() { MainserviceCategoryId = mainServiceCategoryId, BusinessUnitId = businessUnitId };
                model.isEdit = true;
            }

            model.MainserviceCategoryId = (model.MainserviceCategoryId ?? MainServiceCategoryId.Production);
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<VwMasterJobClosing>>($"{url}/GetMasterClosing") ?? new List<VwMasterJobClosing>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadAP([DataSourceRequest] DataSourceRequest request, string mainServiceCategoryId, string businessUnitId, bool isEdit)
        {
            var model = await client.Get<List<MapCoaTransaksi>>($"{url}/GetMasterClosingAP/{mainServiceCategoryId ?? "0"}/{businessUnitId ?? "0"}/{isEdit}") ?? new List<MapCoaTransaksi>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadAR([DataSourceRequest] DataSourceRequest request, string mainServiceCategoryId, string businessUnitId, bool isEdit)
        {
            var model = await client.Get<List<MapCoaTransaksi>>($"{url}/GetMasterClosingAR/{mainServiceCategoryId ?? "0"}/{businessUnitId ?? "0"}/{isEdit}") ?? new List<MapCoaTransaksi>();
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
        public async Task<IActionResult> Submit(VwMasterJobClosing model)
        {
            if (!(model.isEdit ?? false))
            {
                var get = await client.Get<VwMasterJobClosing>($"{url}/GetMasterClosingById/{model.MainserviceCategoryId}/{model.BusinessUnitId}");
                if (get != null)
                {
                    throw new Exception($"business unit '{model.BusinessUnitName}' already exists");
                }
            }

            if (model.AP.Where(x => string.IsNullOrEmpty(x.CoaId)).Count() > 0 && model.AP.Where(x => string.IsNullOrEmpty(x.Condition)).Count() > 0)
            {
                throw new Exception("Account Name (Level 5 COA) and Condition cannot be empty");
            }
            else
            {

                if (model.AP.Where(x => string.IsNullOrEmpty(x.CoaId)).Count() > 0)
                {
                    throw new Exception("Account Name (Level 5 COA) cannot be empty");
                }

                if (model.AP.Where(x => string.IsNullOrEmpty(x.Condition)).Count() > 0)
                {
                    throw new Exception("Condition cannot be empty");
                }
            }

            var result = new List<MapCoaTransaksi>();
            if (model.AP != null)
            {
                Parallel.ForEach(model.AP, item =>
                 {
                     var ap = new MapCoaTransaksi()
                     {
                         Id = item.Id,
                         ModuleCode = item.ModuleCode,
                         MainserviceCategoryId = model.MainserviceCategoryId,
                         BusinessUnitId = model.BusinessUnitId,
                         LineCode = item.LineCode,
                         Description = item.Description,
                         CoaId = item.CoaId,
                         DK = item.DK,
                         AccountName = item.AccountName,
                         CodeRec = item.CodeRec,
                         CreatedBy = item.CreatedBy,
                         CreatedOn = item.CreatedOn,
                         isEdit = model.isEdit
                     };
                     result.Add(ap);
                 });
            }

            if (model.AR != null)
            {
                Parallel.ForEach(model.AR, item =>
                {
                    var ap = new MapCoaTransaksi()
                    {
                        Id = item.Id,
                        ModuleCode = item.ModuleCode,
                        MainserviceCategoryId = model.MainserviceCategoryId,
                        BusinessUnitId = model.BusinessUnitId,
                        LineCode = item.LineCode,
                        Description = item.Description,
                        CoaId = item.CoaId,
                        DK = item.DK,
                        AccountName = item.AccountName,
                        CodeRec = item.CodeRec,
                        CreatedBy = item.CreatedBy,
                        CreatedOn = item.CreatedOn,
                        isEdit = model.isEdit
                    };
                    result.Add(ap);
                });

            }
            var response = await client.PostApiResponse<MapCoaTransaksi>($"{url}/AddMasterClosing", result);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(MapCoaTransaksi model) => Json(await client.DeleteApiResponse<MapCoaTransaksi>($"{url}/DeleteMasterClosing/{model.MainserviceCategoryId}/{model.BusinessUnitId}"));
    }
}