using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Enum;
using Phoenix.Data.Services.Um;
using Phoenix.Shared.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Data
{
    public interface IDropdownService
    {
        #region Global
        Task<dynamic> GetUom();
        Task<dynamic> GetJobGrades();
        Task<dynamic> GetMenu();
        Task<dynamic> GetJobTitles();
        Task<dynamic> GetJobLevels();
        Task<dynamic> GetSubGroups();
        Task<dynamic> GetDivision();
        Task<dynamic> GetDivisionHris();
        Task<dynamic> GetDivisionFinance();
        Task<dynamic> GetApNumber();
        Task<dynamic> GetArNumber(string bankReceivedId);
        Task<dynamic> GetLegalEntity();
        Task<dynamic> GetDivisions(string subGroupId);
        Task<dynamic> GetDepartments(string divisionId);
        Task<dynamic> GetLocations();
        Task<dynamic> GetFamily(string employeeId);
        Task<dynamic> GetItemType();
        Task<dynamic> GetDepartments();
        Task<dynamic> GetUsers();
        //Task<dynamic> GetMasterDepartments();
        Task<dynamic> GetPaymentType();
        Task<dynamic> GetItemCode(string ItemTypeId);
        Task<dynamic> GetItemInventoryAndFixedAsset(string ItemTypeId);
        Task<dynamic> GetGroups();
        Task<dynamic> GetSubGroupsHris(string GroupId);
        Task<dynamic> GetDivisionsHris(string subGroupId);
        Task<dynamic> GetDepartmentsHris(string divisionId);
        Task<dynamic> GetWorkingHour();
        #endregion

        #region HRIS
        Task<dynamic> GetMasterDivisionSelfservice();
        Task<dynamic> GetEmployeeBasicInfo();
        Task<dynamic> GetEmployeeBasicInfoByName(string name);
        Task<dynamic> GetDDLPayrollComponent(string DeclarationTypeId);
        Task<dynamic> GetTerminationEmployeeBasicInfoByName(string name, string terminationType);
        Task<dynamic> GetEmployeeBasicInfoByHead(string name);
        Task<dynamic> GetEmployeeBasicInfoMasterCounterPart(string id);
        Task<dynamic> GetEmployeeResignationType();
        Task<dynamic> GetEmployeeTerminationType();
        Task<dynamic> GetEmployeeDeathType();
        Task<dynamic> FaultCategoryList();
        Task<dynamic> ServiceNameList();
        Task<dynamic> CategoryExitInterviewList();
        Task<dynamic> LeaveRequestTypeList();
        Task<dynamic> leaverequesttypelistemployeeid(string employeeid);
        Task<dynamic> LoanCategoryList();
        Task<dynamic> LoanCategoryCorporateList();
        Task<dynamic> SalaryComponentList();
        Task<dynamic> AllowanceComponentList(string gradeId);
        Task<dynamic> DeductionComponentList();
        Task<dynamic> BpjsComponentAdditionalList();
        Task<dynamic> BpjsComponentDeductionList();
        Task<dynamic> InsuranceCompanyList();
        Task<dynamic> PdrCategoryList();
        Task<dynamic> PdrGradeList();
        Task<dynamic> LegalCategoryList();
        Task<dynamic> GetDDLEmployeeLegalDoc(string category, string legalCategoryId, string text);
        Task<dynamic> GetDDLLegalDoc(string category, string CounterpartId);
        Task<dynamic> GetDDLCorporateObjective();
        Task<dynamic> GetDDLCorporateObjectiveName(string bscCategory);
        Task<dynamic> GetDDLObjectiveSetting();
        Task<dynamic> GetDDLEmployeePayrollGroup();
        Task<dynamic> GetDDLPTKPSetting();
        Task<dynamic> GetDDLTrainingSetting();
        Task<dynamic> GetDDLTrainingCategory();
        Task<dynamic> GetDDLTrainingSubject();
        Task<dynamic> GetDDLTrainingType();
        Task<dynamic> GetDDLReligion();
        Task<dynamic> GetDDLMarriedStatus();
        Task<dynamic> GetDDLEducationLevel();
        Task<dynamic> GetDDLFamilyStatus();
        Task<dynamic> GetDDLInstitution();
        Task<dynamic> GetDDLEmployementStatus();
        Task<dynamic> GetDDLNationality();
        Task<dynamic> GetDDLEmployeeData();

        Task<dynamic> GetDDLEmployeeChecklistCategory();
        Task<dynamic> GetDDLJobGradesWithLevel(string id);
        Task<dynamic> GetDepartmentById(string id);

        Task<dynamic> GetMasterTrainingRequest();

        Task<dynamic> GetTrainingCompleted();


        #endregion

        #region Media
        Task<dynamic> GetMediaType();
        Task<dynamic> GetMediaPlanRequestType();
        Task<dynamic> GetMediaOrderStatus();
        Task<dynamic> GetMediaPlanStatus();
        Task<dynamic> GetProgramCategory();
        Task<dynamic> GetProgramPosition();
        Task<dynamic> GetProgramType();
        Task<dynamic> GetCascadeFromJob(string jobid, string caseData);
        Task<dynamic> GetBrandASF();
        #endregion

        #region Finance
        #region Master Data
        Task<dynamic> OrganizationList();
        Task<dynamic> GetAffiliation();
        Task<dynamic> GetFrom();
        Task<dynamic> ChartofaccountReportList();
        Task<dynamic> GetCOACodeByNameLv1();
        Task<dynamic> GetCOACodeByNameLv2(string CodeRec);
        Task<dynamic> GetCOACodeByNameLv3(string CodeRec);
        Task<dynamic> GetCOACodeByNameLv4(string CodeRec);
        Task<dynamic> GetCOACodeByNameLv5(string CodeRec);
        Task<dynamic> CountryList();
        Task<dynamic> ProvincesList(string countryId);
        Task<dynamic> CityList(string ProvinceId);
        Task<dynamic> GetClientlist();
        Task<dynamic> GetVendorList();
        Task<dynamic> GetMasterConditionProcurement();
        Task<dynamic> CostSharingAllocationList();
        Task<dynamic> GetFixedAssetMovementStatus();
        Task<dynamic> GetAllocationRatio();
        Task<dynamic> GetMasterBudgetCode();
        Task<dynamic> GetBudgetCode(string itemTypeId, int year, string hrisDivisionId, string budgetOwnerId);
        Task<dynamic> GetMasterBudgetCode(string type);
        Task<dynamic> GetMasterMonthlyBudgetSpreads();
        Task<dynamic> GetVersionBudgetCapex(int year);
        Task<dynamic> GetVersionBudgetOpex(int year);
        Task<dynamic> GetYearPeriod(string legalEntityId);
        #endregion
        Task<dynamic> GetCompany();
        Task<dynamic> GetCompanys();
        Task<dynamic> GetClients(string id);
        Task<dynamic> GetVendors(string id);
        Task<dynamic> GetRFQVendors(string rfqId, string vendorId);
        Task<dynamic> GetBrands();
        Task<dynamic> GetMediaPlanNo();
        Task<dynamic> GetCurrencies();
        Task<dynamic> GetCurrencyParamsAndDefault(string currencyId);
        Task<dynamic> GetEfaktur(string legalEntityId, string invoiceDate, string vatNo);
        Task<dynamic> GetCurrencyIsNoDefault();
        Task<dynamic> GetTAPDDL();
        Task<dynamic> GetPurchaseOrderByClientId(string clietnId, string invoiceId);
        Task<dynamic> GetPurchaseOrderByVendorId(string vendorId);
        Task<dynamic> GetInvoiceVendorByPoId(string poId);
        Task<dynamic> GetChartOfAccount(string code_rec_1, string level);
        Task<dynamic> GetCOALevel(string level);
        Task<dynamic> GetCOAByNameLv5();
        Task<dynamic> GetCOABank();
        Task<dynamic> GetCOAOpeningBalance();
        Task<dynamic> GetCOAByNameAPLv5();
        Task<dynamic> GetCOAByNameARLv5();
        Task<dynamic> GetChartOfAccountLv5();
        Task<dynamic> GetChartOfAccountLv5Like(string search);
        Task<dynamic> GetMainServiceCategory();
        Task<dynamic> GetPurchaseRequestNumber(string purchaseId);
        Task<dynamic> GetJobDetailInRFQTask();
        Task<dynamic> GetRFQOpenPO();
        Task<dynamic> GetSpecialRequestPOType();
        Task<dynamic> GetBrands(string company_id);
        Task<dynamic> GetBusiness(string affiliationid);

        Task<dynamic> GetAffiliationByIdAsset(string id);

        Task<dynamic> GetInventoryGoodsReceipt(string poId);
        Task<dynamic> GetServiceReceipt(string poId);
        Task<dynamic> GetGoodRequest();
        Task<dynamic> GetCoaInvoice();
        Task<dynamic> GetCoaInvoiceTaxPayable();
        Task<dynamic> GetBanks();
        Task<dynamic> GetBankByLegalEntity(string legalEntityId);
        Task<dynamic> GetCoaInvoiceClient();
        Task<dynamic> GetVendorsForInvRecevied(string id);
        Task<dynamic> GetClientInv();
        //Task<dynamic> GetBankReceivedType();
        Task<dynamic> GetPCA();
        Task<dynamic> GetPCE();
        Task<dynamic> GetFiscalYear();
        Task<dynamic> GetFiscalYearByLegalId(string legalId);
        Task<dynamic> GetFiscalPeriod(string Id);
        Task<dynamic> GetMasterLocationProcurement();
        Task<dynamic> GetBankVendor();
        Task<dynamic> GetFixedAsset();
        Task<dynamic> GetFixedAssetAP(string journalId);
        Task<dynamic> GetFixedAssetByLocation(string locationId);
        Task<dynamic> GetFixedAssetCombine();
        Task<dynamic> GetFixedAssetCombineAll();
        Task<dynamic> GetCOAID();
        Task<dynamic> GetMasterDepreciation();
        Task<dynamic> GetInventory();
        Task<dynamic> GetOrderReceipt(string id);
        Task<dynamic> GetPettyCash();
        Task<dynamic> GetPettyCashByLegalEntity(string legalEntityId);
        Task<dynamic> GetBankandPettyCash();
        Task<dynamic> GetDetailStatus();
        Task<dynamic> MoveLocation();
        Task<dynamic> LocationWarehouse();
        Task<dynamic> InvGetform(string param);
        Task<dynamic> GetTaskCAS(string casId);
        Task<dynamic> GetVendorNames();
        Task<dynamic> GetItemCategory();
        Task<dynamic> GetJournalAdjustmentNumber(JournalAdjustmentSearchNumber search);
        Task<dynamic> GetSalaryAllocationNumber(SalaryAllocationSearchNumber search);
        Task<dynamic> GetCOALevel();
        Task<dynamic> GetBusinessHris(string affiliationid);
        Task<dynamic> GetBusinessFinance(string affiliationid);
        #endregion

        #region PM
        Task<dynamic> GetJobNotExistsInPCAandJobId(string Status, string JobId);
        Task<dynamic> GetJobNotExistsInPCEandJobId(string Status, string JobId);
        Task<dynamic> GetMotherPca();
        Task<dynamic> GetRateCard();
        Task<dynamic> GetTaskRateCard(string rateCardId);
        Task<dynamic> GetSubTaskRateCard(string rateCardId, string taskRateCardName);
        Task<dynamic> GetTypeOfExpense();
        Task<dynamic> GetMainserviceCategory();
        Task<dynamic> GetMainserviceCategoryPCA();
        Task<dynamic> GetShareservices();
        Task<dynamic> GetShareservices(string MainserviceCategoryId);
        Task<dynamic> GetJobId();
        Task<dynamic> GetChannel();
        Task<dynamic> GetJobByPCEApproved();
        Task<dynamic> GetPCEReference(string pceId);
        Task<dynamic> MasterOutletList(string clientId);
        Task<dynamic> GetPurchaceOrderByClientIdInGoodReceipt(string clientId, string grId);
        Task<dynamic> GetDDLNonJobByBusinessUnitId();
        Task<dynamic> GetDDLJobList();
        Task<dynamic> GetDDLFolderNameMyDam();
        Task<dynamic> GetDDLJobNameMyDam();
        Task<dynamic> GetDDLClientNameMyDam();
        Task<dynamic> GetDDLCampaignNameMyDam();

        #endregion
    }
    public class DropdownService : IDropdownService
    {
        readonly DataContext context;
        private readonly IManageUserAppService userApp;

        /// <summary>
        /// And endpoint to manage MasterDivisionSelfservice
        /// </summary>
        /// <param name="context">Database context</param>
        public DropdownService(DataContext context, IManageUserAppService userApp)
        {
            this.context = context;
            this.userApp = userApp;
        }

        #region GLOBAL

        public async Task<dynamic> GetUom() => await context.Uom.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.unit_symbol, Value = x.Id }).ToListAsync();
        public async Task<dynamic> GetMenu() => await context.TmMenus.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.MenuName, Value = x.Id }).OrderBy(x => x.Text).ToListAsync();

        public async Task<dynamic> GetJobGrades() => await context.JobGrades.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.GradeName, Value = x.Id }).OrderBy(x => x.Text).ToListAsync();
        public async Task<dynamic> GetJobTitles() => await context.JobTitles.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.TitleName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> GetJobLevels() => await context.JobLevels.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.JobName, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetSubGroups()
        {
            var result = await (from x in context.BusinessUnits
                                join y in context.BusinessUnitTypes on x.BusinessUnitTypeId equals y.Id
                                where y.BusinessUnitLevel == BusinessUnitLevelCode.BusinessUnitSubgroup && !(x.IsDeleted ?? false) && !(x.Isfinance ?? false)
                                select new
                                {
                                    Text = x.UnitName,
                                    Value = x.Id
                                }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetDivision()
        {
            var result = await (from x in context.BusinessUnits
                                join y in context.BusinessUnitTypes on x.BusinessUnitTypeId equals y.Id
                                where y.BusinessUnitLevel == BusinessUnitLevelCode.Division && !(x.IsDeleted ?? false)
                                select new
                                {
                                    Text = x.UnitName,
                                    Value = x.Id
                                }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetDivisionHris()
        {
            var result = await (from x in (from a in context.BusinessUnits
                                           join b in context.BusinessUnitTypes on a.BusinessUnitTypeId equals b.Id
                                           where b.BusinessUnitLevel == BusinessUnitLevelCode.Division && !(a.IsDeleted ?? false) && a.Ishris == true
                                           select a)
                                join y in (from a in context.BusinessUnits
                                           join b in context.BusinessUnitTypes on a.BusinessUnitTypeId equals b.Id
                                           where b.BusinessUnitLevel == BusinessUnitLevelCode.BusinessUnitSubgroup && !(a.IsDeleted ?? false)
                                           select a) on x.ParentUnit equals y.Id
                                select new
                                {
                                    Text = x.UnitName,
                                    Value = x.Id
                                }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetApNumber()
        {
            //var result = await context.AccountPayables.Where(x => !(x.IsDeleted ?? false) && x.IsPayment == false)
            //                    .Select(x => new
            //                    {
            //                        Text = x.AccountPayableNumber,
            //                        Value = x.Id
            //                    }).ToListAsync();

            var result = await (from tb in context.AccountPayables
                                join inv in context.InvoiceReceivables on tb.InvoiceReceivableId equals inv.Id
                                join po in context.PurchaseOrders on inv.PoId equals po.Id
                                where !(tb.IsDeleted ?? false) && tb.IsPayment == false //&& string.IsNullOrEmpty(po.CashAdvanceId)
                                select new
                                {
                                    Text = tb.AccountPayableNumber,
                                    Value = tb.Id
                                }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetArNumber(string bankReceivedId)
        {
            var bankReceived = (from a in context.BankReceived
                                join b in context.BankReceivedDetail on a.Id equals b.BankReceivedId
                                where !(a.IsDeleted ?? false)
                                select new { a.Id, a.Status, b.AccountReceivebleNumber });

            var result = await context.AccountReceivables.Where(x => !(x.IsDeleted ?? false) && x.Status == ARStatus.Posted
                                && (!bankReceived.Any(y => y.AccountReceivebleNumber == x.AccountReceivableNumber && y.Status != PaymentRequestStatus.Complete && y.Id != bankReceivedId)))
                                .Select(x => new
                                {
                                    Text = x.AccountReceivableNumber,
                                    Value = x.Id
                                }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetDivisionFinance()
        {
            var result = await context.BusinessUnits.Where(x => !(x.IsDeleted ?? false) && (x.Isfinance ?? false))
                                .Select(x => new
                                {
                                    Text = x.UnitName,
                                    Value = x.Id
                                }).ToListAsync();
            return result;
        }


        public async Task<dynamic> GetDDLNonJobByBusinessUnitId()
        {
            var result = await (from x in context.MasterNonjob
                                where !(x.IsDeleted ?? false)
                                select new
                                {
                                    Text = x.name,
                                    Value = x.Id
                                }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetDivisions(string subGroupId)
        {
            var result = await (from x in context.BusinessUnits
                                join y in context.BusinessUnitTypes on x.BusinessUnitTypeId equals y.Id
                                where y.BusinessUnitLevel == BusinessUnitLevelCode.Division && !(x.IsDeleted ?? false) && x.ParentUnit == subGroupId && !(x.Isfinance ?? false)
                                select new
                                {
                                    Text = x.UnitName,
                                    Value = x.Id
                                }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetDepartments()
        {
            var result = await (from x in context.BusinessUnits
                                join y in context.BusinessUnitTypes on x.BusinessUnitTypeId equals y.Id
                                where y.BusinessUnitLevel == BusinessUnitLevelCode.Department && !(x.IsDeleted ?? false)
                                select new
                                {
                                    Text = x.UnitName,
                                    Value = x.Id
                                }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetUsers()
        {

            var result = await (from x in context.TmUserApps
                                where !(x.IsDeleted ?? false)
                                select new
                                {
                                    Text = x.Email,
                                    Value = x.Id
                                }).ToListAsync();
            return result;
        }

        //public async Task<dynamic> GetMasterDepartments()
        //{
        //    var result = await context.MasterDepartments.Where(x => !(x.IsDeleted ?? false)).
        //        Select(x => new
        //        {
        //            Text = x.Name,
        //            Value = x.Id
        //        }).ToListAsync();
        //    return result;
        //}

        public async Task<dynamic> GetDepartments(string divisionId)
        {
            var result = await (from x in context.BusinessUnits
                                join y in context.BusinessUnitTypes on x.BusinessUnitTypeId equals y.Id
                                where y.BusinessUnitLevel == BusinessUnitLevelCode.Department && x.ParentUnit == divisionId && !(x.IsDeleted ?? false)
                                select new
                                {
                                    Text = x.UnitName,
                                    Value = x.Id
                                }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetDepartmentById(string id)
        {
            var result = await (from x in context.BusinessUnits
                                where x.Ishris == true && !(x.IsDeleted ?? false)
                                select new
                                {
                                    Text = x.UnitName,
                                    Value = x.Id
                                }).ToListAsync();

            if (id != "noselected")
            {
                result = result.Where(x => x.Value == id).ToList();
            }
            return result;
        }

        public async Task<dynamic> GetDDLJobGradesWithLevel(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                var result = await context.JobGrades.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.GradeName, Value = x.Id }).OrderBy(x => x.Text).ToListAsync();
                return result;
            }
            else
            {
                var gradeLevel = await (context.JobGrades.Where(x => x.Id == id && !(x.IsDeleted ?? false)).Select(x => x.Joblevelid).FirstOrDefaultAsync());
                var gradeLevelInt = Convert.ToInt32(gradeLevel);
                var result = await (from x in context.JobGrades
                                    where Convert.ToInt32(x.Joblevelid) < gradeLevelInt && !(x.IsDeleted ?? false)
                                    orderby x.GradeName
                                    select new
                                    {
                                        Text = x.GradeName,
                                        Value = x.Id
                                    }).ToListAsync();
                return result;
            }

        }

        public async Task<dynamic> GetLocations() => await context.Locations.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.LocationName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> GetFamily(string employeeId) => await context.Familys.Where(x => x.EmployeeBasicInfoId == employeeId).Select(x => new { Text = x.Name, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetItemType()
        {
            return await context.ItemType.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.TypeName, Value = x.Id }).ToListAsync();
        }
        public async Task<dynamic> GetLegalEntity()
        {
            //ini jangan asal main taro all legal entity karena bakalan berpengaruh ke semua transksi. jika memang ada all legal entity di latakin di master data legal entiy jgn di hardcode begini. transaksi jadi kacau semua klo di hardcode. 
            //klo memang hanya kebutuhanny untuk transkasi tertentu saja buat function sendiri jgn function ini yg di rubah karena banyak transaksi yg pakai function ini.
            //List<dynamic> list = new List<dynamic>();
            //var allData = new
            //{
            //    Text = "All",
            //    Value = "All"
            //};
            //list.Add(allData);
            var result = await (from x in context.LegalEntity
                                where !(x.IsDeleted ?? false)
                                select new
                                {
                                    Text = x.LegalEntityName,
                                    Value = x.Id
                                }).ToListAsync();
            //list.AddRange(result);

            return result;
        }
        public async Task<dynamic> GetPaymentType()
        {
            var result = await (from x in context.PaymentType
                                where !(x.IsDeleted ?? false)
                                select new
                                {
                                    Text = x.TypeName,
                                    Value = x.Id
                                }).ToListAsync();
            return result;
        }
        public async Task<dynamic> GetItemCode(string ItemTypeId)
        {
            return await context.Item.Where(x => !(x.IsDeleted ?? false) && x.ItemTypeId == ItemTypeId).Select(x => new { Text = x.ItemCode, Value = x.Id }).ToListAsync();
        }
        public async Task<dynamic> GetItemInventoryAndFixedAsset(string itemTypeId)
        {
            if (itemTypeId == ItemTypeId.Inventory)
            {
                return await context.Inventory.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Value = x.Id, Text = x.SKUCode + " - " + x.InventoryName }).ToListAsync();
            }
            else if (itemTypeId == ItemTypeId.FixedAsset)
            {
                return await context.MasterBudgetCodes.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Value = x.Id, Text = x.BudgetCode + " - " + x.BudgetName }).ToListAsync();
            }
            return null;
        }

        public async Task<dynamic> GetGroups()
        {
            return await (from a in context.BusinessUnits
                          join b in context.BusinessUnitTypes on a.BusinessUnitTypeId equals b.Id
                          where b.BusinessUnitLevel == BusinessUnitLevelCode.HoldingGroup && !(a.IsDeleted ?? false)
                          select new
                          {
                              Text = a.UnitName,
                              Value = a.Id
                          }).ToListAsync();
        }
        public async Task<dynamic> GetSubGroupsHris(string GroupId)
        {
            return await (from a in context.BusinessUnits
                          join b in context.BusinessUnitTypes on a.BusinessUnitTypeId equals b.Id
                          where b.BusinessUnitLevel == BusinessUnitLevelCode.BusinessUnitSubgroup && !(a.IsDeleted ?? false)
                          select new
                          {
                              Text = a.UnitName,
                              Value = a.Id
                          }).ToListAsync();
        }
        public async Task<dynamic> GetDivisionsHris(string subGroupId)
        {
            var result = await (from x in (from a in context.BusinessUnits
                                           join b in context.BusinessUnitTypes on a.BusinessUnitTypeId equals b.Id
                                           where b.BusinessUnitLevel == BusinessUnitLevelCode.Division && !(a.IsDeleted ?? false) && a.Ishris == true
                                           select a)
                                join y in (from a in context.BusinessUnits
                                           join b in context.BusinessUnitTypes on a.BusinessUnitTypeId equals b.Id
                                           where b.BusinessUnitLevel == BusinessUnitLevelCode.BusinessUnitSubgroup && !(a.IsDeleted ?? false)
                                           select a) on x.ParentUnit equals y.Id
                                where x.ParentUnit == subGroupId
                                select new
                                {
                                    Text = x.UnitName,
                                    Value = x.Id
                                }).ToListAsync();
            return result;
        }
        public async Task<dynamic> GetDepartmentsHris(string divisionId)
        {
            return await (from a in context.BusinessUnits
                          join b in context.BusinessUnitTypes on a.BusinessUnitTypeId equals b.Id
                          where b.BusinessUnitLevel == BusinessUnitLevelCode.Department && !(a.IsDeleted ?? false)
                          select new
                          {
                              Text = a.UnitName,
                              Value = a.Id
                          }).ToListAsync();
        }

        public async Task<dynamic> GetWorkingHour()
        {
            return await context.MasterWorkingTimes.Where(x => x.IsDeleted != true).Select(x => new { Text = x.Name, Value = x.Id }).ToListAsync();
        }
        #endregion

        #region HRIS
        //public async Task<dynamic> GetMasterDivisionSelfservice() => await context.MasterDivisionSelfservices.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.BusinessUnitName, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetMasterDivisionSelfservice()
        {
            //var result = await (from a in context.BusinessUnits
            //                    join b in context.BusinessUnitTypes on a.BusinessUnitTypeId equals b.Id
            //                    where !(a.IsDeleted ?? false) && a.Ishris == true && b.BusinessUnitLevel == 800
            //                    select new { Text = a.UnitName, Value = a.BusinessUnitTypeId }).ToListAsync();
            var result = await (from a in context.MasterDivisionSelfservices
                                join b in context.BusinessUnits on a.BusinessUnitId equals b.Id
                                where !(a.IsDeleted ?? false)
                                select new { Text = b.UnitName, Value = a.Id }).ToListAsync();
            return result;
        }



        public async Task<dynamic> GetEmployeeBasicInfo()
        {
            return await context.EmployeeBasicInfos.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.NameEmployee, Value = x.Id }).ToListAsync();
        }

        public async Task<dynamic> GetEmployeeBasicInfoMasterCounterPart(string id)
        {
            var result = await (from a in context.LegalDocumentMasterCounterparts
                                join b in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals b.Id
                                where !(a.IsDeleted ?? false) && a.LegalCategoryId == id
                                select new
                                {
                                    Text = b.NameEmployee,
                                    Value = b.Id
                                }).ToListAsync();

            return result;
        }


        public async Task<dynamic> GetEmployeeBasicInfoByName(string name)
        {
            var result = await context.EmployeeBasicInfos
                                .Where(x => !(x.IsDeleted ?? false) && x.NameEmployee.Trim().Replace(" ", "").ToLower().Contains(name.Trim().Replace(" ", "").ToLower()))
                                .Select(x => new { Text = x.NameEmployee, Value = x.Id }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetDDLPayrollComponent(string DeclarationTypeId)
        {
            List<dynamic> list = new List<dynamic>();
            var bpjsAdd = await context.BpjsComponents.Where(x => !(x.IsDeleted ?? false) && x.Type == 1).Select(x => new { Text = x.SubComponentName, Value = x.Id }).ToListAsync();
            var allowance = await context.AllowanceComponents.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => new { Text = x.SubComponentName, Value = x.Id }).ToListAsync();
            var bpjsDed = await context.BpjsComponents.Where(x => !(x.IsDeleted ?? false) && x.Type == 2).Select(x => new { Text = x.SubComponentName, Value = x.Id }).ToListAsync();
            var deduction = await context.DeductionComponents.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => new { Text = x.SubComponentName, Value = x.Id }).ToListAsync();

            if (DeclarationTypeId == "Additional")
            {
                //list.AddRange(bpjsAdd);
                list.AddRange(allowance);
            }
            else
            {
                //list.AddRange(bpjsDed);
                list.AddRange(deduction);
            }

            return list;
        }

        public async Task<dynamic> GetTerminationEmployeeBasicInfoByName(string name, string terminationType)
        {
            var employeeId = context.GetEmployeeId();
            var status = new int[] { StatusTransaction.Draft, StatusTransaction.Rejected };
            var terminationTypes = new string[] { TerminationType.ProlongedIllness, TerminationType.Sanction };
            var isContract = terminationType == TerminationType.EndOfProbation || terminationType == TerminationType.EndOfContract;

            var employeeStatus = "";
            if (terminationType == TerminationType.EndOfProbation) employeeStatus = EmployeeStatus.Probation;
            else if (terminationType == TerminationType.EndOfContract) employeeStatus = EmployeeStatus.Contract;

            var group = await userApp.GetByUserLogin();
            var HCAdmin = group.Where(x => x.GroupId == GroupAcess.HCAdmin).Count() > 0;
            var terminationEmployeeId = context.Terminations.Where(x => !status.Any(y => y == x.Status) && !terminationTypes.Any(y => y == x.TerminationType)).Select(x => x.EmployeeBasicInfoId).ToList();

            if (HCAdmin)
            {
                var result = await context.EmployeeBasicInfos.Where(x => !(x.IsDeleted ?? false) &&
                                    x.NameEmployee.Trim().Replace(" ", "").ToLower().Contains(name.Trim().Replace(" ", "").ToLower()) &&
                                    !terminationEmployeeId.Any(y => y == x.Id) &&
                                    (!isContract ? true : x.EmployementStatusId == employeeStatus))
                                    .Select(x => new { Text = x.NameEmployee, Value = x.Id }).Take(50).ToListAsync();
                //var result = await context.EmployeeBasicInfos.Where(x => !(x.IsDeleted ?? false) && 
                //                    x.NameEmployee.Trim().Replace(" ", "").ToLower().Contains(name.Trim().Replace(" ", "").ToLower())).Select(x => new { Text = x.NameEmployee, Value = x.Id }).Take(50).ToListAsync();
                return result;
            }
            else
            {
                var parentId = (from b in context.EmployeeBasicInfos
                                join c in context.BusinessUnitJobLevels on b.BusinessUnitJobLevelId equals c.Id
                                where b.Id == employeeId
                                select c.Id).FirstOrDefault();

                var result = (from b in context.EmployeeBasicInfos
                              join c in context.BusinessUnitJobLevels on b.BusinessUnitJobLevelId equals c.Id
                              //where b.NameEmployee.Trim().Replace(" ", "").ToLower().Contains(name.Trim().Replace(" ", "").ToLower()) &&
                              //!terminationEmployeeId.Any(y => y == b.Id) && c.ParentId.Trim() == parentId &&
                              //(!isContract ? true : b.EmployementStatusId == employeeStatus)
                              where b.NameEmployee.Trim().Replace(" ", "").ToLower().Contains(name.Trim().Replace(" ", "").ToLower()) &&
                              !terminationEmployeeId.Any(y => y == b.Id) &&
                              (!isContract ? true : b.EmployementStatusId == employeeStatus)
                              //where b.NameEmployee.Trim().Replace(" ", "").ToLower().Contains(name.Trim().Replace(" ", "").ToLower()) && !(b.IsDeleted ?? false)
                              select new { Text = b.NameEmployee, Value = b.Id }).Take(50).ToList();
                return result;
            }
        }

        public async Task<dynamic> GetEmployeeBasicInfoByHead(string name)
        {
            var employeeId = context.GetEmployeeId();
            var group = await userApp.GetByUserLogin();
            var HCAdmin = group.Where(x => x.GroupId == GroupAcess.HCAdmin).Count() > 0;

            if (HCAdmin)
            {
                return await context.EmployeeBasicInfos
                                    .Where(x => !(x.IsDeleted ?? false) && x.NameEmployee.Trim().Replace(" ", "").ToLower().Contains(name.Trim().Replace(" ", "").ToLower()))
                                    .Select(x => new { Text = x.NameEmployee, Value = x.Id }).ToListAsync();
            }
            else
            {
                var parentId = (from b in context.EmployeeBasicInfos
                                join c in context.BusinessUnitJobLevels on b.BusinessUnitJobLevelId equals c.Id
                                where b.Id == employeeId
                                select c.Id).FirstOrDefault();

                var result = (from b in context.EmployeeBasicInfos
                              join c in context.BusinessUnitJobLevels on b.BusinessUnitJobLevelId equals c.Id
                              where b.NameEmployee.Trim().Replace(" ", "").ToLower().Contains(name.Trim().Replace(" ", "").ToLower()) && c.ParentId.Trim() == parentId
                              select new { Text = b.NameEmployee, Value = b.Id }).Take(50).ToList();
                return result;
            }
        }

        public async Task<dynamic> GetEmployeeResignationType() => await GetTerminationType(new string[] { TerminationType.Resignation });
        public async Task<dynamic> GetEmployeeTerminationType() => await GetTerminationType(new string[] { TerminationType.EndOfContract, TerminationType.EndOfProbation });
        public async Task<dynamic> GetEmployeeDeathType() => await GetTerminationType(new string[] { TerminationType.Death, TerminationType.ProlongedIllness, TerminationType.Sanction, TerminationType.Retired });
        private async Task<dynamic> GetTerminationType(string[] id) => await context.TmTerminationTypes.Where(x => !(x.IsDeleted ?? false) && id.Any(z => z == x.Id)).Select(x => new { Text = x.TypeName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> FaultCategoryList() => await context.FaultCategorys.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.FaultName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> ServiceNameList() => await context.MasterMappingRequests.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.ServiceName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> CategoryExitInterviewList() => await context.CategoryExitInterviews.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.Name, Value = x.Id }).ToListAsync();
        public async Task<dynamic> LeaveRequestTypeList() => await context.LeaveRequestTypes.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.Name, Value = x.Id }).ToListAsync();

        public async Task<dynamic> leaverequesttypelistemployeeid(string employeeid)
        {
            //var list = await context.LeaveRequestTypes.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.Name, Value = x.Id }).ToListAsync();
            //var dateBirth = await context.EmployeeBasicInfos.Where(x => x.Id == employeeid).Select(x => x.DateBirth).FirstOrDefaultAsync();
            //var birthday = new DateTime(DateTime.Now.Year, dateBirth.Value.Month, dateBirth.Value.Day);

            //var dateBirthMin1Week = birthday.AddDays(-7);
            //var dataBirthMaxExp = birthday.AddMonths(1);
            //if (DateTime.Now.Date >= dateBirthMin1Week && DateTime.Now.Date <= dataBirthMaxExp)
            //{
            //    return list;
            //}
            //else
            //{
            //    list = list.Where(x => x.Value != "5622a5db-23fd-43fb-8ca0-3fbe8eda2a34" && x.Value != "5c771383-28d4-4359-adc0-c6d9a512a5db").ToList();
            //}
            //return list;
            List<SelectListItem> model = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(employeeid))
            {
                var paramType = new SqlParameter[] {
                                new SqlParameter("@RequestBy", employeeid)
                            };

                var que = string.Format("EXEC uspGetDropdownListLeaveRequest '{0}'", employeeid);
                var dt = await context.GetDataTable(que, null);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SelectListItem m = new SelectListItem();

                        if (!string.IsNullOrEmpty(dt.Rows[i]["Value"].ToString()))
                        {
                            m.Value = dt.Rows[i]["Value"].ToString();
                        }

                        if (!string.IsNullOrEmpty(dt.Rows[i]["Text"].ToString()))
                        {
                            m.Text = dt.Rows[i]["Text"].ToString();
                        }

                       
                        model.Add(m);
                    }

                }
            }

            return model;
        }

        public async Task<dynamic> LoanCategoryList() => await context.LoanCategorys.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.Name, Value = x.Id }).ToListAsync();
        public async Task<dynamic> LoanCategoryCorporateList() => await context.LoanCategorys.Where(x => !(x.IsDeleted ?? false) && x.is_Corporate_Loan == true).Select(x => new { Text = x.Name, Value = x.Id }).ToListAsync();
        public async Task<dynamic> SalaryComponentList() => await context.SalaryMainComponents.Where(x => !(x.IsDeleted ?? false) && !(x.IsDefault ?? false)).Select(x => new { Text = x.SubComponentName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> AllowanceComponentList(string gradeId)
        {
            //List<dynamic> list = new List<dynamic>();
            //if (string.IsNullOrEmpty(gradeId))
            //{
            //    var allowance = await context.AllowanceComponents.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.SubComponentName, Value = x.Id }).ToListAsync();

            //    list.AddRange(allowance);

            //}
            //else
            //{
            //    var allowance = await context.AllowanceComponents.Where(x => !(x.IsDeleted ?? false) && x.JobGradeId == gradeId).Select(x => new { Text = x.SubComponentName, Value = x.Id }).ToListAsync();
            //    list.AddRange(allowance);
            //}
            //var bpjsAdd = await context.BpjsComponents.Where(x => !(x.IsDeleted ?? false) && x.Type == 1).Select(x => new { Text = x.SubComponentName, value = x.Id }).ToListAsync();
            //list.AddRange(bpjsAdd);
            //return list;
            return await context.AllowanceComponents.Where(x => !(x.IsDeleted ?? false) && !(x.IsDefault ?? false)).Select(x => new { Text = x.SubComponentName, Value = x.Id }).ToListAsync();

        }
        public async Task<dynamic> DeductionComponentList() => await context.DeductionComponents.Where(x => !(x.IsDeleted ?? false) && !(x.IsDefault ?? false)).Select(x => new { Text = x.SubComponentName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> BpjsComponentAdditionalList() => await context.BpjsComponents.Where(x => !(x.IsDeleted ?? false) && x.Type == BPJSType.Additional).Select(x => new { Text = x.SubComponentName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> BpjsComponentDeductionList() => await context.BpjsComponents.Where(x => !(x.IsDeleted ?? false) && x.Type == BPJSType.Deduction).Select(x => new { Text = x.SubComponentName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> InsuranceCompanyList() => await context.InsuranceCompanys.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.NameInsurance, Value = x.Id }).ToListAsync();
        public async Task<dynamic> PdrCategoryList() => await context.PdrCategorys.Where(x => !(x.IsDeleted ?? false)).OrderBy(x => x.No).Select(x => new { Text = x.Name, Value = x.Id }).ToListAsync();
        public async Task<dynamic> PdrGradeList() => await context.PdrGrades.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.Result, Value = x.Id }).ToListAsync();
        public async Task<dynamic> LegalCategoryList() => await context.LegalCategorys.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.Category, Value = x.Id }).ToListAsync();
        public async Task<dynamic> GetDDLCorporateObjective() => await context.BscCorporateObjectives.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.BscName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> GetDDLCorporateObjectiveName(string bscCategory) => await context.BscCorporateObjectives.Where(x => !(x.IsDeleted ?? false) && x.BscCategory.Trim() == bscCategory.Trim()).Select(x => new { Text = x.BscName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> GetDDLObjectiveSetting() => await context.PdrObjectiveSettings.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.ObjectiveItem, Value = x.Id }).ToListAsync();
        public async Task<dynamic> GetDDLEmployeePayrollGroup() => await context.EmployeePayrollGroups.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.GroupName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> GetDDLPTKPSetting() => await context.PtkpSettings.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.Name, Value = x.Id }).ToListAsync();


        public async Task<dynamic> GetDDLEmployeeLegalDoc(string category, string legalCategoryId, string text)
        {
            if (category == CategoryLegalDoc.Employee)
            {
                return await context.EmployeeBasicInfos.Where(x => !(x.IsDeleted ?? false) && x.NameEmployee.Contains(text)).Select(x => new { Text = x.NameEmployee, Value = x.Id }).ToListAsync();
            }
            else
            {

                var result = await (from a in context.EmployeeBasicInfos
                                    join b in context.LegalDocumentMasterCounterparts on a.Id equals b.EmployeeBasicInfoId
                                    where (a.IsDeleted ?? false) == false && b.LegalCategoryId == legalCategoryId && a.NameEmployee.Contains(text)
                                    select new { Text = a.NameEmployee, Value = a.Id }
                                     ).ToListAsync();
                return result;
            }
        }

        public async Task<dynamic> GetDDLLegalDoc(string category, string CounterpartId)
        {
            return await (from a in context.LegalDocuments
                          join b in context.LegalDocumentCounterparts on a.Id equals b.LegalDocumentId
                          where (a.IsDeleted ?? false) == false && (b.IsDeleted ?? false) == false &&
                          a.Category == category && b.CounterpartId == CounterpartId
                          select new { Text = a.Label, Value = a.Id }).ToListAsync();
        }

        public async Task<dynamic> GetDDLTrainingSetting()
        {
            return await context.Training.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.Name, Value = x.Id }).ToListAsync();
        }
        public async Task<dynamic> GetDDLTrainingCategory()
        {
            return await context.TrainingCategory.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.Category, Value = x.Category }).ToListAsync();
        }

        public async Task<dynamic> GetDDLTrainingSubject()
        {
            return await context.TrainingSubject.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.Subject, Value = x.Subject }).ToListAsync();
        }

        public async Task<dynamic> GetDDLTrainingType()
        {
            return await context.TrainingType.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.Type, Value = x.Type }).ToListAsync();
        }

        public async Task<dynamic> GetDDLReligion() => await context.Religions.Where(x => x.IsDeleted != true).Select(x => new { Text = x.ReligionName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> GetDDLMarriedStatus() => await context.MarriedStatuss.Where(x => x.IsDeleted != true).Select(x => new { Text = x.MarriedName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> GetDDLEducationLevel() => await context.EducationLevels.Where(x => x.IsDeleted != true).Select(x => new { Text = x.EducationName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> GetDDLFamilyStatus() => await context.FamilyStatuss.Where(x => x.IsDeleted != true).Select(x => new { Text = x.FamilyStatusName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> GetDDLInstitution() => await context.Institutions.Where(x => x.IsDeleted != true).Select(x => new { Text = x.InstitutionName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> GetDDLEmployementStatus() => await context.EmployementStatuss.Where(x => x.IsDeleted != true).Select(x => new { Text = x.EmploymentStatusName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> GetDDLNationality() => await context.Nationalitys.Where(x => x.IsDeleted != true).Select(x => new { Text = x.NationalityName, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetDDLEmployeeData() => await context.EmployeeBasicInfos.Where(x => x.IsDeleted != true).Select(x => new { Text = x.NameEmployee, Value = x.Id }).ToListAsync();

        //  GetDDLEmployee

        public async Task<dynamic> GetDDLEmployeeChecklistCategory() => await context.EmployeeChecklistCategorys.Where(x => x.IsDeleted != true).Select(x => new { Text = x.ChecklistCategory, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetMasterTrainingRequest() => await context.MasterTrainingRequests.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.RequestName, Value = x.Id }).ToListAsync();


        public async Task<dynamic> GetTrainingCompleted()
        {
            return await (from a in context.TrainingRequisition
                          join b in context.Training on a.TrainingId equals b.Id
                          where a.Status == "5"
                          select new { Text = b.Name, Value = a.Id }).ToListAsync();
        }



        #endregion

        #region Media

        public async Task<dynamic> GetMediaType()
        {
            var result = await context.MediaTypes.Select(x => new { Text = x.MediaTypeName, Value = x.Id }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetMediaPlanRequestType()
        {
            var result = await context.TmMediaPlanRequestTypes.Select(x => new { Text = x.RequestTypeName, Value = x.Id }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetMediaPlanNo()
        {
            var result = await context.TrMediaPlanTVs
                .Where(w => w.TmMediaPlanStatusId == MediaPlanStatusName.Approved)
                .Select(x => new { Text = x.MediaPlanNo, Value = x.Id }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetMediaOrderStatus()
        {
            var result = await context.TmMediaOrderStatuses
                .Select(x => new { Text = x.MediaOrderStatusName, Value = x.Id }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetMediaPlanStatus()
        {
            var result = await context.TmMediaPlanStatuses
                .Select(x => new { Text = x.MediaPlanStatusName, Value = x.Id }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetProgramCategory()
        {
            var result = await context.TmProgramCategoryTypes
               .Select(x => new { Text = x.ProgramCategoryName, Value = x.Id }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetProgramPosition()
        {
            var result = await context.TmProgramPositions
               .Select(x => new { Text = x.ProgramPositionName, Value = x.Id }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetProgramType()
        {
            var result = await context.TmProgramTypes
               .Select(x => new { Text = x.ProgramTypeName, Value = x.Id }).ToListAsync();
            return result;
        }
        public async Task<dynamic> GetCascadeFromJob(string jobid, string caseData)
        {

            if (caseData.ToLower() == "mainservicecategory")
            {
                return await (from a in context.JobDetail
                              join b in context.MainServiceCategorys on a.MainServiceCategory equals b.Id
                              where !(a.IsDeleted ?? false) && a.Id == jobid
                              select new { Text = b.Name, Value = b.Id }).ToListAsync();
            }
            else if (caseData.ToLower() == "division")
            {
                return await (from a in context.JobDetail
                              join b in context.BusinessUnits on a.BusinessUnitDivisionId equals b.Id
                              where !(a.IsDeleted ?? false) && a.Id == jobid
                              select new { Text = b.UnitName, Value = b.Id }).ToListAsync();
            }
            else if (caseData.ToLower() == "legalentity")
            {
                return await (from a in context.JobDetail
                              join b in context.LegalEntity on a.LegalEntityId equals b.Id
                              where !(a.IsDeleted ?? false) && a.Id == jobid
                              select new { Text = b.LegalEntityName, Value = b.Id }).ToListAsync();
            }
            else if (caseData.ToLower() == "affiliation")
            {
                return await (from a in context.JobDetail
                              join b in context.Affiliations on a.AffiliationId equals b.Id
                              where !(a.IsDeleted ?? false) && a.Id == jobid
                              select new { Text = b.AffiliationName, Value = b.Id }).ToListAsync();
            }
            else if (caseData.ToLower() == "clientname")
            {
                return await (from a in context.JobDetail
                              join b in context.Companys on a.CompanyId equals b.Id
                              where !(a.IsDeleted ?? false) && a.Id == jobid
                              select new { Text = b.CompanyName, Value = b.Id }).ToListAsync();
            }
            else if (caseData.ToLower() == "clientbrand")
            {
                return await (from a in context.JobDetail
                              join b in context.CompanyBrands on a.BrandId equals b.Id
                              where !(a.IsDeleted ?? false) && a.Id == jobid
                              select new { Text = b.BrandName, Value = b.Id }).ToListAsync();
            }
            return new List<SelectListItem>();

        }

        public async Task<dynamic> GetBrandASF()
        {
            var result = await context.CompanyBrands.Where(a => !string.IsNullOrEmpty(a.ASFValue.ToString()))
               .Select(x => new { Text = x.ASFValue, Value = x.ASFValue }).Distinct().ToListAsync();
            return result;
        }
        #endregion

        #region Finance
        #region Master Data
        public async Task<dynamic> OrganizationList() => await context.Organizations.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.OrganizationName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> GetAffiliation() => await context.Affiliations.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.AffiliationName, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetFrom() => await context.GetFroms.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.GetFromName, Value = x.Id }).ToListAsync();

        public async Task<dynamic> ChartofaccountReportList() => await context.ChartofaccountReports.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.ReportName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> GetCOACodeByNameLv1() => await context.ChartOfAccounts.Where(coa => !(coa.IsDeleted ?? false) && coa.Level == "1").OrderBy(x => x.CodeRec).Select(x => new { Text = x.Name1, Value = x.CodeRec }).ToListAsync();
        public async Task<dynamic> GetCOACodeByNameLv2(string CodeRec) => await context.ChartOfAccounts.Where(coa => !(coa.IsDeleted ?? false) && coa.Level == "2" && CoaCodeLevel(CodeRec, coa.CodeRec, 2)).OrderBy(x => x.CodeRec).GroupBy(x => new { x.Name1, x.CodeRec }).Select(x => new { Text = x.Key.Name1, Value = x.Key.CodeRec }).ToListAsync();
        public async Task<dynamic> GetCOACodeByNameLv3(string CodeRec) => await context.ChartOfAccounts.Where(coa => !(coa.IsDeleted ?? false) && coa.Level == "3" && CoaCodeLevel(CodeRec, coa.CodeRec, 3)).OrderBy(x => x.CodeRec).GroupBy(x => new { x.Name1, x.CodeRec }).Select(x => new { Text = x.Key.Name1, Value = x.Key.CodeRec }).ToListAsync();
        public async Task<dynamic> GetCOACodeByNameLv4(string CodeRec) => await context.ChartOfAccounts.Where(coa => !(coa.IsDeleted ?? false) && coa.Level == "4" && CoaCodeLevel(CodeRec, coa.CodeRec, 4)).OrderBy(x => x.CodeRec).GroupBy(x => new { x.Name1, x.CodeRec }).Select(x => new { Text = x.Key.Name1, Value = x.Key.CodeRec }).ToListAsync();
        public async Task<dynamic> GetCOACodeByNameLv5(string CodeRec) => await context.ChartOfAccounts.Where(coa => !(coa.IsDeleted ?? false) && coa.Level == "5" && CoaCodeLevel(CodeRec, coa.CodeRec, 5)).OrderBy(x => x.CodeRec).GroupBy(x => new { x.Name1, x.CodeRec }).Select(x => new { Text = x.Key.Name1, Value = x.Key.CodeRec }).ToListAsync();
        public async Task<dynamic> CountryList() => await context.Countrys.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.CountryName, Value = x.Id }).OrderBy(x => x.Text).ToListAsync();
        public async Task<dynamic> ProvincesList(string countryId) => await context.Provinces.Where(x => !(x.IsDeleted ?? false) && x.CountryId == countryId).Select(x => new { Text = x.ProvinceName, Value = x.Id }).OrderBy(x => x.Text).ToListAsync();
        public async Task<dynamic> CityList(string ProvinceId) => await context.Citys.Where(x => !(x.IsDeleted ?? false) && x.ProvinceId == ProvinceId).Select(x => new { Text = x.CityName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> GetClientlist() => await context.Companys.Where(x => !(x.IsDeleted ?? false) && (x.IsClient ?? false)).Select(x => new { Text = x.CompanyName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> GetVendorList() => await context.Companys.Where(x => !(x.IsDeleted ?? false) && (x.IsVendor ?? false)).Select(x => new { Text = x.CompanyName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> GetMasterConditionProcurement() => await context.MasterConditionProcurement.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.ConditionName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> CostSharingAllocationList() => await context.CostSharingAllocations.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.CostSharingAllocationName, Value = x.Id }).ToListAsync();
        private bool CoaCodeLevel(string codeRec, string codeRecNew, int level)
        {
            var result = "";
            var resultNew = "";
            var array = codeRec.Split(".");
            var arrayNew = codeRecNew.Split(".");
            if (array.First() == arrayNew.First())
            {
                for (int i = 1; i <= array.Length; i++)
                {
                    if (i >= level) break;
                    result += array[i - 1];
                    resultNew += arrayNew[i - 1];
                };
            }
            else
            {
                return false;
            }
            return (result == resultNew);
        }
        #endregion

        public async Task<dynamic> GetFixedAssetByLocation(string locationId)
        {
            return await context.FixedAsset.Where(x => !(x.IsDeleted ?? false) && x.LocationId == locationId && x.Condition != 5).Select(x => new { Text = x.FixedAssetName, Value = x.Id }).ToListAsync();
        }

        public async Task<dynamic> GetCompany()
        {
            var result = await context.Companys.Where(x => !(x.IsDeleted ?? false) && x.IsClient == true).Select(x => new { Text = x.CompanyName, Value = x.Id }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetCompanys()
        {
            var result = await context.Companys.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.CompanyName, Value = x.Id }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetClients(string id)
        {
            var result = await context.Companys.Where(x => !(x.IsDeleted ?? false) && (x.IsClient ?? false) && string.IsNullOrEmpty(id) ? true : x.Id == id).Select(x => new { Text = x.CompanyName, Value = x.Id }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetVendors(string id)
        {
            var result = await context.Companys.Where(x => !(x.IsDeleted ?? false) && (x.IsVendor ?? false) && string.IsNullOrEmpty(id) ? true : x.Id == id).Select(x => new { Text = x.CompanyName, Value = x.Id }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetRFQVendors(string rfqId, string vendorId)
        {
            var vendorDetail = await context.RequestForQuotationVendor.Where(y => !(y.IsDeleted ?? false) && y.RequestForQuotationId == rfqId && y.VendorId != vendorId).ToListAsync();
            var result = await context.Companys.Where(x => !(x.IsDeleted ?? false) && (x.IsVendor ?? false) && !vendorDetail.Any(y => y.VendorId == x.Id)).Select(x => new { Text = x.CompanyName, Value = x.Id }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetBrands()
        {
            var result = await context.CompanyBrands.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.BrandName, Value = x.Id }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetCurrencies()
        {
            var result = await context.Currencies.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.CurrencyName, Value = x.Id, isdefault = x.IsDefault }).OrderByDescending(x => x.isdefault).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetCurrencyParamsAndDefault(string currencyId)
        {
            var currencyParams = await context.Currencies.Where(x => x.Id == currencyId).FirstOrDefaultAsync();
            var currencyDefault = await context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).FirstOrDefaultAsync();
            var result = new List<SelectListItem>()
            {
                new SelectListItem() { Text = currencyParams.CurrencyName, Value = currencyParams.Id },
                new SelectListItem() { Text = currencyDefault.CurrencyName, Value = currencyDefault.Id }
            };
            return result;
        }
        public async Task<dynamic> GetEfaktur(string legalEntityId, string invoiceDate, string vatNo)
        {
            DateTime? date = null;
            if (!string.IsNullOrEmpty(invoiceDate) || invoiceDate != "0") date = Convert.ToDateTime(invoiceDate);
            var result = await (from a in context.EFaktur
                                join b in context.EFakturDetail on a.Id equals b.EFakturId
                                where !(a.IsDeleted ?? false) && !(b.IsDeleted ?? false)
                                 && a.LegalEntityId == legalEntityId && date >= a.StartDate && date <= a.EndDate
                                 && (string.IsNullOrEmpty(vatNo) ? b.IsUsed == false : ((b.vat_no == vatNo && b.IsUsed == true) || b.IsUsed == false))
                                select new
                                {
                                    Text = b.vat_no,
                                    Value = b.vat_no
                                }).OrderBy(x => x.Value).ToListAsync();
            return result;
        }


        public async Task<dynamic> GetCurrencyIsNoDefault()
        {
            var result = await context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault != true).Select(x => new { Text = x.CurrencyName, Value = x.Id }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetDetailStatus()
        {
            var result = await context.InventoryDetailStatus.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => new { Text = x.statusName, Value = x.getForm }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetPurchaseOrderByClientId(string clietnId, string invoiceId)
        {
            var result = await context.PurchaseOrders.Where(x => x.CompanyId == clietnId &&
                !(x.IsDeleted ?? false) &&
                (string.IsNullOrEmpty(invoiceId) ?
                (!(context.InvoiceReceivables.Any(z => !(z.IsDeleted ?? false) && z.PoId == x.Id && z.InvoiceStatusId != InvoiceReceivedStatus.Posted)) &&
                 !(context.InvoiceReceivables.Where(z => !(z.IsDeleted ?? false) && z.PoId == x.Id).GroupBy(z => z.PoId).Select(z => new { PoId = z.Key, TotalCostVendor = z.Sum(y => y.TotalCostVendor) }).Any(z => z.PoId == x.Id && z.TotalCostVendor == x.GrandTotal)) && x.Status == PurchaseOrderStatus.Approved && (x.Status != PurchaseOrderStatus.Closed)) :
                  context.InvoiceReceivables.Any(z => !(z.IsDeleted ?? false) && z.Id == invoiceId && z.PoId == x.Id)
                ))
                .Select(x => new { Text = x.PurchaseOrderNumber, Value = x.Id }).ToListAsync();
            return result;
        }

        //Invoice per termin
        //public async Task<dynamic> GetPurchaseOrderByClientId(string clietnId, string invoiceId)
        //{
        //    var poId = await context.InvoiceReceivables.Where(x => x.Id == invoiceId).Select(x => x.PoId).FirstOrDefaultAsync();
        //    var result = await context.PurchaseOrders.Where(x => x.CompanyId == clietnId &&
        //       (string.IsNullOrEmpty(invoiceId) ? validatePO(context.InvoiceReceivables.Where(y => y.PoId == x.Id && y.InvoiceStatusId != InvoiceReceivedStatus.Rejected).Count(), x.PercentPaymentTerm1, x.PercentPaymentTerm2, x.PercentPaymentTerm3) : x.Id == poId) &&
        //        !(x.IsDeleted ?? false) && x.Status == StatusTransactionName.Approved)
        //        .Select(x => new { Text = x.PurchaseOrderNumber, Value = x.Id }).ToListAsync();
        //    return result;
        //}

        //private bool validatePO(int paymentto, decimal? term1, decimal? term2, decimal? term3)
        //{
        //    var totalpayment = 0;
        //    if ((term1 ?? 0) != 0) totalpayment++;
        //    if ((term2 ?? 0) != 0) totalpayment++;
        //    if ((term3 ?? 0) != 0) totalpayment++;
        //    return paymentto < totalpayment;
        //}

        public async Task<dynamic> GetPurchaseOrderByVendorId(string vendorId)
        {
            var result = await context.PurchaseOrders.Where(x => x.CompanyId == vendorId && !(x.IsDeleted ?? false) && x.Status == StatusTransactionName.Approved).Select(x => new { Text = x.PurchaseOrderNumber, Value = x.Id }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetTAPDDL()
        {
            var result = await context.TemporaryAccountPayables.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.TapNumber, Value = x.Id }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetInvoiceVendorByPoId(string poId)
        {
            var result = await context.InvoiceReceivables.Where(x => !(x.IsDeleted ?? false) && x.PoId == poId && !x.IsDeleted.GetValueOrDefault()).Select(x => new { Text = x.InvoiceVendor, Value = x.InvoiceVendor }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetChartOfAccount(string code_rec_1, string level)
        {
            var result = await context.ChartOfAccounts.Where(coa => !(coa.IsDeleted ?? false) && coa.CodeRec.Substring(0, 1) == code_rec_1 && coa.Level == level).Select(x => new { Text = x.Name3, Value = x.Id }).ToListAsync();
            return result;
        }
        public async Task<dynamic> GetCOALevel(string level) => await context.ChartOfAccounts.Where(coa => !(coa.IsDeleted ?? false) && coa.Level == level).OrderBy(x => x.CodeRec).Select(x => new { Text = $"({x.CodeRec}) {x.Name5}", Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetCOAByNameLv5() => await context.ChartOfAccounts.Where(coa => !(coa.IsDeleted ?? false) && coa.Level.Equals("5")).OrderBy(x => x.CodeRec).Select(x => new { Text = $"({x.CodeRec}) {x.Name5}", Value = x.Id }).ToListAsync();
        public async Task<dynamic> GetCOABank() => await context.ChartOfAccounts.Where(coa => !(coa.IsDeleted ?? false) && coa.Level.Equals("5") && coa.CodeRec.Contains("1.1.01.")).OrderBy(x => x.CodeRec).Select(x => new { Text = $"({x.CodeRec}) {x.Name5}", Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetCOAOpeningBalance() => await context.ChartOfAccounts
                                    .Where(coa => !(coa.IsDeleted ?? false)
                                            && coa.Level.Equals("5")
                                            && (coa.CodeRec.Split(".", StringSplitOptions.None)[0] == "1"
                                                || coa.CodeRec.Split(".", StringSplitOptions.None)[0] == "2"
                                                || coa.CodeRec.Split(".", StringSplitOptions.None)[0] == "3")
                                    ).OrderBy(x => x.CodeRec).Select(x => new { Text = $"({x.CodeRec}) {x.Name5}", Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetCOAByNameAPLv5() => await context.ChartOfAccounts.Where(coa => !(coa.IsDeleted ?? false) && coa.Level.Equals("5") && coa.CodeRec.Contains("2.1.01.")).OrderBy(x => x.CodeRec).Select(x => new { Text = $"({x.CodeRec}) {x.Name5}", Value = x.Id }).ToListAsync();
        public async Task<dynamic> GetCOAByNameARLv5() => await context.ChartOfAccounts.Where(coa => !(coa.IsDeleted ?? false) && coa.Level.Equals("5") && coa.CodeRec.Contains("1.1.02.")).OrderBy(x => x.CodeRec).Select(x => new { Text = $"({x.CodeRec}) {x.Name5}", Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetChartOfAccountLv5()
        {
            var result = await context.ChartOfAccounts.Where(coa => !(coa.IsDeleted ?? false) && coa.Level.Equals("5")).OrderBy(x => x.CodeRec).Select(x => new { Text = x.CodeRec, Value = x.Id }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetChartOfAccountLv5Like(string search) => await context.ChartOfAccounts.Where(coa => !(coa.IsDeleted ?? false) && coa.Level.Equals("5") && EF.Functions.Like(coa.CodeRec, "%" + search + "%")).OrderBy(x => x.CodeRec).Select(x => new { Text = x.CodeRec, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetMainServiceCategory() => await context.MainServiceCategorys.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.Name, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetMainserviceCategoryPCA()
        {
            var result = await context.MainServiceCategorys.Where(x => !(x.IsDeleted ?? false) && x.Id != MainServiceCategoryOther.MainServiceCategoryId).Select(x => new { Text = x.Name, Value = x.Id }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetPurchaseRequestNumber(string purchaseId)
        {
            var result = await context.PurchaseRequest.Where(x => !(x.IsDeleted ?? false) &&
            !string.IsNullOrEmpty(purchaseId) ? true : !context.RequestForQuotation.Any(y => y.PurchaseRequestId == x.Id))
            .Select(x => new { Text = x.RequestNumber, Value = x.Id }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetJobDetailInRFQTask()
        {
            var result = await context.RequestForQuotationTask.Where(p => !(p.IsDeleted ?? false) && !p.JobId.Equals(null)).ToListAsync();
            return result.Distinct().Select(x => new { Text = x.JobId, Value = x.JobId });
        }

        public async Task<dynamic> GetRFQOpenPO()
        {
            var result = await context.RequestForQuotation.Where(p => !(p.IsDeleted ?? false) && p.Status.Equals(StatusTransaction.StatusName(StatusTransaction.Approved))).ToListAsync();
            return result.Select(x => new { Text = x.RequestForQuotationNumber, Value = x.Id });
        }

        public async Task<dynamic> GetSpecialRequestPOType()
        {
            var result = await context.SpecialRequests.Where(p => !(p.IsDeleted ?? false)).ToListAsync();
            return result.Select(x => new { Text = x.SpecialName, Value = x.Id });
        }

        public async Task<dynamic> GetBrands(string company_id)
        {
            var result = await context.CompanyBrands.Where(p => !(p.IsDeleted ?? false) && p.CompanyId == company_id).ToListAsync();
            return result.Select(x => new { Text = x.BrandName, Value = x.Id });
        }

        public async Task<dynamic> GetBusiness(string affiliationid)
        {
            var result = await context.BusinessUnits.Where(p => !(p.IsDeleted ?? false) && p.AffiliationId == affiliationid).ToListAsync();
            return result.Select(x => new { Text = x.UnitName, Value = x.Id });
        }

        public async Task<dynamic> GetBusinessFinance(string affiliationid)
        {
            var result = await context.BusinessUnits.Where(p => !(p.IsDeleted ?? false) && p.AffiliationId == affiliationid && p.Isfinance == true).ToListAsync();
            return result.Select(x => new { Text = x.UnitName, Value = x.Id });
        }

        public async Task<dynamic> GetBusinessHris(string affiliationid)
        {
            var result = await context.BusinessUnits.Where(p => !(p.IsDeleted ?? false) && p.AffiliationId == affiliationid && p.Ishris == true).ToListAsync();
            return result.Select(x => new { Text = x.UnitName, Value = x.Id });
        }

        public async Task<dynamic> GetAffiliationByIdAsset(string id)
        {
            var result = await context.FixedAsset.Where(p => !(p.IsDeleted ?? false) && p.Id == id).ToListAsync();
            return result.Select(x => new { Text = x.AffiliationId, Value = x.AffiliationId });
        }




        public async Task<dynamic> GetInventoryGoodsReceipt(string poId) => await context.InventoryGoodsReceipts.Where(x => !(x.IsDeleted ?? false) && x.PurchaseOrderId == poId).Select(x => new { Text = x.GoodsReceiptNumber, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetServiceReceipt(string poId) => await context.ServiceReceipt.Where(x => !(x.IsDeleted ?? false) && x.PurchaseOrderId == poId).Select(x => new { Text = x.ServiceReceiptNumber, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetCoaInvoice()
        {
            var result = await context.CoaInvoice.ToListAsync();
            return result.Select(x => new { Text = x.Name5, Value = x.Id });
        }
        public async Task<dynamic> GetCoaInvoiceTaxPayable()
        {
            var result = await context.CoaInvoiceTaxpayable.ToListAsync();
            return result.Select(x => new { Text = x.Name5, Value = x.Id });
        }
        public async Task<dynamic> GetBanks()
        {
            return await (from a in context.Banks
                          join b in context.LegalEntity on a.LegalEntityId equals b.Id
                          where !(a.IsDeleted ?? false)
                          select new { Text = $"({a.AccountNumber}) {a.BankName} - {b.LegalEntityName}", Value = a.Id }).ToListAsync();
        }

        public async Task<dynamic> GetBankByLegalEntity(string legalEntityId)
        {
            return await (from a in context.Banks
                          join b in context.LegalEntity on a.LegalEntityId equals b.Id
                          where !(a.IsDeleted ?? false) && (string.IsNullOrEmpty(legalEntityId) ? true : a.LegalEntityId == legalEntityId)
                          select new { Text = $"({a.AccountNumber}) {a.BankName} - {b.LegalEntityName}", Value = a.Id }).ToListAsync();
        }

        public async Task<dynamic> GetPettyCash()
        {
            return await (from a in context.MasterPettyCashs
                          join b in context.LegalEntity on a.LegalEntityId equals b.Id
                          where !(a.IsDeleted ?? false)
                          select new { Text = $"({a.BankAccountNumber}) {a.BankAccountName} - {b.LegalEntityName}", Value = a.Id }).ToListAsync();
        }

        public async Task<dynamic> GetPettyCashByLegalEntity(string legalEntityId)
        {
            return await (from a in context.MasterPettyCashs
                          join b in context.LegalEntity on a.LegalEntityId equals b.Id
                          where !(a.IsDeleted ?? false) && (string.IsNullOrEmpty(legalEntityId) ? true : a.LegalEntityId == legalEntityId)
                          select new { Text = $"({a.BankAccountNumber}) {a.BankAccountName} - {b.LegalEntityName}", Value = a.Id }).ToListAsync();
        }

        public async Task<dynamic> GetBankandPettyCash()
        {
            return await (from a in context.MasterPettyCashs
                          join b in context.LegalEntity on a.LegalEntityId equals b.Id
                          where !(a.IsDeleted ?? false)
                          select new { Text = a.BankAccountName + "-" + b.LegalEntityName, Value = a.Id })
                    .Union(
                            from a in context.Banks
                            join b in context.LegalEntity on a.LegalEntityId equals b.Id
                            where !(a.IsDeleted ?? false)
                            select new { Text = a.BankName + "-" + b.LegalEntityName, Value = a.Id }
                    ).ToListAsync();
        }

        public async Task<dynamic> GetCoaInvoiceClient()
        {
            var result = await context.CoaInvoiceClients.ToListAsync();
            return result.Select(x => new { Text = x.Name5, Value = x.Id });
        }
        public async Task<dynamic> GetVendorsForInvRecevied(string id) => await context.Companys.Where(x => !(x.IsDeleted ?? false) && x.IsVendor.Equals(true) && string.IsNullOrEmpty(id) ? true : x.Id == id).Select(x => new { Text = x.CompanyName, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetClientInv() => await context.Companys.Where(x => !(x.IsDeleted ?? false) && x.IsClient.Equals(true) && x.IsDeleted.Equals(false)).Select(x => new { Text = x.CompanyName, Value = x.Id }).ToListAsync();

        //public async Task<dynamic> GetBankReceivedType() => await context.BankReceivedType.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.ReceivedType, Value = x.Id }).ToListAsync();


        public async Task<dynamic> GetPCA() => await context.Pcas.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.Code, Value = x.Id }).ToListAsync();


        public async Task<dynamic> GetPCE() => await context.Pces.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.Code, Value = x.Id }).ToListAsync();


        public async Task<dynamic> GetFiscalYear() => await context.FinancialYears.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.FinancialYearName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> GetFiscalYearByLegalId(string legalId)
        {
            if (legalId == "-")
                return await context.FinancialYears.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.FinancialYearName, Value = x.Id }).ToListAsync();
            else
                return await context.FinancialYears.Where(x => !(x.IsDeleted ?? false) && x.LegalEntityId == legalId).Select(x => new { Text = x.FinancialYearName, Value = x.Id }).ToListAsync();
        }

        public async Task<dynamic> GetFiscalPeriod(string Id) => await context.FinancePeriods.Where(x => !(x.IsDeleted ?? false) && x.FiscalYearId == Id).Select(x => new { Text = x.FinancePeriodName, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetMasterLocationProcurement() => await context.MasterLocationProcurement.Where(x => !(x.IsDeleted ?? false) && x.status == "Active").Select(x => new { Text = x.LocationStore, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetOrderReceipt(string id) => await context.OrderReceipt.Where(x => x.IsDeleted.Equals(false) && x.GoodRequestNumber == id).Select(x => new { Text = x.ReceiptOrderNumber, Value = x.Id }).ToListAsync();

        #endregion

        #region PM
        public async Task<dynamic> GetJobNotExistsInPCAandJobId(string Status, string JobId)
        {
            if (Status == StatusTransactionName.Draft || Status == StatusTransactionName.Rejected)
            {
                var userId = context.GetUserId();
                return await (from a in context.PmTeams
                              join b in context.PmTeamMembers on a.Id equals b.TeamId
                              join c in context.JobDetail on a.JobId equals c.Id
                              where !(a.IsDeleted ?? false) && !(b.IsDeleted ?? false) && !(c.IsDeleted ?? false) &&
                                    b.ApplicationUserId == userId && !context.Pcas.Any(y => y.JobId == c.Id && y.JobId != JobId && !(y.IsDeleted ?? false) && y.Status != StatusTransactionName.Rejected)
                              select new { Text = c.JobName, Value = c.Id }).ToListAsync();
            }
            else
            {
                return await context.JobDetail.Where(x => !(x.IsDeleted ?? false) && !context.Pcas.Any(y => y.JobId == x.Id && y.JobId != JobId && (y.IsDeleted ?? false) == false)).Select(x => new { Text = x.JobName, Value = x.Id }).ToListAsync();
            }
        }

        public async Task<dynamic> GetDDLJobList()
        {
            return await context.JobDetail.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.Id + "-" + x.JobName, Value = x.Id }).ToListAsync();
        }

        public async Task<dynamic> GetDDLFolderNameMyDam()
        {
            return await context.peFileFolders.Where(x => x.parent_id == "mydam" && x.is_deleted == false).Select(x => new { Text = new DirectoryInfo(x.folder_path).Name, Value = x.Id }).ToListAsync();
        }

        public async Task<dynamic> GetDDLJobNameMyDam()
        {
            var listJob = await context.MyDam.Where(x => !string.IsNullOrEmpty(x.ParentId)).Select(x => x.ParentId).Distinct().ToListAsync();
            var query = await context.JobDetail.Where(x => listJob.Contains(x.Id)).Select(x => new { Text = x.JobName, Value = x.Id }).Distinct().ToListAsync();
            return query;
        }

        public async Task<dynamic> GetDDLClientNameMyDam()
        {
            var listJob = await context.MyDam.Where(x => !string.IsNullOrEmpty(x.ParentId)).Select(x => x.ParentId).Distinct().ToListAsync();
            var query = await (from q in context.JobDetail
                               join r in context.ClientBrief on q.ClientBriefId equals r.Id
                               join s in context.Companys on r.ClientId equals s.Id
                               where listJob.Contains(q.Id)
                               select new
                               {
                                   Text = s.CompanyName,
                                   Value = s.Id
                               }).Distinct().ToListAsync();
            return query;
        }

        public async Task<dynamic> GetDDLCampaignNameMyDam()
        {
            var listJob = await context.MyDam.Where(x => !string.IsNullOrEmpty(x.ParentId)).Select(x => x.ParentId).Distinct().ToListAsync();
            var query = await (from q in context.JobDetail
                               join r in context.ClientBrief on q.ClientBriefId equals r.Id
                               where listJob.Contains(q.Id)
                               select new
                               {
                                   Text = r.CampaignName,
                                   Value = r.Id
                               }).Distinct().ToListAsync();
            return query;
        }


        public async Task<dynamic> GetJobNotExistsInPCEandJobId(string Status, string JobId)
        {
            //if (Status == StatusTransactionName.Draft || Status == StatusTransactionName.Rejected)
            //{
            //    var userId = context.GetUserId();
            //    return await (from a in context.PmTeams
            //                  join b in context.PmTeamMembers on a.Id equals b.TeamId
            //                  join c in context.JobDetail on a.JobId equals c.Id
            //                  where !(a.IsDeleted ?? false) && !(b.IsDeleted ?? false) && !(c.IsDeleted ?? false) &&
            //                        b.ApplicationUserId == userId && !context.Pces.Any(y => y.JobId == c.Id && y.JobId != JobId && (y.IsDeleted ?? false) == false && y.Status != StatusTransactionName.Rejected)
            //                  select new { Text = c.JobName, Value = c.Id }).ToListAsync();
            //}
            //else
            //{
            return await context.JobDetail.Where(x => !(x.IsDeleted ?? false) && !context.Pces.Any(y => y.JobId == x.Id && y.JobId != JobId && (y.IsDeleted ?? false) == false)).Select(x => new { Text = x.JobName, Value = x.Id }).ToListAsync();
            //}
        }

        public async Task<dynamic> GetMotherPca() => await context.MotherPcas.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.Name, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetRateCard() => await context.RateCards.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.Name, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetTaskRateCard(string rateCardId)
        {
            var model = await context.RateCards.Where(x => !(x.IsDeleted ?? false) && x.Id == rateCardId).FirstOrDefaultAsync();
            if (model != null)
            {
                var detail = model.Detail;
                if (!string.IsNullOrEmpty(detail))
                {
                    var list = JsonConvert.DeserializeObject<List<TaskRateCard>>(detail);
                    return list.Select(x => new { Text = x.task_name, Value = x.task_name });
                }
            }
            return new List<SelectListItem>();
        }
        public async Task<dynamic> GetSubTaskRateCard(string rateCardId, string taskRateCardName)
        {
            var model = await context.RateCards.Where(x => !(x.IsDeleted ?? false) && x.Id == rateCardId).FirstOrDefaultAsync();
            if (model != null)
            {
                var detail = model.Detail;
                if (!string.IsNullOrEmpty(detail))
                {
                    var list = JsonConvert.DeserializeObject<List<TaskRateCard>>(detail);
                    var task = list.Where(x => x.task_name == taskRateCardName).FirstOrDefault();
                    if (task != null)
                    {
                        return task.sub_Task.Select(x => new { Text = x.subtask, Value = x.subtask });
                    }
                }
            }
            return new List<SelectListItem>();
        }

        public async Task<dynamic> GetTypeOfExpense() => await context.TypeOfExpenses.Where(x => !(x.IsDeleted ?? false) && (x.IsActive ?? true)).Select(x => new { Text = x.Name, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetMainserviceCategory() => await context.MainServiceCategorys.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.Name, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetShareservices() => await context.ShareServicess.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.ShareserviceName, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetShareservices(string MainserviceCategoryId) => await context.ShareServicess.Where(x => !(x.IsDeleted ?? false) && x.MainserviceCategoryId == MainserviceCategoryId).Select(x => new { Text = x.ShareserviceName, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetJobId() => await context.JobDetail.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.Id + " - " + x.JobName, Value = x.Id }).Union(context.MasterNonjob.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = "(NON JOB) - " + x.name, Value = x.Id })).ToListAsync();

        public async Task<dynamic> GetChannel() => await context.Channel.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.ChannelName, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetPCEReference(string pceId)
        {
            var result = await context.Pces.Where(x => !(x.IsDeleted ?? false) && x.Id != pceId).Select(x => new { Text = x.Code, Value = x.Id }).ToListAsync();
            return result;
        }

        public async Task<dynamic> MasterOutletList(string clientId) => await context.MasterOutlets.Where(x => !(x.IsDeleted ?? false) && x.ClientId == clientId).Select(x => new { Text = x.OutletName, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetJobByPCEApproved()
        {
            var result = await (from a in context.Pces
                                join b in context.JobDetail on a.JobId equals b.Id
                                where !(a.IsDeleted ?? false) && a.Status == StatusTransactionName.Approved
                                select new { Text = b.Id + " - " + b.JobName, Value = b.Id }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetFixedAssetMovementStatus()
        {
            var result = await context.FixedAssetMovementStatus.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.StatusName, Value = x.Id }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetBankVendor()
        {
            var result = await (from a in context.Banks
                                join c in context.Companys on a.Id equals c.BankId
                                where !(a.IsDeleted ?? false)
                                select new { Text = a.BankName, Value = a.Id }).Distinct().ToListAsync();
            //result = result.Distinct();
            return result;
        }

        public async Task<dynamic> GetGoodRequest() => await context.GoodRequest.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.GoodRequestNumber, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetFixedAsset() => await context.FixedAsset.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.FixedAssetName, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetFixedAssetAP(string journalId) => await context.FixedAsset.Where(x => !(x.IsDeleted ?? false) && !context.JournalTemps.Any(y => y.FixedAssetId == x.Id && y.Id != journalId)).Select(x => new { Text = $"{x.Id} - {x.FixedAssetName}", Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetFixedAssetCombine()
        {
            var result = await context.FixedAsset.Where(x => !(x.IsDeleted ?? false) && !context.FixedAssetDepreciation.Any(y => y.FixedAssetID == x.Id)).Select(x => new { Text = $"{x.Id} - {x.FixedAssetName}", Value = x.Id }).ToListAsync();
            //var result = await (from a in context.FixedAsset.Where(x => !(x.IsDeleted ?? false) &&
            //                     !(from o in context.FixedAssetDepreciation
            //                       select o.FixedAssetID).Contains(x.Id))
            //                    select new { Text = a.Id + "-" + a.FixedAssetName, Value = a.Id }).Distinct().ToListAsync();
            return result;
        }

        public async Task<dynamic> GetFixedAssetCombineAll() => await context.FixedAsset.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.Id + "-" + x.FixedAssetName, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetCOAID() => await context.ChartOfAccounts.Where(x => !(x.IsDeleted ?? false) && x.Level == "5")
            .Select(x => new { Text = x.CodeRec + "-" + x.Name5, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetInventory() => await context.Inventory.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.InventoryName, Value = x.InventoryInNumber }).ToListAsync();

        public async Task<dynamic> GetMasterDepreciation() => await context.MasterDepreciation.Where(x => !(x.IsDeleted ?? false) && x.status == "Active").Select(x => new { Text = x.DepreciationName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> GetAllocationRatio() => await context.CostSharingAllocations.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.Id + " - " + x.CostSharingAllocationName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> GetMasterBudgetCode() => await context.MasterBudgetCodes.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Value = x.Id, Text = x.BudgetCode + " - " + x.BudgetName }).ToListAsync();
        public async Task<dynamic> GetBudgetCode(string itemTypeId, int year, string hrisDivisionId, string budgetOwnerId)
        {
            string source = string.Empty;
            if (itemTypeId == ItemTypeId.Inventory || itemTypeId == ItemTypeId.NonInventory) source = BudgetCodeType.OPEX;
            if (itemTypeId == ItemTypeId.FixedAsset) source = BudgetCodeType.CAPEX;
            var result = await (from a in context.BudgetMasters
                                join b in context.MasterBudgetCodes on a.MasterBudgetCodeId equals b.Id
                                join c in context.BudgetMasterHeaders on a.BudgetMasterHeaderId equals c.Id
                                where !(a.IsDeleted ?? false) && a.HrisDivisionId == hrisDivisionId && c.BudgetYear == year &&
                                c.Source == source && a.Status == StatusTransactionName.Approved &&
                                ((string.IsNullOrEmpty(budgetOwnerId) || budgetOwnerId == "0") ? true : a.BugetOwnerId == budgetOwnerId) && c.Status.ToLower() == "active"
                                select new { Value = a.Id, Text = b.BudgetCode + " - " + b.BudgetName }).ToListAsync();
            return result;
        }
        public async Task<dynamic> GetMasterBudgetCode(string type) => await context.MasterBudgetCodes.Where(x => !(x.IsDeleted ?? false) && x.Source == type).Select(x => new { Text = x.BudgetCode + " - " + x.BudgetName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> GetMasterMonthlyBudgetSpreads() => await context.MasterMonthlyBudgetSpreads.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.MasterMonthlyBudgetName, Value = x.Id }).ToListAsync();
        public async Task<dynamic> GetVersionBudgetCapex(int year) => await context.BudgetMasterHeaders.Where(x => !(x.IsDeleted ?? false) && x.Source == BudgetCodeType.CAPEX && x.BudgetYear == year).Select(x => new { Text = x.Version, Value = x.Version }).Distinct().ToListAsync();
        public async Task<dynamic> GetVersionBudgetOpex(int year) => await context.BudgetMasterHeaders.Where(x => !(x.IsDeleted ?? false) && x.Source == BudgetCodeType.OPEX && x.BudgetYear == year).Select(x => new { Text = x.Version, Value = x.Version }).Distinct().ToListAsync();
        public async Task<dynamic> GetYearPeriod(string legalEntityId)
        {
            int year = 2019;
            var model = await context.FinancialYears.Where(x => !(x.IsDeleted ?? false) && x.LegalEntityId == legalEntityId).OrderBy(x => x.StartDate).FirstOrDefaultAsync();
            if (model != null) year = model.StartDate.Year;
            var result = new List<SelectListItem>();
            for (int i = year; i <= (DateTime.Now.Year + 5); i++) result.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            return result;
        }

        public async Task<dynamic> GetPurchaceOrderByClientIdInGoodReceipt(string clientId, string grId)
        {
            var gr = await (from a in context.InventoryGoodsReceipts
                            join b in context.InventoryGoodsReceiptsDetail on a.Id equals b.InventoryGoodsReceiptId
                            where !(a.IsDeleted ?? false) && b.RemainingQty == 0 && a.Id != grId
                            select a.PurchaseOrderNumber).Distinct().ToListAsync();

            var listStatus = new List<string>();
            listStatus.Add(StatusTransactionName.Approved);
            listStatus.Add(StatusTransactionName.Publish);

            if (grId != null)
            {

                //return await context.PurchaseOrders.Where(x => !(x.IsDeleted ?? false)
                //          && x.CompanyId == clientId
                //          //&& !gr.Contains(x.PurchaseOrderNumber)
                //          && !context.InventoryGoodsReceipts.Any(y => y.PurchaseOrderId == x.Id && y.Status == StatusTransactionName.Draft && y.Id != grId)
                //          && x.Status == StatusTransactionName.Approved)
                //            .Select(x => new { Text = x.PurchaseOrderNumber, Value = x.Id }).AsNoTracking().ToListAsync();
            }
            //else
            //{
            //    //return await context.PurchaseOrders.Where(x => !(x.IsDeleted ?? false)
            //    //         && x.CompanyId == clientId
            //    //         //&& !context.InventoryGoodsReceipts.Any(y => y.PurchaseOrderId == x.Id && y.Status == StatusTransactionName.Draft && y.Id != grId)
            //    //         && !gr.Contains(x.PurchaseOrderNumber)
            //    //         && x.Status == StatusTransactionName.Approved)
            //    //           .Select(x => new { Text = x.PurchaseOrderNumber, Value = x.Id }).AsNoTracking().ToListAsync();
            //}

            return await context.PurchaseOrders.Where(x => !(x.IsDeleted ?? false)
                        && x.CompanyId == clientId
                        //&& !context.InventoryGoodsReceipts.Any(y => y.PurchaseOrderId == x.Id && y.Status == StatusTransactionName.Draft && y.Id != grId)
                        && !gr.Contains(x.PurchaseOrderNumber)
                        && listStatus.Contains(x.Status))
                          .Select(x => new { Text = x.PurchaseOrderNumber, Value = x.Id }).AsNoTracking().ToListAsync();

        }

        public async Task<dynamic> MoveLocation() => await context.Movement_Locations.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.movement_name, Value = x.Id }).ToListAsync();

        public async Task<dynamic> LocationWarehouse() => await context.Warehouse.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.WarehouseName, Value = x.Id }).ToListAsync();

        public async Task<dynamic> InvGetform(string param) => await context.Inventory_GetForms.Where(x => !(x.IsDeleted ?? false) && x.getform_status.Contains(param)).Select(x => new { Text = x.getform_name, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetTaskCAS(string casId) => await context.CASettlementDetail.Where(x => !(x.IsDeleted ?? false) && x.CaSettlementId == casId).Select(x => new { Text = x.TaskName, Value = x.TaskId }).ToListAsync();

        public async Task<dynamic> GetVendorNames() => await context.Companys.Where(x => x.IsVendor.Equals(true)).Select(x => new { Text = x.CompanyName, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetItemCategory() => await context.ItemCategories.Where(x => x.IsActive.Equals(true)).Select(x => new { Text = x.CategoryName, Value = x.Id }).ToListAsync();

        public async Task<dynamic> GetJournalAdjustmentNumber(JournalAdjustmentSearchNumber search)
        {
            if (search.Type == JournalAdjustmentType.AccountPayable)
            {
                return await (from a in context.AccountPayables
                              join b in context.Journal on a.Id equals b.ReferenceId
                              where !(a.IsDeleted ?? false) &&
                              (search.FiscalYear != null ? Convert.ToDateTime(b.ReferenceDate ?? b.TransactionDate).Year == search.FiscalYear : true) &&
                              ((search.StartPeriod != null && search.EndPeriod != null) ? (Convert.ToDateTime(b.ReferenceDate ?? b.TransactionDate) >= search.StartPeriod && Convert.ToDateTime(b.ReferenceDate ?? b.TransactionDate) <= search.EndPeriod) : true)
                              select new { Text = a.AccountPayableNumber, Value = a.AccountPayableNumber }).Distinct().ToListAsync();
            }
            else if (search.Type == JournalAdjustmentType.AccountReceivable)
            {
                return await (from a in context.AccountReceivables
                              join b in context.Journal on a.Id equals b.ReferenceId
                              where !(a.IsDeleted ?? false) &&
                              (search.FiscalYear != null ? Convert.ToDateTime(b.ReferenceDate ?? b.TransactionDate).Year == search.FiscalYear : true) &&
                              ((search.StartPeriod != null && search.EndPeriod != null) ? (Convert.ToDateTime(b.ReferenceDate ?? b.TransactionDate) >= search.StartPeriod && Convert.ToDateTime(b.ReferenceDate ?? b.TransactionDate) <= search.EndPeriod) : true)
                              select new { Text = a.AccountReceivableNumber, Value = a.AccountReceivableNumber }).Distinct().ToListAsync();
            }
            else if (search.Type == JournalAdjustmentType.CashAdvanceSettlement)
            {
                return await (from a in context.CASettlement
                              join b in context.Journal on a.Id equals b.ReferenceId
                              where !(a.IsDeleted ?? false) &&
                              (search.FiscalYear != null ? Convert.ToDateTime(b.ReferenceDate ?? b.TransactionDate).Year == search.FiscalYear : true) &&
                              ((search.StartPeriod != null && search.EndPeriod != null) ? (Convert.ToDateTime(b.ReferenceDate ?? b.TransactionDate) >= search.StartPeriod && Convert.ToDateTime(b.ReferenceDate ?? b.TransactionDate) <= search.EndPeriod) : true)
                              select new { Text = a.SettlementNumber, Value = a.SettlementNumber }).Distinct().ToListAsync();
            }
            else if (search.Type == JournalAdjustmentType.BankPayment)
            {
                return await (from a in context.PaymentRequest
                              join b in context.Journal on a.Id equals b.ReferenceId
                              where !(a.IsDeleted ?? false) && a.Status == PaymentRequestStatus.Complete &&
                              (search.FiscalYear != null ? Convert.ToDateTime(b.ReferenceDate ?? b.TransactionDate).Year == search.FiscalYear : true) &&
                              ((search.StartPeriod != null && search.EndPeriod != null) ? (Convert.ToDateTime(b.ReferenceDate ?? b.TransactionDate) >= search.StartPeriod && Convert.ToDateTime(b.ReferenceDate ?? b.TransactionDate) <= search.EndPeriod) : true)
                              select new { Text = a.PaymentRequestNumber, Value = a.PaymentRequestNumber }).Distinct().ToListAsync();
            }
            else if (search.Type == JournalAdjustmentType.BankReceived)
            {
                return await (from a in context.BankReceived
                              join b in context.Journal on a.Id equals b.ReferenceId
                              where !(a.IsDeleted ?? false) &&
                              (search.FiscalYear != null ? Convert.ToDateTime(b.ReferenceDate ?? b.TransactionDate).Year == search.FiscalYear : true) &&
                              ((search.StartPeriod != null && search.EndPeriod != null) ? (Convert.ToDateTime(b.ReferenceDate ?? b.TransactionDate) >= search.StartPeriod && Convert.ToDateTime(b.ReferenceDate ?? b.TransactionDate) <= search.EndPeriod) : true)
                              select new { Text = a.PaymentRequestNumber, Value = a.PaymentRequestNumber }).Distinct().ToListAsync();
            }
            else if (search.Type == JournalAdjustmentType.PettyCashSettlement)
            {
                return await (from a in context.PettyCashs
                              join b in context.Journal on a.Id equals b.ReferenceId
                              where !(a.IsDeleted ?? false) &&
                              (search.FiscalYear != null ? Convert.ToDateTime(b.ReferenceDate ?? b.TransactionDate).Year == search.FiscalYear : true) &&
                              ((search.StartPeriod != null && search.EndPeriod != null) ? (Convert.ToDateTime(b.ReferenceDate ?? b.TransactionDate) >= search.StartPeriod && Convert.ToDateTime(b.ReferenceDate ?? b.TransactionDate) <= search.EndPeriod) : true)
                              select new { Text = a.Id, Value = a.Id }).Distinct().ToListAsync();
            }
            return new List<SelectListItem>();
        }


        public async Task<dynamic> GetSalaryAllocationNumber(SalaryAllocationSearchNumber search)
        {
            if (search.Type == SalaryAllocationType.AccountPayable)
            {
                return await (from a in context.AccountPayables
                              join b in context.Journal on a.Id equals b.ReferenceId
                              where !(a.IsDeleted ?? false) &&
                              (search.FiscalYear != null ? Convert.ToDateTime(b.ReferenceDate ?? b.TransactionDate).Year == search.FiscalYear : true) &&
                              ((search.StartPeriod != null && search.EndPeriod != null) ? (Convert.ToDateTime(b.ReferenceDate ?? b.TransactionDate) >= search.StartPeriod && Convert.ToDateTime(b.ReferenceDate ?? b.TransactionDate) <= search.EndPeriod) : true)
                              select new { Text = a.AccountPayableNumber, Value = a.AccountPayableNumber }).Distinct().ToListAsync();
            }
            else if (search.Type == SalaryAllocationType.BankPayment)
            {
                return await (from a in context.PaymentRequest
                              join b in context.Journal on a.Id equals b.ReferenceId
                              where !(a.IsDeleted ?? false) && a.Status == PaymentRequestStatus.Complete &&
                              (search.FiscalYear != null ? Convert.ToDateTime(b.ReferenceDate ?? b.TransactionDate).Year == search.FiscalYear : true) &&
                              ((search.StartPeriod != null && search.EndPeriod != null) ? (Convert.ToDateTime(b.ReferenceDate ?? b.TransactionDate) >= search.StartPeriod && Convert.ToDateTime(b.ReferenceDate ?? b.TransactionDate) <= search.EndPeriod) : true)
                              select new { Text = a.PaymentRequestNumber, Value = a.PaymentRequestNumber }).Distinct().ToListAsync();
            }
            return new List<SelectListItem>();
        }

        public async Task<dynamic> GetCOALevel() => await context.ChartOfAccounts.Where(x => !(x.IsDeleted ?? false)).Select(x => new { Text = x.Level, Value = x.Level }).Distinct().ToListAsync();

        #endregion
    }
}
