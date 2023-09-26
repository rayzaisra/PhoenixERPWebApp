using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Phoenix.Data;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Enum;
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services;
using Phoenix.Data.Services.Um;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Shared.Core.Areas.Hris.Controllers
{
    /// <summary>
    /// dropdown API Endpoint.
    /// </summary>RefCoAInvestmentCostListByProjectId
    [AllowAnonymousRoleAttribute]
    [Produces("application/json")]
    [Route("api/dropdown")]
    public class DropdownController : Controller
    {
        readonly IDropdownService service;
        private readonly IManageUserAppService userApp;

        /// <summary>
        /// And endpoint to manage MasterDivisionSelfservice
        /// </summary>
        /// <param name="context">Database context</param>
        public DropdownController(IDropdownService service, IManageUserAppService userApp)
        {
            this.service = service;
            this.userApp = userApp;
        }

        #region Global
        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("JobGrades")]
        [AllowAnonymousRole]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetJobGrades()
        {
            var result = await service.GetJobGrades();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("Menu")]
        [AllowAnonymousRole]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetMenu()
        {
            var result = await service.GetMenu();
            return Ok(new ApiResponse(result));
        }


        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("JobTitles")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetJobTitles()
        {
            var result = await service.GetJobTitles();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("JobLevels")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetJobLevels()
        {
            var result = await service.GetJobLevels();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("SubGroups")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetSubGroups()
        {
            var result = await service.GetSubGroups();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("Divisions/{subGroupId}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDivisions(string subGroupId)
        {
            var result = await service.GetDivisions(subGroupId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("Departments/{divisionId}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDepartments(string divisionId)
        {
            var result = await service.GetDepartments(divisionId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("DepartmentById/{id}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDepartmentById(string id)
        {
            var result = await service.GetDepartmentById(id);
            return Ok(new ApiResponse(result));
        }
        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("Locations")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetLocations()
        {
            var result = await service.GetLocations();
            return Ok(new ApiResponse(result));
        }


        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("Family/{employeeId}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetFamily(string employeeId)
        {
            var result = await service.GetFamily(employeeId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("LegalEntity")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetLegalEntity()
        {
            var result = await service.GetLegalEntity();
            return Ok(new ApiResponse(result));
        }
        [HttpGet("PaymentType")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetPaymentType()
        {
            var result = await service.GetPaymentType();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetItemCode/{ItemTypeId}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetItemCode(string ItemTypeId)
        {
            var result = await service.GetItemCode(ItemTypeId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetItemInventoryAndFixedAsset/{ItemTypeId}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetItem(string ItemTypeId)
        {
            var result = await service.GetItemInventoryAndFixedAsset(ItemTypeId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("Groups")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetGroups()
        {
            var result = await service.GetGroups();
            return Ok(new ApiResponse(result));
        }
        #endregion

        #region HRIS
        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("MasterDivisionSelfservice")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetMasterDivisionSelfservice()
        {
            var result = await service.GetMasterDivisionSelfservice();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("Division")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDivision()
        {
            var result = await service.GetDivision();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("DivisionHris")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDivisionHris()
        {
            var result = await service.GetDivisionHris();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("DivisionFinance")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDivisionFinance()
        {
            var result = await service.GetDivisionFinance();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("ApNumber")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetApNumber()
        {
            var result = await service.GetApNumber();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("ArNumber/{bankReceivedId}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetArNumber(string bankReceivedId)
        {
            var result = await service.GetArNumber(bankReceivedId);
            return Ok(new ApiResponse(result));
        }


        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("EmployeeBasicInfo")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetEmployeeBasicInfo()
        {
            var result = await service.GetEmployeeBasicInfo();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("EmployeeBasicInfoMasterCounterPart/{id?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetEmployeeBasicInfoMasterCounterPart(string id)
        {
            var result = await service.GetEmployeeBasicInfoMasterCounterPart(id);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetDDLNonJobByBusinessUnitId/")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLNonJobByBusinessUnitId()
        {
            var result = await service.GetDDLNonJobByBusinessUnitId();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetDDLPayrollComponent/{DeclarationTypeId?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLPayrollComponent(string DeclarationTypeId)
        {
            var result = await service.GetDDLPayrollComponent(DeclarationTypeId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("JobGradesWithLevel/{id?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> JobGradesWithLevel(string id)
        {
            var result = await service.GetDDLJobGradesWithLevel(id);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("EmployeeBasicInfoByName/{name?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetEmployeeBasicInfoByName(string name = "")
        {
            var result = await service.GetEmployeeBasicInfoByName(name);
            return Ok(new ApiResponse(result));
        }


        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <param name="name">Name parameter.</param>
        /// <param name="terminationType">Termination Type parameter.</param>
        /// <returns>List of json object.</returns>
        [HttpGet("TerminationEmployeeBasicInfoByName/{name?}/{terminationType?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetTerminationEmployeeBasicInfoByName(string name = "", string terminationType = "")
        {
            var result = await service.GetTerminationEmployeeBasicInfoByName(name, terminationType);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <param name="name">Name parameter.</param>
        /// <param name="terminationType">Termination Type parameter.</param>
        /// <returns>List of json object.</returns>
        [HttpGet("GetEmployeeBasicInfoByHead/{name?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetEmployeeBasicInfoByHead(string name = "")
        {
            var result = await service.GetEmployeeBasicInfoByHead(name);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("EmployeeResignationType")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetEmployeeResignationType()
        {
            var result = await service.GetEmployeeResignationType();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("EmployeeTerminationType")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetEmployeeTerminationType()
        {
            var result = await service.GetEmployeeTerminationType();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("EmployeeDeathType")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetEmployeeDeathType()
        {
            var result = await service.GetEmployeeDeathType();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("faultcategorylist")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> FaultCategoryList()
        {
            var result = await service.FaultCategoryList();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("servicenamelist")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> ServiceNameList()
        {
            var result = await service.ServiceNameList();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("categoryexitinterviewlist")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> CategoryExitInterviewList()
        {
            var result = await service.CategoryExitInterviewList();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("leaverequesttypelist")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> LeaveRequestTypeList()
        {
            var result = await service.LeaveRequestTypeList();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("leaverequesttypelistemployeeid/{employeeid?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> leaverequesttypelistemployeeid(string employeeid)
        {
            var result = await service.leaverequesttypelistemployeeid(employeeid);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("loancategorylist")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> LoanCategoryList()
        {
            var result = await service.LoanCategoryList();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("LoanCategoryCorporateList")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> LoanCategoryCorporateList()
        {
            var result = await service.LoanCategoryCorporateList();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("salarycomponentlist")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> SalaryComponentList()
        {
            var result = await service.SalaryComponentList();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("allowancecomponentlist/{gradeId?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> AllowanceComponentList(string gradeId)
        {
            var result = await service.AllowanceComponentList(gradeId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("deductioncomponentlist")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> DeductionComponentList()
        {
            var result = await service.DeductionComponentList();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("bpjscomponentadditionallist")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> BpjsComponentAdditionalList()
        {
            var result = await service.BpjsComponentAdditionalList();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("bpjscomponentdeductionlist")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> BpjsComponentDecuctionList()
        {
            var result = await service.BpjsComponentDeductionList();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("insurancecompanylist")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> InsuranceCompanyList()
        {
            var result = await service.InsuranceCompanyList();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("pdrcategorylist")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> PdrCategoryList()
        {
            var result = await service.PdrCategoryList();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("pdrgradelist")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> PdrGradeList()
        {
            var result = await service.PdrGradeList();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("legalcategorylist")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> LegalCategoryList()
        {
            var result = await service.LegalCategoryList();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("employeeLegalDoc/{category}/{legalCategoryId?}/{text?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLEmployeeLegalDoc(string category, string legalCategoryId = "", string text = "")
        {
            var result = await service.GetDDLEmployeeLegalDoc(category, legalCategoryId, text);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("LegalDoc/{category}/{CounterpartId}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLLegalDoc(string category, string CounterpartId)
        {
            var result = await service.GetDDLLegalDoc(category, CounterpartId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("bsccategorylist")]
        [ActionRole(ActionMethod.Allowed)]
        public IActionResult GetDDLBscCategoryList()
        {
            var result = new List<SelectListItem>() {
                            new SelectListItem() { Text = BscCategory.FinancialPerspective, Value = BscCategory.FinancialPerspective },
                            new SelectListItem() { Text = BscCategory.CustomerPerspective, Value = BscCategory.CustomerPerspective },
                            new SelectListItem() { Text = BscCategory.OperationalExcellenceAndInnovation, Value = BscCategory.OperationalExcellenceAndInnovation },
                            new SelectListItem() { Text = BscCategory.LearningAndGrowthPerspective, Value = BscCategory.LearningAndGrowthPerspective } };
            return Ok(new ApiResponse(result));
        }


        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("corporateobjectivelist")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLCorporateObjective()
        {
            var result = await service.GetDDLCorporateObjective();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("corporateobjectivename/{bsccategory}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLCorporateObjectiveName(string bscCategory)
        {
            var result = await service.GetDDLCorporateObjectiveName(bscCategory);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("objectiveSettingList")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLObjectiveSetting()
        {
            var result = await service.GetDDLObjectiveSetting();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("employeePayrollGroupList")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLEmployeePayrollGroup()
        {
            var result = await service.GetDDLEmployeePayrollGroup();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("PtkpSettingList")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLPTKPSetting()
        {
            var result = await service.GetDDLPTKPSetting();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("MonthList")]
        [ActionRole(ActionMethod.Allowed)]
        public IActionResult GetDDLMonth()
        {
            var result = Enumerable.Range(1, 12).Select(i => new { Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i), Value = i });
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("YearList")]
        [ActionRole(ActionMethod.Allowed)]
        public IActionResult GetDDLYear()
        {
            var result = new List<SelectListItem>();
            for (int i = 2019; i <= (DateTime.Now.Year + 5); i++)
                result.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            return Ok(new ApiResponse(result));
        }


        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetYearPeriod/{legalEntityId}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetYearPeriod(string legalEntityId)
        {
            var result = await service.GetYearPeriod(legalEntityId);
            return Ok(new ApiResponse(result));
        }
        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("TrainingSetting")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLTrainingSetting()
        {
            var result = await service.GetDDLTrainingSetting();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("TrainingCategory")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLTrainingCategory()
        {
            var result = await service.GetDDLTrainingCategory();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("TrainingSubject")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLTrainingSubject()
        {
            var result = await service.GetDDLTrainingSubject();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("TrainingType")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLTrainingType()
        {
            var result = await service.GetDDLTrainingType();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("religionlist")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLReligion()
        {
            var result = await service.GetDDLReligion();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("getddlmarriedstatus")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLMarriedStatus()
        {
            var result = await service.GetDDLMarriedStatus();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("getddleducationlevel")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLEducationLevel()
        {
            var result = await service.GetDDLEducationLevel();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getddlfamilystatus")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLFamilyStatus()
        {
            var result = await service.GetDDLFamilyStatus();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getddlinstitution")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLInstitution()
        {
            var result = await service.GetDDLInstitution();
            return Ok(new ApiResponse(result));
        }


        [HttpGet("getddlemployementstatus")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLEmployementStatus()
        {
            var result = await service.GetDDLEmployementStatus();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getddlnationality")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLNationality()
        {
            var result = await service.GetDDLNationality();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getddlemployeedata")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLEmployeeData()
        {
            var result = await service.GetDDLEmployeeData();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getInstitutionTypes")]
        [ActionRole(ActionMethod.Allowed)]
        public IActionResult GetGetInstitutionTypes()
        {
            var result = new List<SelectListItem>() {
                            new SelectListItem() { Text = InstitutionTypes.GetName(InstitutionTypes.University), Value = InstitutionTypes.University },
                            new SelectListItem() { Text = InstitutionTypes.GetName(InstitutionTypes.Academy), Value = InstitutionTypes.Academy },
                            new SelectListItem() { Text = InstitutionTypes.GetName(InstitutionTypes.Courses), Value = InstitutionTypes.Courses },
                            new SelectListItem() { Text = InstitutionTypes.GetName(InstitutionTypes.Others), Value = InstitutionTypes.Others }};
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetDDLEmployeeChecklistCategory")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLEmployeeChecklistCategory()
        {
            var result = await service.GetDDLEmployeeChecklistCategory();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetWorkingHour")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetWorkingHour()
        {
            var result = await service.GetWorkingHour();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetMasterTrainingRequest")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetMasterTrainingRequest()
        {
            var result = await service.GetMasterTrainingRequest();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetTrainingCompleted")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetTrainingCompleted()
        {
            var result = await service.GetTrainingCompleted();
            return Ok(new ApiResponse(result));
        }
        #endregion

        #region Media
        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("mediatype")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetMediaType()
        {
            var result = await service.GetMediaType();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("mediaplanrequesttype")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetMediaPlanRequestType()
        {
            var result = await service.GetMediaPlanRequestType();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("company")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetCompany()
        {
            var result = await service.GetCompany();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("get_companys")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetCompanys()
        {
            var result = await service.GetCompanys();
            return Ok(new ApiResponse(result));
        }


        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("client/{id?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetClients(string id = null)
        {
            var result = await service.GetClients(id);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("vendor/{id?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetVendors(string id = null)
        {
            var result = await service.GetVendors(id);
            return Ok(new ApiResponse(result));
        }


        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("rfqvendor/{rfqId}/{vendorId?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetRFQVendors(string rfqId, string vendorId)
        {
            var result = await service.GetRFQVendors(rfqId, vendorId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("brand")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetBrands()
        {
            var result = await service.GetBrands();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("mediaplanno")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetMediaPlanNo()
        {
            var result = await service.GetMediaPlanNo();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("mediaorderstatus")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetMediaOrderStatus()
        {
            var result = await service.GetMediaOrderStatus();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("mediaplanstatus")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetMediaPlanStatus()
        {
            var result = await service.GetMediaPlanStatus();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("programcategory")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetProgramCategory()
        {
            var result = await service.GetProgramCategory();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("programposition")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetProgramPosition()
        {
            var result = await service.GetProgramPosition();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("programtype")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetProgramType()
        {
            var result = await service.GetProgramType();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("vendorinv/{id?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetVendorsForInvRecevied(string id = null)
        {
            var result = await service.GetVendorsForInvRecevied(id);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("cascadejob/{jobid}/{caseData}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetCascadeFromJob(string jobid, string caseData)
        {
            var result = await service.GetCascadeFromJob(jobid, caseData);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("brandasf")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetBrandASF()
        {
            var result = await service.GetBrandASF();
            return Ok(new ApiResponse(result));
        }

        #endregion

        #region Finance
        #region Master Data
        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("organizationlist")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> OrganizationList()
        {
            var result = await service.OrganizationList();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetAffiliation")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetAffiliation()
        {
            var result = await service.GetAffiliation();
            return Ok(new ApiResponse(result));
        }


        [HttpGet("GetFrom")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetFrom()
        {
            var result = await service.GetFrom();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("chartofaccountreportlist")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> ChartofaccountReportList()
        {
            var result = await service.ChartofaccountReportList();
            return Ok(new ApiResponse(result));
        }


        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("accountingcondition")]
        [ActionRole(ActionMethod.Allowed)]
        public IActionResult GetAccountingCondition()
        {
            var result = new List<SelectListItem>() {
                            new SelectListItem() { Text = "Credit", Value = "Credit" },
                            new SelectListItem() { Text = "Debit", Value = "Debit" } };
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("accountingconditionDK")]
        [ActionRole(ActionMethod.Allowed)]
        public IActionResult GetAccountingConditionDK()
        {
            var result = new List<SelectListItem>() {
                            new SelectListItem() { Text = "Credit", Value = "k" },
                            new SelectListItem() { Text = "Debit", Value = "d" } };
            return Ok(new ApiResponse(result));
        }

        [HttpGet("COACodeByNameLv1")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetCOACodeByNameLv1()
        {
            var result = await service.GetCOACodeByNameLv1();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("COACodeByNameLv2/{CodeRec}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetCOACodeByNameLv2(string CodeRec)
        {
            var result = await service.GetCOACodeByNameLv2(CodeRec);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("COACodeByNameLv3/{CodeRec}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetCOACodeByNameLv3(string CodeRec)
        {
            var result = await service.GetCOACodeByNameLv3(CodeRec);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("COACodeByNameLv4/{CodeRec}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetCOACodeByNameLv4(string CodeRec)
        {
            var result = await service.GetCOACodeByNameLv4(CodeRec);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("COACodeByNameLv5/{CodeRec}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetCOACodeByNameLv5(string CodeRec)
        {
            var result = await service.GetCOACodeByNameLv5(CodeRec);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("countrylist")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> CountryList()
        {
            var result = await service.CountryList();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("provincelist/{CountryId}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> ProvinceList(string CountryId)
        {
            var result = await service.ProvincesList(CountryId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("citylist/{ProvinceId}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> CityList(string ProvinceId)
        {
            var result = await service.CityList(ProvinceId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("clientlist")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetClientlist()
        {
            var result = await service.GetClientlist();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("vendorlist")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetVendorList()
        {
            var result = await service.GetVendorList();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("isActive")]
        [ActionRole(ActionMethod.Allowed)]
        public IActionResult GetIsActive()
        {
            var result = new List<SelectListItem>() {
                            new SelectListItem() { Text = "Active", Value = "True" },
                            new SelectListItem() { Text = "Inactice", Value = "False" }
            };
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("AllocationRatio")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetAllocationRatio()
        {
            var result = await service.GetAllocationRatio();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("StatusBudget")]
        [ActionRole(ActionMethod.Allowed)]
        public IActionResult GetStatusBudget()
        {
            var result = new List<SelectListItem>() {
                            new SelectListItem() { Text = "ACTIVE", Value = "Active" },
                            new SelectListItem() { Text = "INACTIVE", Value = "InActive" },
                            new SelectListItem() { Text = "OPEN", Value = "Open" },
                            new SelectListItem() { Text = "CLOSE", Value = "Close" } };
            return Ok(new ApiResponse(result));
        }

        [HttpGet("MasterBudgetCode")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetMasterBudgetCode()
        {
            var result = await service.GetMasterBudgetCode();
            return Ok(new ApiResponse(result));
        }


        [HttpGet("budgetcode/{itemTypeId}/{year}/{hrisDivisionId}/{budgetOwnerId?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetBudgetCode(string itemTypeId, int year, string hrisDivisionId, string budgetOwnerId)
        {
            var result = await service.GetBudgetCode(itemTypeId, year, hrisDivisionId, budgetOwnerId);
            return Ok(new ApiResponse(result));
        }


        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("masterbudgetcodecapex")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetMasterBudgetCodeCapex()
        {
            var result = await service.GetMasterBudgetCode(BudgetCodeType.CAPEX);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("masterbudgetcodeopex")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetMasterBudgetCodeOpex()
        {
            var result = await service.GetMasterBudgetCode(BudgetCodeType.OPEX);
            return Ok(new ApiResponse(result));
        }
        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("MasterMonthlyBudgetSpreads")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetMasterMonthlyBudgetSpreads()
        {
            var result = await service.GetMasterMonthlyBudgetSpreads();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("VersionBudgetCapex/{year}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetVersionBudgetCapex(int year)
        {
            var result = await service.GetVersionBudgetCapex(year);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("VersionBudgetOpex/{year}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetVersionBudgetOpex(int year)
        {
            var result = await service.GetVersionBudgetOpex(year);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("currency")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetCurrency()
        {
            var result = await service.GetCurrencies();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("CurrencyParamsAndDefault/{currencyId}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetCurrencyParamsAndDefault(string currencyId)
        {
            var result = await service.GetCurrencyParamsAndDefault(currencyId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("Operator")]
        [ActionRole(ActionMethod.Allowed)]
        public IActionResult GetOperator()
        {
            var result = new List<SelectListItem>() {
                    new SelectListItem() {  Text ="Multiplied (X)", Value = "*"},
                    new SelectListItem() {  Text ="Divided (:)", Value = "/"}
            };

            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("efaktur/{legalEntityId}/{invoiceDate}/{vatNo?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetEfaktur(string legalEntityId, string invoiceDate, string vatNo)
        {
            var result = await service.GetEfaktur(legalEntityId, invoiceDate, vatNo);
            return Ok(new ApiResponse(result));
        }
        #endregion
        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetFixedAssetByLocation/{locationId?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetFixedAssetByLocation(string locationId = null)
        {
            var result = await service.GetFixedAssetByLocation(locationId);
            return Ok(new ApiResponse(result));
        }
        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("currencyIsNoDefault")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetCurrencyIsNoDefault()
        {
            var result = await service.GetCurrencyIsNoDefault();
            return Ok(new ApiResponse(result));
        }
        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("TAPDDLList")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetTAPDDL()
        {
            var result = await service.GetTAPDDL();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("coa/{param1}/{level}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetChartOfAccount(string param1, string level)
        {
            var result = await service.GetChartOfAccount(param1, level);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetMainServiceCategory")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetMainServiceCategory()
        {
            var result = await service.GetMainServiceCategory();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("PoClient/{clientId}/{invoiceId?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetPurchaceOrderByClientId(string clientId, string invoiceId)
        {
            var result = await service.GetPurchaseOrderByClientId(clientId, invoiceId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetPoByVendorInGoodReceipt/{clientId}/{goodReceiptId?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetPurchaceOrderByClientIdInGoodReceipt(string clientId, string goodReceiptId)
        {
            var result = await service.GetPurchaceOrderByClientIdInGoodReceipt(clientId, goodReceiptId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("PoVendor/{vendorId}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetPurchaseOrderByVendorId(string vendorId)
        {
            var result = await service.GetPurchaseOrderByVendorId(vendorId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("invoiceVendor/{poId}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetInvoiceVendorByPoId(string poId)
        {
            var result = await service.GetInvoiceVendorByPoId(poId);
            return Ok(new ApiResponse(result));
        }
        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("PurchaseRequestNumber/{purchaseId?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetPurchaseRequestNumber(string purchaseId)
        {
            var result = await service.GetPurchaseRequestNumber(purchaseId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetJobDetailInRFQTask")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetJobDetailInRFQTask()
        {
            var result = await service.GetJobDetailInRFQTask();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetRFQOpenPO")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetRFQOpenPO()
        {
            var result = await service.GetRFQOpenPO();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetSpecialRequestPOType")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetSpecialRequestPOType()
        {
            var result = await service.GetSpecialRequestPOType();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetBrands/{company_id}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetBrands(string company_id)
        {
            var result = await service.GetBrands(company_id);
            return Ok(new ApiResponse(result));
        }


        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetBusiness/{affiliationid}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetBusiness(string affiliationid)
        {
            var result = await service.GetBusiness(affiliationid);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetBusinessFinance/{affiliationid}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetBusinessFinance(string affiliationid)
        {
            var result = await service.GetBusinessFinance(affiliationid);
            return Ok(new ApiResponse(result));
        }


        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetBusinessHris/{affiliationid}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetBusinessHris(string affiliationid)
        {
            var result = await service.GetBusinessHris(affiliationid);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetAffiliationByIdAsset/{id}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetAffiliationByIdAsset(string id)
        {
            var result = await service.GetAffiliationByIdAsset(id);
            return Ok(new ApiResponse(result));
        }

        // <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetStatus")]
        [ActionRole(ActionMethod.Allowed)]
        public IActionResult GetStatus()
        {
            var result = new List<SelectListItem>() {
                            new SelectListItem() { Text = StatusTransactionName.Draft, Value = StatusTransactionName.Draft },
                            new SelectListItem() { Text = StatusTransactionName.WaitingApproval, Value = StatusTransactionName.WaitingApproval },
                            new SelectListItem() { Text = StatusTransactionName.Rejected, Value = StatusTransactionName.Rejected },
                            new SelectListItem() { Text = StatusTransactionName.Approved, Value = StatusTransactionName.Approved } };
            return Ok(new ApiResponse(result));
        }

        // <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetStatusPcaPce")]
        [ActionRole(ActionMethod.Allowed)]
        public IActionResult GetStatusPcaPce()
        {
            var result = new List<SelectListItem>() {
                            new SelectListItem() { Text = StatusTransactionName.Draft, Value = StatusTransactionName.Draft },
                            new SelectListItem() { Text = StatusTransactionName.WaitingApproval, Value = StatusTransactionName.WaitingApproval },
                            new SelectListItem() { Text = StatusTransactionName.Rejected, Value = StatusTransactionName.Rejected },
                            new SelectListItem() { Text = StatusTransactionName.Approved, Value = StatusTransactionName.Approved },
                            new SelectListItem() { Text = StatusTransactionName.Revised, Value = StatusTransactionName.Revised } };
            return Ok(new ApiResponse(result));
        }

        // <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetStatusRFQ")]
        [ActionRole(ActionMethod.Allowed)]
        public IActionResult GetStatusRFQ()
        {
            var result = new List<SelectListItem>() {
                            new SelectListItem() { Text = StatusTransactionName.Draft, Value = StatusTransactionName.Draft },
                            new SelectListItem() { Text = StatusTransactionName.OnProcess, Value = StatusTransactionName.OnProcess },
                            new SelectListItem() { Text = "Request Approval", Value = StatusTransactionName.WaitingApproval },
                            new SelectListItem() { Text = StatusTransactionName.Rejected, Value = StatusTransactionName.Rejected },
                            new SelectListItem() { Text = StatusTransactionName.Approved, Value = StatusTransactionName.Approved } };
            return Ok(new ApiResponse(result));
        }

        // <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetStatusPO")]
        [ActionRole(ActionMethod.Allowed)]
        public IActionResult GetStatusPO()
        {
            var result = new List<SelectListItem>() {
                            new SelectListItem() { Text = PurchaseOrderStatus.StatusName(PurchaseOrderStatus.Draft), Value = PurchaseOrderStatus.Draft },
                            new SelectListItem() { Text = PurchaseOrderStatus.StatusName(PurchaseOrderStatus.WaitingApproval), Value = PurchaseOrderStatus.WaitingApproval },
                            new SelectListItem() { Text = PurchaseOrderStatus.StatusName(PurchaseOrderStatus.Rejected), Value = PurchaseOrderStatus.Rejected },
                            new SelectListItem() { Text = PurchaseOrderStatus.StatusName(PurchaseOrderStatus.Approved), Value = PurchaseOrderStatus.Approved } ,
                            //new SelectListItem() { Text = PurchaseOrderStatus.StatusName(PurchaseOrderStatus.CanceledRequest), Value = PurchaseOrderStatus.CanceledRequest },
                            new SelectListItem() { Text = PurchaseOrderStatus.StatusName(PurchaseOrderStatus.Cancelled), Value = PurchaseOrderStatus.Cancelled },
                            //new SelectListItem() { Text = PurchaseOrderStatus.StatusName(PurchaseOrderStatus.ClosedRequest), Value = PurchaseOrderStatus.ClosedRequest },
                            new SelectListItem() { Text = PurchaseOrderStatus.StatusName(PurchaseOrderStatus.Closed), Value = PurchaseOrderStatus.Closed }};
            return Ok(new ApiResponse(result));
        }

        // <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetStatusInvoiceReceived")]
        [ActionRole(ActionMethod.Allowed)]
        public IActionResult GetStatusInvoiceReceived()
        {
            var result = new List<SelectListItem>() {
                    new SelectListItem() {  Text =InvoiceReceivedStatus.StatusName(InvoiceReceivedStatus.Open), Value = InvoiceReceivedStatus.Open},
                    new SelectListItem() {  Text =InvoiceReceivedStatus.StatusName(InvoiceReceivedStatus.TAP), Value = InvoiceReceivedStatus.TAP},
                    new SelectListItem() {  Text =InvoiceReceivedStatus.StatusName(InvoiceReceivedStatus.TAXTAP), Value = InvoiceReceivedStatus.TAXTAP},
                    new SelectListItem() {  Text =InvoiceReceivedStatus.StatusName(InvoiceReceivedStatus.OpenAP), Value = InvoiceReceivedStatus.OpenAP},
                    new SelectListItem() {  Text =InvoiceReceivedStatus.StatusName(InvoiceReceivedStatus.Posted), Value = InvoiceReceivedStatus.Posted},
                    new SelectListItem() {  Text =InvoiceReceivedStatus.StatusName(InvoiceReceivedStatus.Rejected), Value = InvoiceReceivedStatus.Rejected},
                    new SelectListItem() {  Text =InvoiceReceivedStatus.StatusName(InvoiceReceivedStatus.Cancelled), Value = InvoiceReceivedStatus.Cancelled},
            };

            return Ok(new ApiResponse(result));
        }

        // <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetStatusTAP")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetStatusTAP()
        {
            var result = new List<SelectListItem>() {
                    new SelectListItem() {  Text =InvoiceReceivedStatus.StatusName(InvoiceReceivedStatus.TAP), Value = InvoiceReceivedStatus.TAP},
                    new SelectListItem() {  Text =InvoiceReceivedStatus.StatusName(InvoiceReceivedStatus.TAXTAP), Value = InvoiceReceivedStatus.TAXTAP},
                    new SelectListItem() {  Text =InvoiceReceivedStatus.StatusName(InvoiceReceivedStatus.OpenAP), Value = InvoiceReceivedStatus.OpenAP},
                    new SelectListItem() {  Text =InvoiceReceivedStatus.StatusName(InvoiceReceivedStatus.Posted), Value = InvoiceReceivedStatus.Posted},
                    new SelectListItem() {  Text =InvoiceReceivedStatus.StatusName(InvoiceReceivedStatus.Rejected), Value = InvoiceReceivedStatus.Rejected},
                    new SelectListItem() {  Text =InvoiceReceivedStatus.StatusName(InvoiceReceivedStatus.Cancelled), Value = InvoiceReceivedStatus.Cancelled},
            };

            var group = await userApp.GetByUserLogin();
            //var FrontDesk = group.Where(x => x.GroupId == GroupAcess.FrontDesk).Count() > 0;
            //var AdminFinance = group.Where(x => x.GroupId == GroupAcess.AdminFinance).Count() > 0;
            //var AccountingSupervisor = group.Where(x => x.GroupId == GroupAcess.AccountingSupervisor).Count() > 0;
            var TaxSupervisor = group.Where(x => x.GroupId == GroupAcess.TaxSupervisor).Count() > 0;
            var FinanceManager = group.Where(x => x.GroupId == GroupAcess.FinanceManager).Count() > 0;

            if (TaxSupervisor || FinanceManager) result.Remove(result.Where(x => x.Value == InvoiceReceivedStatus.TAP).FirstOrDefault());
            if (FinanceManager) { result.Remove(result.Where(x => x.Value == InvoiceReceivedStatus.TAXTAP).FirstOrDefault()); }

            return Ok(new ApiResponse(result));
        }

        // <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetStatusAP")]
        [ActionRole(ActionMethod.Allowed)]
        public IActionResult GetStatusAP()
        {
            var result = new List<SelectListItem>() {
                    new SelectListItem() {  Text =InvoiceReceivedStatus.StatusName(InvoiceReceivedStatus.Open), Value = InvoiceReceivedStatus.Open},
                    new SelectListItem() {  Text =InvoiceReceivedStatus.StatusName(InvoiceReceivedStatus.TAP), Value = InvoiceReceivedStatus.TAP},
                    new SelectListItem() {  Text =InvoiceReceivedStatus.StatusName(InvoiceReceivedStatus.TAXTAP), Value = InvoiceReceivedStatus.TAXTAP},
                    new SelectListItem() {  Text =InvoiceReceivedStatus.StatusName(InvoiceReceivedStatus.OpenAP), Value = InvoiceReceivedStatus.OpenAP},
                    new SelectListItem() {  Text =InvoiceReceivedStatus.StatusName(InvoiceReceivedStatus.Posted), Value = InvoiceReceivedStatus.Posted},
                    new SelectListItem() {  Text =InvoiceReceivedStatus.StatusName(InvoiceReceivedStatus.Rejected), Value = InvoiceReceivedStatus.Rejected},
                    new SelectListItem() {  Text =InvoiceReceivedStatus.StatusName(InvoiceReceivedStatus.Cancelled), Value = InvoiceReceivedStatus.Cancelled},
            };
            return Ok(new ApiResponse(result));
        }


        // <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetStatusPaymentRequest")]
        [ActionRole(ActionMethod.Allowed)]
        public IActionResult GetStatusPaymentRequest()
        {
            var result = new List<SelectListItem>() {
                    new SelectListItem() {  Text =PaymentRequestStatus.GetStatus(PaymentRequestStatus.Open), Value = PaymentRequestStatus.Open},
                    new SelectListItem() {  Text =PaymentRequestStatus.GetStatus(PaymentRequestStatus.WaitingApproval), Value = PaymentRequestStatus.WaitingApproval},
                    new SelectListItem() {  Text =PaymentRequestStatus.GetStatus(PaymentRequestStatus.Reject), Value = PaymentRequestStatus.Reject},
                    new SelectListItem() {  Text =PaymentRequestStatus.GetStatus(PaymentRequestStatus.Complete), Value = PaymentRequestStatus.Complete},
                    new SelectListItem() {  Text =PaymentRequestStatus.GetStatus(PaymentRequestStatus.Reconsiliation), Value = PaymentRequestStatus.Reconsiliation},
            };
            return Ok(new ApiResponse(result));
        }

        // <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetPCETypeCrossBilling")]
        [ActionRole(ActionMethod.Allowed)]
        public IActionResult GetPCETypeCrossBilling()
        {
            var result = new List<SelectListItem>() {
                    new SelectListItem() {  Text ="Yes", Value = "true" },
                    new SelectListItem() {  Text ="No", Value = "false"}
            };
            return Ok(new ApiResponse(result));
        }


        // <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetInvoicing")]
        [ActionRole(ActionMethod.Allowed)]
        public IActionResult GetInvoicing()
        {
            var result = new List<SelectListItem>() {
                            new SelectListItem() { Text = "MULTIPLE", Value = "MULTIPLE" },
                            new SelectListItem() { Text = "SINGLE", Value ="SINGLE" } };
            return Ok(new ApiResponse(result));
        }

        ///<summary>
        /// GetAll record Inventory Good Receipts
        ///</summary>
        [HttpGet("GetInventoryGoodsReceipt/{poId}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetInventoryGoodsReceipt(string poId)
        {
            var result = await service.GetInventoryGoodsReceipt(poId);
            return Ok(new ApiResponse(result));
        }

        ///<summary>
        /// GetAll record Inventory Good Receipts
        ///</summary>
        [HttpGet("GetServiceReceipt/{poId}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetServiceReceipt(string poId)
        {
            var result = await service.GetServiceReceipt(poId);
            return Ok(new ApiResponse(result));
        }

        ///<summary>
        /// GetAll record Inventory
        ///</summary>
        [HttpGet("GetInventory")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetInventory()
        {
            var result = await service.GetInventory();
            return Ok(new ApiResponse(result));
        }

        ///<summary>
        /// GetAll record Fixed Asset
        ///</summary>
        [HttpGet("GetFixedAsset")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetFixedAsset()
        {
            var result = await service.GetFixedAsset();
            return Ok(new ApiResponse(result));
        }

        ///<summary>
        /// GetAll record Fixed Asset
        ///</summary>
        [HttpGet("GetFixedAssetAP/{journalId?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetFixedAssetAP(string journalId)
        {
            var result = await service.GetFixedAssetAP(journalId);
            return Ok(new ApiResponse(result));
        }

        ///<summary>
        /// GetAll record Fixed Asset Comobine
        ///</summary>
        [HttpGet("GetFixedAssetCombine")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetFixedAssetCombine()
        {
            var result = await service.GetFixedAssetCombine();
            return Ok(new ApiResponse(result));
        }

        ///<summary>
        /// GetAll record Fixed Asset Comobine
        ///</summary>
        [HttpGet("GetFixedAssetCombineAll")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetFixedAssetCombineAll()
        {
            var result = await service.GetFixedAssetCombineAll();
            return Ok(new ApiResponse(result));
        }


        ///<summary>
        /// GetAll record Fixed Asset Comobine
        ///</summary>
        [HttpGet("GetCOAID")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetCOAID()
        {
            var result = await service.GetCOAID();
            return Ok(new ApiResponse(result));
        }


        ///<summary>
        /// GetAll record Inventory Good Request
        ///</summary>
        [HttpGet("GetGoodRequest")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetGoodRequest()
        {
            var result = await service.GetGoodRequest();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetOrderReceipt/{id?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetOrderReceipt(string id = null)
        {
            var result = await service.GetOrderReceipt(id);
            return Ok(new ApiResponse(result));
        }

        ///<summary>
        /// GetAll record Inventory Good Receipts
        ///</summary>
        [HttpGet("GetCoaInvoice")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetCoaInvoice()
        {
            var result = await service.GetCoaInvoice();
            return Ok(new ApiResponse(result));
        }

        ///<summary>
        /// GetAll record Inventory Good Receipts
        ///</summary>
        [HttpGet("GetCoaInvoiceTaxPayable")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetCoaInvoiceTaxPayable()
        {
            var result = await service.GetCoaInvoiceTaxPayable();
            return Ok(new ApiResponse(result));
        }

        ///<summary>
        /// GetAll record Inventory Good Receipts
        ///</summary>
        [HttpGet("GetCoaInvoiceClient")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetCoaInvoiceClient()
        {
            var result = await service.GetCoaInvoiceClient();
            return Ok(new ApiResponse(result));
        }

        // <summary>
        /// Gets status for Invoicing Client.
        /// </summary>
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetInvoiceClientStatus")]
        [ActionRole(ActionMethod.Allowed)]
        public IActionResult GetInvoiceClientStatus()
        {
            var result = new List<SelectListItem>() {
                    new SelectListItem() {  Text =InvoiceClientStatus.StatusName(InvoiceClientStatus.Open), Value = InvoiceClientStatus.Open.ToString()},
                    new SelectListItem() {  Text =InvoiceClientStatus.StatusName(InvoiceClientStatus.WaitingApproval), Value = InvoiceClientStatus.WaitingApproval.ToString()},
                    new SelectListItem() {  Text =InvoiceClientStatus.StatusName(InvoiceClientStatus.Approved), Value = InvoiceClientStatus.Approved.ToString()},
                    new SelectListItem() {  Text =InvoiceClientStatus.StatusName(InvoiceClientStatus.Rejected), Value = InvoiceClientStatus.Rejected.ToString()}
            };
            return Ok(new ApiResponse(result));
        }

        // <summary>
        /// Gets status for Account Receivable
        /// </summary>
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetArStatus")]
        [ActionRole(ActionMethod.Allowed)]
        public IActionResult GetArStatus()
        {
            var result = new List<SelectListItem>()
            {
                new SelectListItem() { Text = ARStatus.Open, Value = ARStatus.Open },
                new SelectListItem() { Text = ARStatus.Posted, Value = ARStatus.Posted}
            };
            return Ok(new ApiResponse(result));
        }

        // <summary>
        /// Gets status for Account Receivable
        /// </summary>
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetInvoiceDeliveryNoteStatus")]
        [ActionRole(ActionMethod.Allowed)]
        public IActionResult GetInvoiceDeliveryNoteStatus()
        {
            var result = new List<SelectListItem>()
            {
                new SelectListItem() { Text = InvoiceDeliveryNoteStatus.Open, Value = InvoiceDeliveryNoteStatus.Open },
                new SelectListItem() { Text = InvoiceDeliveryNoteStatus.Closed, Value = InvoiceDeliveryNoteStatus.Closed}
            };
            return Ok(new ApiResponse(result));
        }

        ///<summary>
        /// GetAll record Inventory Good Receipts
        ///</summary>
        [HttpGet("GetBanks")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetBanks()
        {
            var result = await service.GetBanks();
            return Ok(new ApiResponse(result));
        }

        ///<summary>
        /// GetAll record Inventory Good Receipts
        ///</summary>
        [HttpGet("GetBankByLegalEntity/{legalEntityId?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetBankByLegalEntity(string legalEntityId)
        {
            var result = await service.GetBankByLegalEntity(legalEntityId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetPettyCash")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetPettyCash()
        {
            var result = await service.GetPettyCash();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetPettyCashByLegalEntity/{legalEntityId?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetPettyCashByLegalEntity(string legalEntityId)
        {
            var result = await service.GetPettyCashByLegalEntity(legalEntityId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetBankandPettyCash")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetBankandPettyCash()
        {
            var result = await service.GetBankandPettyCash();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("clientinv")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetClientInv()
        {
            var result = await service.GetClientInv();
            return Ok(new ApiResponse(result));
        }


        ///// <summary>
        ///// Gets all records with Text Value property. It is used for dropdownlist.
        ///// </summary>
        ///// <returns>List of json object with Text Value property.</returns>
        //[HttpGet("GetBankReceivedType")]
        //[ActionRole(ActionMethod.Allowed)]
        //public async Task<IActionResult> GetBankReceivedType()
        //{
        //    var result = await service.GetBankReceivedType();
        //    return Ok(new ApiResponse(result));
        //}


        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetPCA")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetPCA()
        {
            var result = await service.GetPCA();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetPCE")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetPCE()
        {
            var result = await service.GetPCE();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetFiscalYear")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetFiscalYear()
        {
            var result = await service.GetFiscalYear();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetFiscalYearByLegalId/{legalId}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetFiscalYearByLegalId(string legalId)
        {
            var result = await service.GetFiscalYearByLegalId(legalId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetFiscalPeriod/{Id}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetFiscalPeriod(string Id)
        {
            var result = await service.GetFiscalPeriod(Id);
            return Ok(new ApiResponse(result));
        }


        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetMasterLocationProcurement")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetMasterLocationProcurement()
        {
            var result = await service.GetMasterLocationProcurement();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetMasterConditionProcurement")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetMasterConditionProcurement()
        {
            var result = await service.GetMasterConditionProcurement();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetCostSharingAllocation")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> CostSharingAllocationList()
        {
            var result = await service.CostSharingAllocationList();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetMasterDepreciation")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetMasterDepreciation()
        {
            var result = await service.GetMasterDepreciation();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("FixedAssetMovementStatus")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetFixedAssetMovementStatus()
        {
            var result = await service.GetFixedAssetMovementStatus();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("vendorbank")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetBankVendor()
        {
            var result = await service.GetBankVendor();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("InventoryDetailstatus")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetInventoryDetailStatus()
        {
            var result = await service.GetDetailStatus();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetTaskCAS/{casId}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetTaskCAS(string casId)
        {
            var result = await service.GetTaskCAS(casId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("get_vendornames")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetVendorNames()
        {
            var result = await service.GetVendorNames();
            return Ok(new ApiResponse(result));
        }


        [HttpGet("get_itemcategories")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetItemCategory()
        {
            var result = await service.GetItemCategory();
            return Ok(new ApiResponse(result));
        }

        // <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetJournalAdjustmentType")]
        [ActionRole(ActionMethod.Allowed)]
        public IActionResult GetJournalAdjustmentType()
        {
            var result = new List<SelectListItem>() {
                            new SelectListItem() { Text = JournalAdjustmentType.AccountPayable, Value = JournalAdjustmentType.AccountPayable },
                            new SelectListItem() { Text = JournalAdjustmentType.AccountReceivable, Value = JournalAdjustmentType.AccountReceivable },
                            new SelectListItem() { Text = JournalAdjustmentType.CashAdvanceSettlement, Value = JournalAdjustmentType.CashAdvanceSettlement },
                            new SelectListItem() { Text = JournalAdjustmentType.BankPayment, Value = JournalAdjustmentType.BankPayment },
                            new SelectListItem() { Text = JournalAdjustmentType.BankReceived, Value = JournalAdjustmentType.BankReceived },
                            new SelectListItem() { Text = JournalAdjustmentType.PettyCashSettlement, Value = JournalAdjustmentType.PettyCashSettlement } };
            return Ok(new ApiResponse(result));
        }

        // <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetSalaryAllocationType")]
        [ActionRole(ActionMethod.Allowed)]
        public IActionResult GetSalaryAllocationType()
        {
            var result = new List<SelectListItem>() {
                            new SelectListItem() { Text = SalaryAllocationType.AccountPayable, Value = SalaryAllocationType.AccountPayable },
                            new SelectListItem() { Text = SalaryAllocationType.BankPayment, Value = SalaryAllocationType.BankPayment }};
            return Ok(new ApiResponse(result));
        }

        // <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpPost("GetJournalAdjustmentNumber")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetJournalAdjustmentNumber([FromBody] JournalAdjustmentSearchNumber search)
        {
            var result = await service.GetJournalAdjustmentNumber(search);
            return Ok(new ApiResponse(result));
        }

        // <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpPost("GetSalaryAllocationNumber")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetSalaryAllocationNumber([FromBody] SalaryAllocationSearchNumber search)
        {
            var result = await service.GetSalaryAllocationNumber(search);
            return Ok(new ApiResponse(result));
        }


        // <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetReportType")]
        [ActionRole(ActionMethod.Allowed)]
        public IActionResult GetReportType()
        {
            var result = new List<SelectListItem>() {
                            new SelectListItem() { Text = ReportType.ManagementReport, Value = ReportType.ManagementReport },
                            new SelectListItem() { Text = ReportType.ActualReport, Value = ReportType.ActualReport } };
            return Ok(new ApiResponse(result));
        }


        ///<summary>
        /// GetAll record Fixed Asset Comobine
        ///</summary>
        [HttpGet("getcoalevel")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetCOALevel()
        {
            var result = await service.GetCOALevel();
            return Ok(new ApiResponse(result));
        }

        #endregion

        #region PM
        [HttpGet("Job")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetJob()
        {
            var result = await service.GetJobId();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("Channel")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetChannel()
        {
            var result = await service.GetChannel();
            return Ok(new ApiResponse(result));
        }

        #endregion


        #region PM
        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <param name="JobId">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("JobNotExistsInPCAandJobId/{Status}/{JobId?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetJobNotExistsInPCAandJobId(string Status, string JobId = "")
        {
            var result = await service.GetJobNotExistsInPCAandJobId(Status, JobId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <param name="JobId">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("JobNotExistsInPCEandJobId/{Status}/{JobId?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetJobNotExistsInPCEandJobId(string Status, string JobId = "")
        {
            var result = await service.GetJobNotExistsInPCEandJobId(Status, JobId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("MotherPca")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetMotherPca()
        {
            var result = await service.GetMotherPca();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("RateCard")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetRateCard()
        {
            var result = await service.GetRateCard();
            return Ok(new ApiResponse(result));
        }


        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="rateCardId">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("TaskRateCard/{rateCardId}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetTaskRateCard(string rateCardId)
        {
            var result = await service.GetTaskRateCard(rateCardId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <param name="rateCardId">Key id parameter.</param>
        /// <param name="taskRateCardName">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("SubTaskRateCard/{rateCardId}/{taskRateCardName}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetSubTaskRateCard(string rateCardId, string taskRateCardName)
        {
            var result = await service.GetSubTaskRateCard(rateCardId, taskRateCardName);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("itemtype")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetItemType()
        {
            var result = await service.GetItemType();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("COALevel/{level}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetCOALevel(string level)
        {
            var result = await service.GetCOALevel(level);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("COAByNameLv5")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetCOAByNameLv5()
        {
            var result = await service.GetCOAByNameLv5();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("COABank")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetCOABank()
        {
            var result = await service.GetCOABank();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetCOAOpeningBalance")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetCOAOpeningBalance()
        {
            var result = await service.GetCOAOpeningBalance();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("COAByNameAPLv5")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetCOAByNameAPLv5()
        {
            var result = await service.GetCOAByNameAPLv5();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("COAByNameARLv5")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetCOAByNameARLv5()
        {
            var result = await service.GetCOAByNameARLv5();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("coalv5")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetChartOfAccountLv5()
        {
            var result = await service.GetChartOfAccountLv5();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("coalv5like/{search}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetChartOfAccountLv5Like(string search)
        {
            var result = await service.GetChartOfAccountLv5Like(search);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("TypeOfExpense")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetTypeOfExpense()
        {
            var result = await service.GetTypeOfExpense();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("MainserviceCategory")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetMainserviceCategory()
        {
            var result = await service.GetMainserviceCategory();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("MainserviceCategoryPCA")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetMainserviceCategoryPCA()
        {
            var result = await service.GetMainserviceCategoryPCA();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("Shareservices")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetShareservices()
        {
            var result = await service.GetShareservices();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("Shareservices/{MainserviceCategoryId}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetShareservices(string MainserviceCategoryId)
        {
            var result = await service.GetShareservices(MainserviceCategoryId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("unit")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetUnitList()
        {
            var result = await service.GetUom();
            //var result = new List<SelectListItem>() {
            //                new SelectListItem() { Text = "Unit", Value = "Unit" },
            //                new SelectListItem() { Text ="kg", Value ="kg" },
            //                new SelectListItem() { Text = "pcs", Value ="pcs" },
            //                new SelectListItem() { Text ="md", Value = "md" } };
            return Ok(new ApiResponse(result));
        }


        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("Departments")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDepartments()
        {
            var result = await service.GetDepartments();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("UserApp")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetUsers()
        {
            var result = await service.GetUsers();
            return Ok(new ApiResponse(result));
        }

        ///// <summary>
        ///// Gets all records without parameter as criteria.
        ///// </summary>
        ///// <returns>List of json object.</returns>
        //[HttpGet("MasterDepartments")]
        //[ActionRole(ActionMethod.Allowed)]
        //public async Task<IActionResult> GetMasterDepartments()
        //{
        //    var result = await service.GetMasterDepartments();
        //    return Ok(new ApiResponse(result));
        //}
        // <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("OtherFee")]
        [ActionRole(ActionMethod.Allowed)]
        public IActionResult GetOtherFee()
        {
            var result = new List<SelectListItem>() {
                            new SelectListItem() { Text = "Agency Service Fee", Value = "Agency Service Fee" },
                            new SelectListItem() { Text ="Retainer Fee", Value ="Retainer Fee" }
            };
            return Ok(new ApiResponse(result));
        }

        // <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("JobId")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetJobId()
        {
            var result = await service.GetJobId();
            return Ok(new ApiResponse(result));
        }

        // <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetJobByPCEApproved")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetJobByPCEApproved()
        {
            var result = await service.GetJobByPCEApproved();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <param name="pceId">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("PCEReference/{pceId?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetPCEReference(string pceId = "")
        {
            var result = await service.GetPCEReference(pceId);
            return Ok(new ApiResponse(result));
        }

        // <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("StatusPCE")]
        [ActionRole(ActionMethod.Allowed)]
        public IActionResult GetStatusPCE()
        {
            var result = new List<SelectListItem>() {
                            new SelectListItem() { Text = "Ready to Invoice", Value = PCEStatus.Approved },
                            new SelectListItem() { Text =PCEStatus.Revise, Value =PCEStatus.Revise }
            };
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("masteroutletlist/{clientId?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> MasterOutletList(string clientId)
        {
            var result = await service.MasterOutletList(clientId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetDDLJobList")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLJobList()
        {
            var result = await service.GetDDLJobList();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetDDLFolderNameMyDam")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLFolderNameMyDam()
        {
            var result = await service.GetDDLFolderNameMyDam();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetDDLJobNameMyDam")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLJobNameMyDam()
        {
            var result = await service.GetDDLJobNameMyDam();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetDDLClientNameMyDam")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLClientNameMyDam()
        {
            var result = await service.GetDDLClientNameMyDam();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetDDLCampaignNameMyDam")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetDDLCampaignNameMyDam()
        {
            var result = await service.GetDDLCampaignNameMyDam();
            return Ok(new ApiResponse(result));
        }


        #endregion
    }
}
