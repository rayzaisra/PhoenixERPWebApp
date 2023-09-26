using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Media.Transaction.MediaPlanTV;
using Phoenix.Data.Models.Media.MasterData.MediaPlanStatus;
using Phoenix.Data.Models.Um;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Phoenix.Data.Models.Media;
using Phoenix.Data.Models.Media.MasterData.MediaOrderStatus;
using Phoenix.Data.Models.Media.MasterData.ProgramPosition;
using Phoenix.Data.Models.Media.MasterData.ProgramType;
using Phoenix.Data.Models.Finance.Master;
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data.Models.Finance.Transaction;
using Phoenix.Data.Models.Finance;
using Phoenix.Data.Models.Finance.Transaction.InventoryIn;
using Phoenix.Data.Models.Finance.Transaction.Item;
using Phoenix.Data.Models.Finance.Transaction.ServiceReceipt;
using Phoenix.Data.Models.Media.Transaction.MediaOrder;
using Phoenix.Data.Models.Finance.MasterData;
using Phoenix.Data.Models.Finance.Transaction.GoodReturn;
using Phoenix.Data.Models.HrisNew.MasterData.Training;
using Phoenix.Data.Models.Finance.Transaction.Inventory;
using Phoenix.Data.Models.Pm.MasterData;
using Phoenix.Shared.Core.Entities.ProjectManagement;
using Phoenix.Data.Models.Finance.Transaction.FixedAssetOutMovement;

namespace Phoenix.Data
{
    public class DataContext : DbContext
    {
        public IConfiguration configuration;
        public HttpContext HttpContext { get; }

        public DataContext(IHttpContextAccessor accessor, IConfiguration configuration)
        {
            HttpContext = accessor.HttpContext;
            this.configuration = configuration;
            Database.SetCommandTimeout(60000);
        }

        //public DataContext(DbContextOptions<Shared.Core.Contexts.EFContext> options, IHttpContextAccessor accessor, IConfiguration configuration)
        //{
        //    HttpContext = accessor.HttpContext;
        //    this.configuration = configuration;
        //}

        public override int SaveChanges()
        {
            var entities = from e in ChangeTracker.Entries()
                           where e.State == EntityState.Added
                               || e.State == EntityState.Modified
                           select e.Entity;
            foreach (var entity in entities)
            {
                var validationContext = new ValidationContext(entity);
                Validator.ValidateObject(entity, validationContext);
            }

            return base.SaveChanges();
        }
        //private void ParseToken()
        //{
        //    if (HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues value))
        //    {
        //        var principal = Shared.Core.Filters.JwtManager.GetPrincipal(value.ToString().Substring("Bearer ".Length));
        //        //var identity = principal.Identity as ClaimsIdentity;
        //        var claim = principal.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType); //var claim = context.HttpContext.User.Claims.Where(c => c.Type == ClaimsIdentity.DefaultNameClaimType).FirstOrDefault();
        //        var username = claim.Value;
        //    }
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                if (!optionsBuilder.IsConfigured)
                {

                    optionsBuilder.UseSqlServer(configuration.GetConnectionString("baseConnectionString"));
                    optionsBuilder.EnableSensitiveDataLogging();
                }
                base.OnConfiguring(optionsBuilder);
            }
            catch (Exception ex)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.HasDefaultSchema("hr");
            modelBuilder.Entity<TransactionId>().HasKey(x => new { x.TransId, x.RequestDate });
            modelBuilder.Entity<StatusLog>().HasKey(x => new { x.TransactionId, x.Seq });

            modelBuilder.Entity<TmMenu>()
                .HasMany(x => x.TmUserAccesses)
                .WithOne(x => x.TmMenu)
                .HasForeignKey(x => new { x.MenuId });

