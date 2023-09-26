using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CodeMarvel.Infrastructure.ModelShared;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.Extentions;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;

namespace PhoenixERPWebApp.Areas.HrisNew.Controllers
{
    [Area(nameof(HrisNew))]
    public class EmployeeBasicController : Controller
    {
        readonly string url = ApiUrl.EmployeeBasic;
        readonly string urlFamily = ApiUrl.Family;
        readonly string urlEmployeeEmergencyContract = ApiUrl.EmployeeEmergencyContact;
        readonly string urlEducation = ApiUrl.Education;
        readonly string urlEmployeeExperience = ApiUrl.EmployeeExperience;
        readonly string urlEmployeeTraining = ApiUrl.EmployeeTraining;
        readonly string urlEmployeeChecklist = ApiUrl.EmployeeChecklist;
        private readonly ApiClientFactory client;

        static string paramValueTemp = string.Empty;
        static string paramColumnTemp = string.Empty;

        public EmployeeBasicController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index(string param)
        {
            if (param != "edit")
            {
                paramValueTemp = string.Empty;
                paramColumnTemp = string.Empty;
            }
            return View();
        }

        public async Task<IActionResult> AddEdit(string Id, string paramcol = "", string paramval = "", string isReadOnly = "")
        {
            var model = new EmployeeBasicInfo();
            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<EmployeeBasicInfo>($"{url}/{Id}");

                if (!string.IsNullOrEmpty(paramcol))
                {
                    model.ParamColumn = paramcol;
                    model.ParamValue = paramval;
                    paramValueTemp = paramval;
                    paramColumnTemp = paramcol;
                }
                else
                {
                    model.ParamColumn = string.Empty;
                    model.ParamValue = string.Empty;
                    paramValueTemp = string.Empty;
                    paramColumnTemp = string.Empty;
                }
                model.IsReadOnly = isReadOnly;

                //ViewBag.isRequestor = !(model.IsHcProcess ?? false);//(model.Status == StatusTransaction.Draft || model.Status == StatusTransaction.Rejected);
            }
            else
            {
                ViewBag.Header = "Create";

                //var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                //model.requestEmployee = requestor;
                //model.Requestor = requestor.EmployeeId;
                //model.RequestDate = DateTime.Now;
                //ViewBag.isRequestor = true;
                //model.infoEmployee = new InfoEmployee();
                //model.TerminationType = TerminationType.EndOfContract;
            }

            //model.Reason = HttpUtility.HtmlDecode(model.Reason);
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {

			var model = await client.Get<List<EmployeeBasicInfo>>(url) ?? new List<EmployeeBasicInfo>();
            var data = model.ToDataSourceResult(request);

            var result = new { data = data, ParamVal = paramValueTemp, ParamColumn = paramColumnTemp };
            return Json(result, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });

        }

        //[HttpPost]
        //public async Task<IActionResult> submitToHC(Termination model)
        //{
        //    model.Status = StatusTransaction.Draft; //tidak di pakai sehingga harus di set draft untuk tidak terkena validasi approval
        //    model.IsHcProcess = true;
        //    return await Process(model, false);
        //}

        [HttpPost]
        public async Task<IActionResult> Save(EmployeeBasicInfo model, IFormFile filePhoto)
        {
            return await Process(model, filePhoto);
        }

