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

namespace PhoenixERPWebApp.Areas.HrisNew.Controllers
{
    [Area(nameof(HrisNew))]
    public class DropdownController : Controller
    {
        private readonly ApiClientFactory client;

        public DropdownController(ApiClientFactory client)
        {
            this.client = client;
        }

        public async Task<ActionResult> GetDDLMenu()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/Menu";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLMasterDivisionSelfservice()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/MasterDivisionSelfservice";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLEmployeeBasicInfo()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/EmployeeBasicInfo";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLEmployeeBasicInfoMasterCounterPart(string id)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/EmployeeBasicInfoMasterCounterPart/{id}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLEmployeeBasicInfoByName(string name)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/EmployeeBasicInfoByName/{name}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLPayrollComponent(string DeclarationTypeId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetDDLPayrollComponent/{DeclarationTypeId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLTerminationEmployeeBasicInfoByName(string name, string terminationType)
        {
            var model = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(name))
            {
                var getUrl = $"{ApiUrl.dropdownUrl}/TerminationEmployeeBasicInfoByName/{name}/{terminationType}";
                model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            }
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLEmployeeBasicInfoByHead(string name)
        {
            var model = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(name))
            {
                var getUrl = $"{ApiUrl.dropdownUrl}/GetEmployeeBasicInfoByHead/{name}";
                model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            }
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetEmployeeResignationType()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/EmployeeResignationType";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLTerminationType()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/EmployeeTerminationType";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetEmployeeDeathType()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/EmployeeDeathType";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        public async Task<ActionResult> GetDDLFaultCategory()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/faultcategorylist";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLServiceName()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/servicenamelist";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLCategoryExitInterview()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/categoryexitinterviewlist";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public ActionResult GetDDLType()
        {
            var model = new List<SelectListItem>() {
                            new SelectListItem() { Text = InputType.InputTypeName(InputType.Checkbox), Value = InputType.Checkbox.ToString() },
                            new SelectListItem() { Text = InputType.InputTypeName(InputType.Radiobutton), Value = InputType.Radiobutton.ToString() },
                            new SelectListItem() { Text = InputType.InputTypeName(InputType.TextArea), Value = InputType.TextArea.ToString() } };
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLLeaveRequestType()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/leaverequesttypelist";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLLeaveRequestTypeByEmployeeId(string employeeid)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/leaverequesttypelistemployeeid/{employeeid}";
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

        public async Task<ActionResult> GetDDLLoanCategory()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/loancategorylist";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLLoanCategoryCorporate()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/LoanCategoryCorporateList";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public ActionResult GetDDLExludeLeaveType()
        {
            var model = new List<SelectListItem>() {
                            new SelectListItem() { Text = ExludeLeaveType.ExludeLeaveTypeName(ExludeLeaveType.Day), Value = ExludeLeaveType.Day.ToString() },
                            new SelectListItem() { Text = ExludeLeaveType.ExludeLeaveTypeName(ExludeLeaveType.Month), Value = ExludeLeaveType.Month.ToString() }
                        };
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLYear()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/YearList";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLMonth()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/MonthList";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public ActionResult GetDDLResponsibility()
        {
            var model = new List<SelectListItem>() {
                            new SelectListItem() { Text = ResponsibilityType.ResponsibilityTypeName(ResponsibilityType.Employee), Value = ResponsibilityType.Employee.ToString() },
                            new SelectListItem() { Text = ResponsibilityType.ResponsibilityTypeName(ResponsibilityType.Corporate), Value = ResponsibilityType.Corporate.ToString() } };
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLJobGrades()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/JobGrades";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }


        public async Task<ActionResult> GetDDLJobGradesWithLevel(string id)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/JobGradesWithLevel/{id}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLJobTitles()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/JobTitles";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLJobLevels()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/JobLevels";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLSubGroups()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/SubGroups";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDivision()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/Division";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLDivisions(string subGroupId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/Divisions/{subGroupId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLDepartment(string divisionId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/Departments/{divisionId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLDepartments()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/Departments";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLDepartmentById(string id)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/DepartmentById/{id}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLLocations()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/Locations";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLSalaryComponent()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/salarycomponentlist";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLAllowanceComponent(string gradeId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/allowancecomponentlist/{gradeId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLDeductionComponent()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/deductioncomponentlist";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLBpjsComponentAdditional()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/bpjscomponentadditionallist";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLBpjsComponentDeduction()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/bpjscomponentdeductionlist";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLFamily(string employeeId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/Family/{employeeId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLInsuranceCompany()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/insurancecompanylist";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public ActionResult GetDDLTypeBPJS()
        {
            var model = new List<SelectListItem>() {
                            new SelectListItem() { Text = BPJSType.BPJSTypeName(BPJSType.Additional), Value = BPJSType.Additional.ToString() },
                            new SelectListItem() { Text = BPJSType.BPJSTypeName(BPJSType.Deduction), Value = BPJSType.Deduction.ToString() }
            };
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public ActionResult GetDDLSalaryComponentAdditional()
        {
            var model = new List<SelectListItem>() {
                            new SelectListItem() { Text = SalaryComponentTypeAdditional.SalaryComponentTypeAdditionalName(SalaryComponentTypeAdditional.SalaryComponent), Value = SalaryComponentTypeAdditional.SalaryComponent.ToString() },
                            new SelectListItem() { Text = SalaryComponentTypeAdditional.SalaryComponentTypeAdditionalName(SalaryComponentTypeAdditional.Allowance), Value = SalaryComponentTypeAdditional.Allowance.ToString() },
                             new SelectListItem() { Text = SalaryComponentTypeAdditional.SalaryComponentTypeAdditionalName(SalaryComponentTypeAdditional.BPJS), Value = SalaryComponentTypeAdditional.BPJS.ToString() }
            };
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public ActionResult GetDDLSalaryComponentDeduction()
        {
            var model = new List<SelectListItem>() {
                            new SelectListItem() { Text = SalaryComponentTypeDeduction.SalaryComponentTypeDeductionName(SalaryComponentTypeDeduction.BPJS), Value = SalaryComponentTypeDeduction.BPJS.ToString() },
                            new SelectListItem() { Text = SalaryComponentTypeDeduction.SalaryComponentTypeDeductionName(SalaryComponentTypeDeduction.Deduction), Value = SalaryComponentTypeDeduction.Deduction.ToString() }
            };
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLComponentTypeAdditional(int salaryComponentId, string gradeId)
        {
            if (salaryComponentId == SalaryComponentTypeAdditional.SalaryComponent)
            {
                return await GetDDLSalaryComponent();
            }
            else if (salaryComponentId == SalaryComponentTypeAdditional.Allowance)
            {

                return await GetDDLAllowanceComponent(gradeId);
            }
            else if (salaryComponentId == SalaryComponentTypeAdditional.BPJS)
            {
                return await GetDDLBpjsComponentAdditional();
            }
            return Json(new List<SelectListItem>(), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLComponentTypeDeduction(int salaryComponentId)
        {
            if (salaryComponentId == SalaryComponentTypeDeduction.BPJS)
            {
                return await GetDDLBpjsComponentDeduction();
            }
            else if (salaryComponentId == SalaryComponentTypeDeduction.Deduction)
            {
                return await GetDDLDeductionComponent();

            }
            return Json(new List<SelectListItem>(), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public ActionResult GetDDLSalaryZone()
        {
            var model = new List<SelectListItem>();
            for (int i = 1; i <= 9; i++)
            {
                model.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLPdrCategory()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/pdrcategorylist";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLPdrGrade()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/pdrgradelist";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLLegalCategory()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/legalcategorylist";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLEmployeeLegalDoc(string category, string legalCategoryId, string text)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/employeeLegalDoc/{category}/{legalCategoryId}/{text}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public ActionResult GetDDLCategory()
        {
            var model = new List<SelectListItem>() {
                            new SelectListItem() { Text = CategoryLegalDoc.Employee, Value = CategoryLegalDoc.Employee },
                            new SelectListItem() { Text = CategoryLegalDoc.Client, Value = CategoryLegalDoc.Client },
                            new SelectListItem() { Text = CategoryLegalDoc.Vendor, Value = CategoryLegalDoc.Vendor } };
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLLegalDocument(string category, string CounterpartId)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/LegalDoc/{category}/{CounterpartId}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLBscCategory()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/bsccategorylist";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLCorporateObjective()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/corporateobjectivelist";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLBscCorporateObjectiveName(string bscCategory)
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/corporateobjectivename/{bscCategory}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLPDRObjectiveSetting()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/objectiveSettingList";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public ActionResult GetDDLPDRWeight()
        {
            var model = new List<SelectListItem>();
            for (int i = 5; i <= 100; i++)
            {
                model.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLEmployeePayrollGroup()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/employeePayrollGroupList";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLPTKPSetting()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/PtkpSettingList";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetLegalEntity()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/LegalEntity";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLReligion()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/religionlist";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        public async Task<ActionResult> GetDDLMarriedStatus()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/getddlmarriedstatus";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLeducationlevel()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/getddleducationlevel";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLFamilyStatus()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/getddlfamilystatus";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLInstitution()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/getddlinstitution";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLEmployementStatus()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/getddlemployementstatus";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLNationality()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/getddlnationality";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLEmployeeData()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/getddlemployeedata";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetDDLInstitutionTypes()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/getInstitutionTypes";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

		public async Task<ActionResult> GetDDLEmployeeChecklistCategory()
		{
			var getUrl = $"{ApiUrl.dropdownUrl}/GetDDLEmployeeChecklistCategory";
			var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
			return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
		}
		public async Task<ActionResult> GetWorkingHour()
		{
			var getUrl = $"{ApiUrl.dropdownUrl}/GetWorkingHour";
			var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
			return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
		}

        public async Task<ActionResult> GetMasterTrainingRequest()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetMasterTrainingRequest";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetTrainingCompleted()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/GetTrainingCompleted";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }


        #region"hris old"
        public async Task<ActionResult> GetDDLGroups()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/Groups";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        public async Task<ActionResult> GETDDLTraining()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/TrainingSetting";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        public async Task<ActionResult> GETDDLTrainingCategory()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/TrainingCategory";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        public async Task<ActionResult> GETDDLTrainingSubject()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/TrainingSubject";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        public async Task<ActionResult> GETDDLTrainingType()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/TrainingType";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        #endregion
    }
}