            modelBuilder.Entity<TmMenu>()
               .HasMany(x => x.TmGroupAccesses)
               .WithOne(x => x.TmMenu)
               .HasForeignKey(x => new { x.MenuId });

        }

        #region GLOBAL
        public DbSet<TransactionLog> TransactionLogs { get; set; }
        public DbSet<TransactionId> TransactionIds { get; set; }
        public DbSet<EmployeeBasicInfo> EmployeeBasicInfos { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<JobGrade> JobGrades { get; set; }
        public DbSet<JobLevel> JobLevels { get; set; }
        public DbSet<BusinessUnitType> BusinessUnitTypes { get; set; }
        public DbSet<BusinessUnitJobLevel> BusinessUnitJobLevels { get; set; }
        public DbSet<EmployementStatus> EmployementStatuss { get; set; }
        public DbSet<Filemaster> Filemasters { get; set; }
        public DbSet<EmployeeContract> EmployeeContracts { get; set; }
        public DbSet<EmployeeChecklist> EmployeeChecklists { get; set; }
        public DbSet<EmployeeChecklistCategory> EmployeeChecklistCategorys { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<MarriedStatus> MarriedStatuss { get; set; }
        public DbSet<Family> Familys { get; set; }
        public DbSet<FamilyStatus> FamilyStatuss { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Uom> Uom { get; set; }
        public DbSet<PaymentType> PaymentType { get; set; }
        //public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<SysParams> SysParams { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<StatusLog> StatusLogs { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }
        #endregion

        #region HRIS
        #region Master Data
        //Payroll
        public DbSet<SalaryMainComponent> SalaryMainComponents { get; set; }
        public DbSet<UserEmployeePayroll> UserEmployeePayrolls { get; set; }
        public DbSet<SalaryStandard> SalaryStandards { get; set; }
        public DbSet<LoanCategory> LoanCategorys { get; set; }
        public DbSet<PtkpSetting> PtkpSettings { get; set; }
        public DbSet<MasterCutoff> MasterCutoffs { get; set; }
        public DbSet<BpjsComponent> BpjsComponents { get; set; }
        public DbSet<AllowanceComponent> AllowanceComponents { get; set; }
        public DbSet<DeductionComponent> DeductionComponents { get; set; }
        public DbSet<InsuranceCompany> InsuranceCompanys { get; set; }
        public DbSet<EmployeePayrollGroup> EmployeePayrollGroups { get; set; }
        public DbSet<GroupSubcomponentDeduction> GroupSubcomponentDeductions { get; set; }
        public DbSet<GroupSubcomponentAdditional> GroupSubcomponentAdditionals { get; set; }
        public DbSet<DeclarationPayrollHeader> DeclarationPayrollHeader { get; set; }
        public DbSet<DeclarationPayrollDetail> DeclarationPayrollDetail { get; set; }
        //Termination
        public DbSet<TmTerminationType> TmTerminationTypes { get; set; }
        public DbSet<FaultCategory> FaultCategorys { get; set; }
        public DbSet<CategoryExitInterview> CategoryExitInterviews { get; set; }
        public DbSet<ExitInterviewItem> ExitInterviewItems { get; set; }

        //Employee Self Service
        public DbSet<MasterDivisionSelfservice> MasterDivisionSelfservices { get; set; }
        public DbSet<MasterMappingRequest> MasterMappingRequests { get; set; }

        //Leave
        public DbSet<LeaveRequestType> LeaveRequestTypes { get; set; }

        //LegalDocument
        public DbSet<LegalCategory> LegalCategorys { get; set; }
        public DbSet<LegalDocumentMasterCounterpart> LegalDocumentMasterCounterparts { get; set; }

        //PDR
        public DbSet<PdrCategory> PdrCategorys { get; set; }
        public DbSet<PdrParameter> PdrParameters { get; set; }
        public DbSet<PdrGrade> PdrGrades { get; set; }
        public DbSet<PdrSalaryIncrease> PdrSalaryIncreases { get; set; }
        public DbSet<PdrSalaryGrade> PdrSalaryGrades { get; set; }

        //PDR Appraisal
        public DbSet<BscCorporateObjective> BscCorporateObjectives { get; set; }
        public DbSet<PdrObjectiveSetting> PdrObjectiveSettings { get; set; }
        public DbSet<PdrObjectiveTarget> PdrObjectiveTargets { get; set; }
        public DbSet<PdrObjectiveTargetDetail> PdrObjectiveTargetDetails { get; set; }

        public DbSet<Religion> Religions { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<EducationLevel> EducationLevels { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<EmployeeExperience> EmployeeExperiences { get; set; }
        public DbSet<EmployeeEmergencyContact> EmployeeEmergencyContacts { get; set; }
        public DbSet<EmployeeTraining> EmployeeTrainings { get; set; }
        public DbSet<Nationality> Nationalitys { get; set; }
        public DbSet<MasterTrainingRequest> MasterTrainingRequests { get; set; }

        #endregion
        #region Transaction
        //Termination
        public DbSet<Termination> Terminations { get; set; }
        public DbSet<ExitInterview> ExitInterviews { get; set; }
        public DbSet<ExitChecklist> ExitChecklists { get; set; }

        //Employee Self Service
        public DbSet<EmployeeServiceRequest> EmployeeServiceRequests { get; set; }
        public DbSet<Phoenix.Shared.Core.Entities.CandidateRecruitment> CandidateRecruitments { get; set; }
        public DbSet<Phoenix.Shared.Core.Entities.CandidateRecruitmentSignContract> CandidateRecruitmentsSignContract { get; set; }
        //Attendance
        public DbSet<MasterWorkingTime> MasterWorkingTimes { get; set; }
        public DbSet<EmployeeLocation> EmployeeLocations { get; set; }
        public DbSet<MasterLocation> MasterLocations { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        //Leave
        public DbSet<LeaveBalance> LeaveBalances { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<LeaveTransaction> LeaveTransactions { get; set; }
        public DbSet<MassLeave> MassLeaves { get; set; }
        public DbSet<MassLeaveDetail> MassLeaveDetails { get; set; }


        //LegalDocument
        public DbSet<LegalDocumentRequest> LegalDocumentRequests { get; set; }
        public DbSet<LegalDocument> LegalDocuments { get; set; }
        public DbSet<LegalDocumentCounterpart> LegalDocumentCounterparts { get; set; }
        public DbSet<LegalSharingRequest> LegalSharingRequests { get; set; }

        //Overtime
        public DbSet<Overtime> Overtimes { get; set; }

        //Payroll
        public DbSet<EmployeeLoan> EmployeeLoans { get; set; }
        public DbSet<EmployeeLoanPaid> EmployeeLoanPaids { get; set; }
        public DbSet<EmployeeSubcomponentAdditional> EmployeeSubcomponentAdditionals { get; set; }
        public DbSet<EmployeeSubcomponentDeduction> EmployeeSubcomponentDeductions { get; set; }
        public DbSet<EmployeeHealthInsurance> EmployeeHealthInsurances { get; set; }
        public DbSet<EmployeeHealthInsuranceFamily> EmployeeHealthInsuranceFamilys { get; set; }
        //Generate payroll
        public DbSet<SalaryProcessHeader> SalaryProcessHeaders { get; set; }
        public DbSet<SalaryProcess> SalaryProcesss { get; set; }
        public DbSet<SalaryProcessComponent> SalaryProcessComponents { get; set; }
        public DbSet<SalaryProcessDetailPayroll> SalaryProcessDetailPayroll { get; set; }


        //Promotion
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<PromotionSubcomponentAdditional> PromotionSubcomponentAdditionals { get; set; }
        public DbSet<PromotionSubcomponentDeduction> PromotionSubcomponentDeductions { get; set; }

        //Rotation
        public DbSet<Rotation> Rotations { get; set; }
        public DbSet<RotationSubcomponentAdditional> RotationSubcomponentAdditionals { get; set; }
        public DbSet<RotationSubcomponentDeduction> RotationSubcomponentDeductions { get; set; }

        //Demotion
        public DbSet<Demotion> Demotions { get; set; }

        //PDR
        public DbSet<PdrEmployee> PdrEmployees { get; set; }
        public DbSet<PdrDetailM2below> PdrDetailM2belows { get; set; }
        public DbSet<PdrRecomendation> PdrRecomendations { get; set; }
        public DbSet<PdrOverallScore> PdrOverallScores { get; set; }
        public DbSet<PdrDevelopmentPlan> PdrDevelopmentPlans { get; set; }

        //PDR Appraisal
        public DbSet<PdrApprailsalM1Header> PdrApprailsalM1Headers { get; set; }
        public DbSet<PdrApprailsalM1> PdrApprailsalM1s { get; set; }


        //Training
        public DbSet<Training> Training { get; set; }
        public DbSet<TrainingRequisition> TrainingRequisition { get; set; }
        public DbSet<TrainingRequisitionDetail> TrainingRequisitionDetail { get; set; }
        public DbSet<TrainingCategory> TrainingCategory { get; set; }
        public DbSet<TrainingSubject> TrainingSubject { get; set; }
        public DbSet<TrainingType> TrainingType { get; set; }
        public DbSet<TrainingBonding> TrainingBonding { get; set; }

        public DbSet<TrainingRegPayment> TrainingRegPayment { get; set; }
        public DbSet<TrainingShareSession> TrainingShareSession { get; set; }

        public DbSet<TrainingRegpaymentDetail> TrainingRegpaymentDetail { get; set; }


        #endregion
        #endregion

        #region UM
        public DbSet<TmGroup> TmGroups { get; set; }
        public DbSet<TmMenu> TmMenus { get; set; }
        public DbSet<TmGroupAccess> TmGroupAccesses { get; set; }
        public DbSet<TmUserAccess> TmUserAccesses { get; set; }
        public DbSet<TmUserApp> TmUserApps { get; set; }
        public DbSet<TmTemplateApproval> TmTemplateApprovals { get; set; }
        public DbSet<TmConditionUserApproval> TmConditionUserApprovals { get; set; }
        public DbSet<TmTemplateUserApproval> TmTemplateUserApprovals { get; set; }
        public DbSet<TmAuthorization> TmAuthorizations { get; set; }

        public DbSet<TrTemplateApproval> TrTemplateApprovals { get; set; }
        public DbSet<TrUserApproval> TrUserApprovals { get; set; }
        public DbSet<TrConditionUserApproval> TrConditionUserApprovals { get; set; }
        public DbSet<AuditTrails> AuditTrails { get; set; }
        public DbSet<Doa> DOAs { get; set; }
        public DbSet<ChatInfo> ChatInfos { get; set; }
        public DbSet<ChattingAndComment> ChattingAndComments { get; set; }
        public DbSet<GroupChatting> GroupChattings { get; set; }
        public DbSet<GroupChattingMember> GroupChattingMembers { get; set; }
        public DbSet<TmEmailTemplate> TmEmailTemplates { get; set; }
        public DbSet<AzurePortalSetup> AzurePortalSetups { get; set; }
        public DbSet<TrMappingFolderOneDrive> TrMappingFolderOneDrives { get; set; }


        #endregion

        #region Media
        public DbSet<TmMediaType> MediaTypes { get; set; }
        public DbSet<TrMediaFileMaster> TrMediaFileMasters { get; set; }
        public DbSet<TrMediaPlanTV> TrMediaPlanTVs { get; set; }
        public DbSet<TrMediaOrderTV> TrMediaOrderTVs { get; set; }
        public DbSet<TrMediaOrderTVDetail> TrMediaOrderTVDetails { get; set; }
        public DbSet<TrMediaOrderTVDetailSchedule> TrMediaOrderTVDetailSchedules { get; set; }
        public DbSet<TrMediaPlanTVDetail> TrMediaPlanTVDetails { get; set; }
        public DbSet<TrMediaPlanTVDetailSchedule> TrMediaPlanTVDetailSchedules { get; set; }
        public DbSet<TmMediaPlanStatus> TmMediaPlanStatuses { get; set; }
        public DbSet<TmMediaPlanRequestType> TmMediaPlanRequestTypes { get; set; }
        public DbSet<TmMediaOrderStatus> TmMediaOrderStatuses { get; set; }
        public DbSet<TmProgramPosition> TmProgramPositions { get; set; }
        public DbSet<TmProgramType> TmProgramTypes { get; set; }
        public DbSet<TmProgramCategoryType> TmProgramCategoryTypes { get; set; }
        public DbSet<MediaPlanHeader> MediaPlanHeaders { get; set; }
        public DbSet<MediaPlanDetail> MediaPlanDetail { get; set; }
        public DbSet<MediaOrderHeader> MediaOrderHeaders { get; set; }
        public DbSet<MediaOrderDetail> MediaOrderDetails { get; set; }
        #endregion

        #region Finance
        #region Master Data
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Affiliation> Affiliations { get; set; }
        public DbSet<EFaktur> EFaktur { get; set; }
        public DbSet<EFakturDetail> EFakturDetail { get; set; }
        public DbSet<CashFlow> CashFlows { get; set; }
        public DbSet<BusinessUnit> BusinessUnits { get; set; }
        public DbSet<ChartofaccountReport> ChartofaccountReports { get; set; }
        public DbSet<MainServiceCategory> MainServiceCategorys { get; set; }
        public DbSet<ShareServices> ShareServicess { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<GetFrom> GetFroms { get; set; }
        public DbSet<Country> Countrys { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Citys { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<CompanyBrand> CompanyBrands { get; set; }
        public DbSet<MasterBudgetCode> MasterBudgetCodes { get; set; }
        public DbSet<CostSharingAllocation> CostSharingAllocations { get; set; }
        public DbSet<CostSharingAllocationDetail> CostSharingAllocationDetails { get; set; }
        public DbSet<LegalEntity> LegalEntity { get; set; }
        public DbSet<MasterOutlet> MasterOutlets { get; set; }
        public DbSet<JobClosingHeader> JobClosingHeaders { get; set; }
        public DbSet<JobClosingDetail> JobClosingDetails { get; set; }
        public DbSet<JobClosingDetailJournal> JobClosingDetailJournal { get; set; }
        public DbSet<MasterPettyCash> MasterPettyCashs { get; set; }
        public DbSet<MapCoaTransaksi> MapCoaTransaksi { get; set; }
        public DbSet<MasterMonthlyBudgetSpread> MasterMonthlyBudgetSpreads { get; set; }
        public DbSet<BudgetMasterHeader> BudgetMasterHeaders { get; set; }
        public DbSet<BudgetMaster> BudgetMasters { get; set; }
        public DbSet<BudgetMasterDetail> BudgetMasterDetails { get; set; }
        public DbSet<Movement_Location> Movement_Locations { get; set; }
        public DbSet<Inventory_GetForm> Inventory_GetForms { get; set; }
        public DbSet<FinReportControl> FinReportControls { get; set; }
        public DbSet<MasterOpeningBalance> MasterOpeningBalances { get; set; }
        #endregion

        public DbSet<TimeSheetReportByBusinessUnit> TimeSheetReportByBusinessUnit { get; set; }
        public DbSet<TimeSheetReportByBusinessUnitDetail> TimeSheetReportByBusinessUnitDetail { get; set; }
        public DbSet<CashAdvance> CashAdvances { get; set; }
        public DbSet<CashAdvanceDetail> CashAdvanceDetails { get; set; }
        public DbSet<CASettlement> CASettlement { get; set; }
        public DbSet<CASettlementDetail> CASettlementDetail { get; set; }
        public DbSet<CASettlementJournalReverse> CASettlementJournalReverse { get; set; }
        public DbSet<InvoiceReceivable> InvoiceReceivables { get; set; }
        public DbSet<InvoiceReceivableDetail> InvoiceReceivableDetails { get; set; }
        public DbSet<AccountReceivable> AccountReceivables { get; set; }
        public DbSet<Currency> Currencies { get; set; }

        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public DbSet<TemporaryAccountPayable> TemporaryAccountPayables { get; set; }
        public DbSet<TemporaryAccountPayableDetail> TemporaryAccountPayableDetail { get; set; }
        public DbSet<AccountPayable> AccountPayables { get; set; }
        public DbSet<AccountPayableDetail> AccountPayableDetails { get; set; }
        public DbSet<ChartOfAccount> ChartOfAccounts { get; set; }
        //Finance
        public DbSet<MappingJurnal> MappingJurnals { get; set; }
        public DbSet<PettyCash> PettyCashs { get; set; }
        public DbSet<PettyCashDetail> PettyCashDetails { get; set; }
        //public DbSet<Budget> Budget { get; set; }
        public DbSet<TypeBudget> TypeBudget { get; set; }
        public DbSet<FinancialYear> FinancialYears { get; set; }
        public DbSet<FinancePeriod> FinancePeriods { get; set; }
        public DbSet<TypeBudgetHdr> TypeBudgetHdr { get; set; }
        public DbSet<InvoiceClient> InvoiceClients { get; set; }
        public DbSet<InvoiceClientItemOther> InvoiceClientItemOthers { get; set; }
        //public DbSet<TemporaryAccountPayableJournal> TemporaryAccountPayableJournal { get; set; }
        public DbSet<Reconcile> Reconcile { get; set; }
        public DbSet<RequestForQuotation> RequestForQuotation { get; set; }
        public DbSet<RequestForQuotationDetail> RequestForQuotationDetail { get; set; }
        public DbSet<RequestForQuotationVendor> RequestForQuotationVendor { get; set; }
        public DbSet<RequestForQuotationTask> RequestForQuotationTask { get; set; }
        public DbSet<PurchaseRequest> PurchaseRequest { get; set; }
        public DbSet<PurchaseRequestDetail> PurchaseRequestDetail { get; set; }
        public DbSet<InventoryInModel> InventoryIns { get; set; }
        public DbSet<InventoryGoodsReceipt> InventoryGoodsReceipts { get; set; }
        public DbSet<InventoryGoodsReceiptDetail> InventoryGoodsReceiptsDetail { get; set; }
        public DbSet<ServiceReceipt> ServiceReceipt { get; set; }
        public DbSet<ServiceReceiptDetail> ServiceReceiptDetail { get; set; }
        public DbSet<GoodRequest> GoodRequest { get; set; }
        public DbSet<OrderReceipt> OrderReceipt { get; set; }
        public DbSet<GoodRequestDetail> GoodRequestDetail { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<ItemCategories> ItemCategories { get; set; }
        public DbSet<ItemConditions> ItemConditions { get; set; }
        public DbSet<ItemGroups> ItemGroups { get; set; }
        public DbSet<ItemStocks> ItemStocks { get; set; }
        public DbSet<ItemType> ItemType { get; set; }
        public DbSet<PaymentRequest> PaymentRequest { get; set; }
        public DbSet<SpecialRequest> SpecialRequests { get; set; }
        public DbSet<JournalTemp> JournalTemps { get; set; }
        public DbSet<BankPayment> BankPayment { get; set; }
        //public DbSet<MasterDepartment> MasterDepartments { get; set; }
        public DbSet<Journal> Journal { get; set; }
        public DbSet<BankReceived> BankReceived { get; set; }
        public DbSet<BankReceivedDetail> BankReceivedDetail { get; set; }
        public DbSet<BankReceivedDetailJournal> BankReceivedDetailJournal { get; set; }
        public DbSet<MasterDivision> MasterDivision { get; set; }
        public DbSet<PaymentRequestDetail> PaymentRequestDetail { get; set; }
        public DbSet<PaymentRequestDetailJournal> PaymentRequestDetailJournal { get; set; }
        //Master Finance
        public DbSet<MasterLocationProcurement> MasterLocationProcurement { get; set; }
        public DbSet<MasterConditionProcurement> MasterConditionProcurement { get; set; }
        public DbSet<MasterDepreciation> MasterDepreciation { get; set; }
        public DbSet<FixedAsset> FixedAsset { get; set; }
        public DbSet<FixedAssetOutMovement> FixedAssetOutMovement { get; set; }
        public DbSet<FixedAssetMovement> FixedAssetMovement { get; set; }
        public DbSet<FixedAssetMovementDetail> FixedAssetMovementDetail { get; set; }
        public DbSet<FixedAssetMovementStatus> FixedAssetMovementStatus { get; set; }
        public DbSet<FixedAssetDepreciation> FixedAssetDepreciation { get; set; }
        public DbSet<FixedAssetDepreciationDetail> FixedAssetDepreciationDetail { get; set; }
        public DbSet<FixedAssetOutMovementDetail> FixedassetOutMovementDetail { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<InventoryDetail> InventoryDetail { get; set; }
        public DbSet<InventoryDetailStatus> InventoryDetailStatus { get; set; }
        public DbSet<Bast> Bast { get; set; }
        public DbSet<BastDetail> BastDetail { get; set; }
        public DbSet<InvoiceDeliveryNotes> InvoiceDeliveryNotes { get; set; }
        public DbSet<InventoryGoodsReturn> InventoryGoodsReturn { get; set; }
        public DbSet<InventoryGoodsReturnDetail> InventoryGoodsReturnDetail { get; set; }
        public DbSet<RevaluationList> RevaluationLists { get; set; }
        public DbSet<SalaryPaymentRequest> SalaryPaymentRequests { get; set; }
        public DbSet<SalaryPaymentRequestDetail> SalaryPaymentRequestDetails { get; set; }
        public DbSet<JournalWriteOffAp> JournalWriteOffAp { get; set; }
        public DbSet<JournalWriteOffAr> JournalWriteOffAr { get; set; }
        #endregion

        #region Pm
        //Tidak menggunakan framework kendo telerik
        public DbSet<DamFileSharing> DamFileSharings { get; set; }
        public DbSet<Phoenix.Shared.Core.Entities.PeFile> PeFiles { get; set; }
        public DbSet<MotherPca> MotherPcas { get; set; }
        public DbSet<RateCard> RateCards { get; set; }
        public DbSet<TypeOfExpense> TypeOfExpenses { get; set; }
        public DbSet<JobDetail> JobDetail { get; set; }
        public DbSet<ClientBrief> ClientBrief { get; set; }
        public DbSet<PmTeam> PmTeams { get; set; }
        public DbSet<PmTeamMember> PmTeamMembers { get; set; }

        //Transaksi
        public DbSet<Pce> Pces { get; set; }
        public DbSet<PceTask> PceTasks { get; set; }
        public DbSet<Pca> Pcas { get; set; }
        public DbSet<PcaTask> PcaTasks { get; set; }

        public DbSet<TimeSheetHeader> TimeSheetHeaders { get; set; }

        public DbSet<TimeSheetDetail> timeSheetDetails { get; set; }
        public DbSet<Phoenix.Shared.Core.Entities.PeFileFolder> peFileFolders { get; set; }
        public DbSet<Phoenix.Shared.Core.Entities.DamFolderMaster> DamFolderMasters { get; set; }
        public DbSet<JobStatus> PmJobStatus { get; set; }
        public DbSet<Channel> Channel { get; set; }
        public DbSet<MasterNonJob> MasterNonjob { get; set; }
        public DbSet<MasterDayTimesheetUserLocked> MasterDayTimesheetUserLocked { get; set; }
        public DbSet<CampaignType> CampaignType { get; set; }
        public DbSet<BusinessType> BusinessType { get; set; }
        public DbSet<JobType> JobType { get; set; }
        public DbSet<Phoenix.Data.Models.MyDam> MyDam { get; set; }
        public DbSet<MyDamApproval> MyDamApproval { get; set; }

        #endregion

        #region Accounting
        public DbSet<JournalAdjustment> JournalAdjustments { get; set; }
        public DbSet<JournalAdjustmentDetail> JournalAdjustmentDetails { get; set; }
        public DbSet<JournalEntry> JournalEntrys { get; set; }
        public DbSet<JournalEntryDetail> JournalEntryDetails { get; set; }
        public DbSet<Amortization> Amortizations { get; set; }
        public DbSet<AmortizationDetail> AmortizationDetails { get; set; }
        public DbSet<SalaryAllocation> SalaryAllocations { get; set; }
        public DbSet<SalaryAllocationDetail> SalaryAllocationDetails { get; set; }
        #endregion

        #region View Model
        public DbSet<ExecJournalAPView> ExecJournalAPView { get; set; }
        public DbSet<CoaInvoice> CoaInvoice { get; set; }
        public DbSet<CoaInvoiceTaxpayable> CoaInvoiceTaxpayable { get; set; }
        public DbSet<CoaInvoiceClient> CoaInvoiceClients { get; set; }
        //public DbSet<GeneralLedgerDetail> GeneralLedgerDetails { get; set; }
        public DbSet<MappingBusinessUnit> MappingBusinessUnit { get; set; }
        public DbSet<VwJobClosingAp> VwJobClosingAp { get; set; }
        public DbSet<VwJobClosingAr> VwJobClosingAr { get; set; }
        public DbSet<VwjournalAp> VwjournalAp { get; set; }
        public DbSet<VwJournalApDetail> VwJournalApDetail { get; set; }
        public DbSet<VwStatementOfAr> VwStatementOfAr { get; set; }
        public DbSet<VwStatementOfAp> VwStatementOfAp { get; set; }
        public DbSet<VwAgingAp> VwAgingAp { get; set; }
        public DbSet<VwAgingAr> VwAgingAr { get; set; }
        #endregion
    }
}