        private async Task<IActionResult> Process(EmployeeBasicInfo model, IFormFile filePhoto)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();

            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<EmployeeBasicInfo>(url, model, filePhoto);
                if (!string.IsNullOrEmpty(paramColumnTemp))
                    return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index), "EmployeeBasic" , new { param = "edit" })});
                else
                    return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index), "EmployeeBasic") });

            }
            else
            {
                response = await client.PostApiResponse<EmployeeBasicInfo>(url, model, filePhoto);
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index), "EmployeeBasic") });
            }

			//if (isDraft)
			//{
			//return Json(new { success = response.Success, message = response.Message, data = response.Data as EmployeeBasicInfo });
			//}
			//else
			//{
			return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            //}
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Termination model) => Json(await client.DeleteApiResponse<Termination>($"{url}/{model.Id}"));

        #region family
        public async Task<IActionResult> ReadFamily([DataSourceRequest] DataSourceRequest request, string employeeId)
        {
            var model = await client.Get<List<Family>>($"{urlFamily}/GetByEmployeeBasicInfoId/{employeeId}") ?? new List<Family>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult CreateFamily([DataSourceRequest] DataSourceRequest request, Family model)
        {
            model.Id = Guid.NewGuid().ToString();
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult UpdateFamily([DataSourceRequest] DataSourceRequest request, Family model)
        {
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult DestroyFamily([DataSourceRequest] DataSourceRequest request, Family model)
        {
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        #endregion family

        #region employee_emergency_contact
        public async Task<IActionResult> ReadEmployeeEmergencyContact([DataSourceRequest] DataSourceRequest request, string employeeId)
        {
            var model = await client.Get<List<EmployeeEmergencyContact>>($"{urlEmployeeEmergencyContract}/GetByEmployeeBasicInfoId/{employeeId}") ?? new List<EmployeeEmergencyContact>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult CreateEmployeeEmergencyContact([DataSourceRequest] DataSourceRequest request, EmployeeEmergencyContact model)
        {
            model.Id = Guid.NewGuid().ToString();
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult UpdateEmployeeEmergencyContact([DataSourceRequest] DataSourceRequest request, EmployeeEmergencyContact model)
        {
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult DestroyEmployeeEmergencyContact([DataSourceRequest] DataSourceRequest request, EmployeeEmergencyContact model)
        {
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        #endregion employee_emergency_contract

        #region employee_emergency_contact
        public async Task<IActionResult> ReadEducation([DataSourceRequest] DataSourceRequest request, string employeeId)
        {
            var model = await client.Get<List<Education>>($"{urlEducation}/GetByEmployeeBasicInfoId/{employeeId}") ?? new List<Education>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult CreateEducation([DataSourceRequest] DataSourceRequest request, Education model)
        {
            model.Id = Guid.NewGuid().ToString();
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult UpdateEducation([DataSourceRequest] DataSourceRequest request, Education model)
        {
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult DestroyEducation([DataSourceRequest] DataSourceRequest request, Education model)
        {
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        #endregion employee_emergency_contract

        #region employeeexperience
        public async Task<IActionResult> ReadEmployeeExperience([DataSourceRequest] DataSourceRequest request, string employeeId)
        {
            var model = await client.Get<List<EmployeeExperience>>($"{urlEmployeeExperience}/GetByEmployeeBasicInfoId/{employeeId}") ?? new List<EmployeeExperience>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult CreateEmployeeExperience([DataSourceRequest] DataSourceRequest request, EmployeeExperience model)
        {
            model.Id = Guid.NewGuid().ToString();
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult UpdateEmployeeExperience([DataSourceRequest] DataSourceRequest request, EmployeeExperience model)
        {
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult DestroyEmployeeExperience([DataSourceRequest] DataSourceRequest request, EmployeeExperience model)
        {
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        #endregion employeeexperience

        #region employeeetraining
        public async Task<IActionResult> ReadEmployeeTraining([DataSourceRequest] DataSourceRequest request, string employeeId)
        {
            var model = await client.Get<List<EmployeeTraining>>($"{urlEmployeeTraining}/GetByEmployeeBasicInfoId/{employeeId}") ?? new List<EmployeeTraining>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult CreateEmployeeTraining([DataSourceRequest] DataSourceRequest request, EmployeeTraining model)
        {
            model.Id = Guid.NewGuid().ToString();
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult UpdateEmployeeTraining([DataSourceRequest] DataSourceRequest request, EmployeeTraining model)
        {
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult DestroyEmployeeTraining([DataSourceRequest] DataSourceRequest request, EmployeeTraining model)
        {
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        #endregion employeeetraining

        #region EmployyeChecklist
        public async Task<IActionResult> ReadEmployeeChecklist([DataSourceRequest] DataSourceRequest request, string employeeId)
        {
            var model = await client.Get<List<EmployeeChecklist>>($"{urlEmployeeChecklist}/GetByEmployeeBasicInfoId/{employeeId}") ?? new List<EmployeeChecklist>();

            if (model != null)
            {
                foreach (var item in model)
                {
                    item.EchkEmployeeChecklistCategoryArrayId = item.EchkEmployeeChecklistCategoryId.Split(',');
                }
            }
            var data = model.ToDataSourceResult(request);

            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult CreateEmployeeChecklist([DataSourceRequest] DataSourceRequest request, EmployeeChecklist model)
        {
            model.Id = Guid.NewGuid().ToString();
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult UpdateEmployeeChecklist([DataSourceRequest] DataSourceRequest request, EmployeeChecklist model)
        {
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult DestroyEmployeeChecklist([DataSourceRequest] DataSourceRequest request, EmployeeChecklist model)
        {
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        #endregion EmployyeChecklist

        #region EmployeeHistory
        public async Task<IActionResult> ReadEmployeeHistory([DataSourceRequest] DataSourceRequest request, string employeeId)
        {
            var model = await client.Get<List<PhoenixERPWebApp.Models.HrisNew.Transaction.Recruitment.EmployeeHistory>>($"{urlEmployeeChecklist}/GetByEmployeeHistory/{employeeId}") ?? new List<PhoenixERPWebApp.Models.HrisNew.Transaction.Recruitment.EmployeeHistory>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        #endregion

        #region ContractSign
        public async Task<IActionResult> ReadContractSign([DataSourceRequest] DataSourceRequest request, string employeeId)
        {
            var model = await client.Get<List<EmployeeContractSign>>($"{urlEmployeeChecklist}/GetByContractSign/{employeeId}") ?? new List<EmployeeContractSign>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        #endregion

        public async Task<ActionResult> GetDDLJobLevelJobTitle()
        {
            var getUrl = $"{ApiUrl.BusinessUnitJobLevel}/GetDdl";
            var model = await client.Get<List<BusinessUnitJobLevel>>(getUrl) ?? new List<BusinessUnitJobLevel>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLJobLevelJobTitleVacant()
        {
            var getUrl = $"{ApiUrl.BusinessUnitJobLevel}/GetDdlJobTitleVacant";
            var model = await client.Get<List<BusinessUnitJobLevel>>(getUrl) ?? new List<BusinessUnitJobLevel>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLJobLevelJobTitleNonVacant()
        {
            var getUrl = $"{ApiUrl.BusinessUnitJobLevel}/GetDdlJobTitleNonVacant";
            var model = await client.Get<List<BusinessUnitJobLevel>>(getUrl) ?? new List<BusinessUnitJobLevel>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetJobLevelJobTitleDetail(string id)
        {
            var getUrl = $"{ApiUrl.BusinessUnitJobLevel}/GetDetail/{id}";
            var model = await client.Get<BusinessUnitJobLevel>(getUrl) ?? new BusinessUnitJobLevel();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}