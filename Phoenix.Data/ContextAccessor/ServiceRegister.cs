using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Phoenix.Data;
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data.Services;
using Phoenix.Data.Services.Media.MasterData.MediaOrderStatus;
using Phoenix.Data.Services.Media.MasterData.MediaPlanStatus;
using Phoenix.Data.Services.Media;
using Phoenix.Data.Services.Media.MasterData.ProgramPosition;
using Phoenix.Data.Services.Media.MasterData.ProgramType;
using Phoenix.Data.Services.Media.Transaction;
using Phoenix.Data.Services.Um;
using System;
using System.Collections.Generic;
using System.Text;
using Phoenix.ApiExtension.Extensions.GenericExcelExport;
using Phoenix.Data.Services.Finance.Transaction;
using Phoenix.Data.Services.Finance;
using Phoenix.Data.Services.Finance.Report;
using Phoenix.Data.Services.Pm.Report;
using Phoenix.Data.Services.HrisNew.MasterData.Training;

using Phoenix.Data.Services.Pm.MasterData;
using Phoenix.Data.Services.HrisNew.MasterData;
using Phoenix.Data.Services.HrisNew.MasterData.MasterChecklistCategory;
using Phoenix.Data.Services.Pm.Transaction;

namespace Phoenix.Data.ContextAccessor
{
    public static class ServiceRegister
    {
        public static void RegisterPhoenixDependencies(this IServiceCollection services)
        {

            #region Global
            services.AddScoped<IDropdownService, DropdownService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IRequestorService, RequestorService>();
            services.AddScoped<IHolidayService, HolidayService>();
            services.AddScoped<IStatusLogService, StatusLogService>();

            services.AddTransient<StatusLogService>();
            services.AddTransient<FileService>();
            #endregion

            #region HRIS
            #region Master Data
            services.AddScoped<IMasterChecklistCategoryService, MasterChecklistCategoryService>();
            //Payroll
            services.AddScoped<ISalaryMainComponentService, SalaryMainComponentService>();
            services.AddScoped<ISalaryStandardService, SalaryStandardService>();
            services.AddScoped<ILoanCategoryService, LoanCategoryService>();
            services.AddScoped<IPtkpSettingService, PtkpSettingService>();
            services.AddScoped<IMasterCutoffService, MasterCutoffService>();
            services.AddScoped<IBpjsComponentService, BpjsComponentService>();
            services.AddScoped<IAllowanceComponentService, AllowanceComponentService>();
            services.AddScoped<IDeductionComponentService, DeductionComponentService>();
            services.AddScoped<IInsuranceCompanyService, InsuranceCompanyService>();
            services.AddScoped<IEmployeePayrollGroupService, EmployeePayrollGroupService>();
            services.AddScoped<IDeclarationPayrollService, DeclarationPayrollService>();
            //Termination
            services.AddScoped<ITmTerminationTypeService, TmTerminationTypeService>();
            services.AddScoped<IFaultCategoryService, FaultCategoryService>();
            services.AddScoped<ICategoryExitInterviewService, CategoryExitInterviewService>();
            services.AddScoped<IExitInterviewItemService, ExitInterviewItemService>();

            //Employee Self Service
            services.AddScoped<IMasterDivisionSelfserviceService, MasterDivisionSelfserviceService>();
            services.AddScoped<IMasterMappingRequestService, MasterMappingRequestService>();

            //Leave
            services.AddScoped<ILeaveRequestTypeService, LeaveRequestTypeService>();

            //Legal
            services.AddScoped<ILegalCategoryService, LegalCategoryService>();
            services.AddScoped<ILegalDocumentMasterCounterpartService, LegalDocumentMasterCounterpartService>();

            //PDR
            services.AddScoped<IPdrCategoryService, PdrCategoryService>();
            services.AddScoped<IPdrParameterService, PdrParameterService>();
            services.AddScoped<IPdrGradeService, PdrGradeService>();
            services.AddScoped<IPdrSalaryIncreaseService, PdrSalaryIncreaseService>();

            //PDR Appraisal
            services.AddScoped<IBscCorporateObjectiveService, BscCorporateObjectiveService>();
            services.AddScoped<IPdrObjectiveSettingService, PdrObjectiveSettingService>();
            services.AddScoped<IPDRObjectiveTargetService, PDRObjectiveTargetService>();

            services.AddScoped<IMasterTrainingRequestService, MasterTrainingRequestService>();
            services.AddScoped<IBusinessUnitJobLevelService, BusinessUnitJobLevelService>();
            services.AddScoped<IInstitutionService, InstitutionService>();
            #endregion

            #region Transaction
            //Termination
            services.AddScoped<IResignationService, ResignationService>();
            services.AddScoped<IContractService, ContractService>();
            services.AddScoped<IDeathService, DeathService>();
            services.AddScoped<ISanctionService, SanctionService>();
            services.AddScoped<IRetiredService, RetiredService>();
            services.AddScoped<IHCProcessTerminationService, HCProcessTerminationService>();
            services.AddScoped<IExitChecklistService, ExitChecklistService>();
            services.AddScoped<IExitInterviewService, ExitInterviewService>();

            //Employee Self Service
            services.AddScoped<IServiceRequestService, ServiceRequestService>();
            services.AddScoped<IFullfillmentService, FullfillmentService>();
            services.AddScoped<ICommentService, CommentService>();

            //Attendance
            services.AddScoped<IMasterWorkingTimeService, MasterWorkingTimeService>();
            services.AddScoped<IEmployeeLocationService, EmployeeLocationService>();
            services.AddScoped<IMasterLocationService, MasterLocationService>();
            services.AddScoped<IAttendanceService, AttendanceService>();

            //Leave
            services.AddScoped<ILeaveService, LeaveService>();
            services.AddScoped<ILeaveBalanceService, LeaveBalanceService>();
            services.AddScoped<IMassLeaveService, MassLeaveService>();
            services.AddScoped<IManageLeaveBalanceService, ManageLeaveBalanceService>();

            //Legal
            services.AddScoped<ILegalDocumentRequestService, LegalDocumentRequestService>();
            services.AddScoped<ILegalDocumentRequestFullfilmentService, LegalDocumentRequestFullfilmentService>();
            services.AddScoped<ILegalDocumentService, LegalDocumentService>();
            services.AddScoped<ILegalSharingRequestService, LegalSharingRequestService>();

            //Overtime
            services.AddScoped<IOvertimeService, OvertimeService>();

            //Payroll
            services.AddScoped<IEmployeeLoanService, EmployeeLoanService>();
            services.AddScoped<IEmployeePayrollProfileService, EmployeePayrollProfileService>();
            services.AddScoped<IEmployeeHealthInsuranceService, EmployeeHealthInsuranceService>();
            services.AddScoped<IPayrollService, PayrollService>();

            //Promotion
            services.AddScoped<IPromotionService, PromotionService>();

            //Rotation
            services.AddScoped<IRotationService, RotationService>();

            //Demotion
            services.AddScoped<IDemotionService, DemotionService>();

            //PDR
            services.AddScoped<IPdrPerformanceService, PdrPerformanceService>();

            //PDR Appraisal
            services.AddScoped<IPDRApprailsalService, PDRApprailsalService>();

            services.AddScoped<IPdrM1FinalService, PdrM1FinalService>();


            //Training
            services.AddScoped<ITrainingService, TrainingService>();
            services.AddScoped<ITrainingRequisitionService, TrainingRequisitionService>();
            services.AddScoped<ITrainingRequisitionDetailService, TrainingRequisitionDetailService>();
            services.AddScoped<ITrainingCategoryService, TrainingCategoryService>();
            services.AddScoped<ITrainingSubjectService, TrainingSubjectService>();
            services.AddScoped<ITrainingTypeService, TrainingTypeService>();
            services.AddScoped<ITrainingBondingService, TrainingBondingService>();
            services.AddScoped<ITrainingRegistrationPaymentReqService, TrainingRegistrationPaymentReqService>();
            services.AddScoped<ITrainingShareSessionService, TrainingShareSessionService>();

            services.AddScoped<IEmployeeBasicInfoService, EmployeeBasicInfoService>();
            services.AddScoped<IFamilyService, FamilyService>();
            services.AddScoped<IEducationService, EducationService>();
            services.AddScoped<IEmployeeEmergencyContactService, EmployeeEmergencyContactService>();
            services.AddScoped<IEmployeeExperienceService, EmployeeExperienceService>();
            services.AddScoped<IEmployeeTrainingService, EmployeeTrainingService>();
            services.AddScoped<IEmployeeChecklistService, EmployeeChecklistService>();

            #endregion

            #region Report
            services.AddScoped<IPayrollCompareableService, PayrollCompareableService>();
            services.AddScoped<ISummaryPayrollService, SummaryPayrollService>();
            #endregion
            #endregion

            #region UM

            services.AddScoped<IManageMenuService, ManageMenuService>();
            services.AddScoped<IManageGroupAccessService, ManageGroupAccessService>();
            services.AddScoped<IManageUserAccessService, ManageUserAccessService>();
            services.AddScoped<IManageUserAppService, ManageUserAppService>();
            services.AddScoped<IManageGroupService, ManageGroupService>();
            services.AddScoped<IAuditTrailsService, AuditTrailsService>();
            services.AddScoped<IDoaService, DoaService>();
            services.AddScoped<IGroupChattingService, GroupChattingService>();
            services.AddScoped<IGroupChattingMemberService, GroupChattingMemberService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationTypeService, NotificationTypeService>();

            services.AddScoped<IManageTemplateApprovalService, ManageTemplateApprovalService>();
            services.AddScoped<IManageUserApprovalService, ManageUserApprovalService>();
            services.AddScoped<IManageConditionalApprovalService, ManageConditionalApprovalService>();
            services.AddScoped<IManageTransactionApprovalService, ManageTransactionApprovalService>();
            services.AddScoped<ITransactionUserApprovalService, TransactionUserApprovalService>();
            services.AddScoped<ITransactionConditionalApprovalService, TransactionConditionalApprovalService>();
            services.AddScoped<IBudgetManagementService, BudgetManagementService>();
            services.AddScoped<IAbstractDataExport, AbstractDataExportBridge>();
            services.AddScoped<IJobDetailService, JobDetailService>();
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            services.AddTransient<Data.Attributes.GlobalFunctionApproval>();
            services.AddScoped<IManageAzurePortalSetupService, ManageAzurePortalSetupService>();
            services.AddScoped<ISendEmailMsGraph, SendEmailMsGraph>();
            services.AddScoped<IMappingFolderOneDriveService, MappingFolderOneDriveService>();
            services.AddScoped<IManageProfileUserService, ManageProfileUserService>();

            services.AddTransient<ManageTransactionApprovalService>();
            #endregion

            #region Media
            services.AddScoped<IUploadMasterTemplateService, UploadMasterTemplateService>();
            services.AddScoped<IMediaPlanTVService, MediaPlanTVService>();
            services.AddScoped<IMediaPlanTVApprovalService, MediaPlanTVApprovalService>();
            services.AddScoped<IMediaPlanStatusService, MediaPlanStatusService>();
            services.AddScoped<IMediaOrderTVService, MediaOrderTVService>();
            services.AddScoped<IMediaOrderStatusService, MediaOrderStatusService>();
            services.AddScoped<ITmMediaTypeService, TmMediaServiceService>();
            services.AddScoped<IMediaPlanRequestTypeService, MediaPlanRequestTypeService>();
            services.AddScoped<IProgramPositionService, ProgramPositionService>();
            services.AddScoped<IProgramTypeService, ProgramTypeService>();
            services.AddScoped<IProgramCategoryTypeService, ProgramCategoryTypeService>();
            services.AddScoped<IMediaPlanEstimatesService, MediaPlanEstimatesService>();
            services.AddScoped<IMediaOrderService, MediaOrderService>();
            #endregion

            #region Finance
            #region Master Data
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IAffiliationService, AffiliationService>();
            services.AddScoped<IEFakturService, EFakturService>();
            services.AddScoped<ICashFlowService, CashFlowService>();
            services.AddScoped<IBusinessUnitService, BusinessUnitService>();
            services.AddScoped<IChartofaccountReportService, ChartofaccountReportService>();
            services.AddScoped<IMainServiceCategoryService, MainServiceCategoryService>();
            services.AddScoped<IShareServicesService, ShareServicesService>();
            services.AddScoped<IBankService, BankService>();
            services.AddScoped<IGetFromService, GetFromService>();
            services.AddScoped<IItemTypeService, ItemTypeService>();
            services.AddScoped<ICategoryFixedAssetService, CategoryFixedAssetService>();
            //services.AddScoped<IPaymentMethodService, PaymentMethodService>();
            services.AddScoped<IPaymentTypeService, PaymentTypeService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IProvinceService, ProvinceService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICompanyBrandService, CompanyBrandService>();
            services.AddScoped<ITypeOfExpenseService, TypeOfExpenseService>();
            services.AddScoped<IMasterBudgetCodeService, MasterBudgetCodeService>();
            services.AddScoped<ICostSharingAllocationService, CostSharingAllocationService>();
            services.AddScoped<ILegalEntityService, LegalEntityService>();
            services.AddScoped<IMasterOutletService, MasterOutletService>();
            services.AddScoped<IMasterPettyCashService, MasterPettyCashService>();
            services.AddScoped<IMasterMonthlyBudgetService, MasterMonthlyBudgetService>();
            services.AddScoped<IMasterMonthlyBudgetSpreadService, MasterMonthlyBudgetSpreadService>();
            services.AddScoped<IBudgetMasterService, BudgetMasterService>();
            services.AddScoped<IFinReportControlService, FinReportControlService>();
            services.AddScoped<IMasterOpeningBalanceService, MasterOpeningBalanceService>();
            #endregion

            services.AddScoped<IMasterLocationProcurementService, MasterLocationProcurementService>();
            services.AddScoped<IMasterConditionProcurementService, MasterConditionProcurementService>();
            services.AddScoped<IMasterDepreciationService, MasterDepreciationService>();
            services.AddScoped<IFixedAssetService, FixedAssetService>();
            services.AddScoped<IFixedAssetMovementService, FixedAssetMovementService>();
            services.AddScoped<IFixedAssetDepreciationService, FixedAssetDepreciationService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IMappingJurnalService, MappingJurnalService>();
            //services.AddScoped<IInvoiceReceivedService, InvoiceReceivedService>();
            services.AddScoped<IInvoiceReceivableService, InvoiceReceivableService>();
            services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
            //services.AddScoped<ITemporaryAccountPayableService, TemporaryAccountPayableService>();
            //services.AddScoped<IAccountPayableService, AccountPayableService>();
            services.AddScoped<ICashAdvanceService, CashAdvanceService>();
            services.AddScoped<ICashAdvanceSattlementService, CashAdvanceSattlementService>();
            services.AddScoped<ICASettlementJournalReverseService, CASettlementJournalReverseService>();
            services.AddScoped<IInventoryGoodsReceiptService, InventoryGoodsReceiptService>();
            services.AddScoped<IGoodRequestService, GoodRequestService>();
            services.AddScoped<IServiceReceiptService, ServiceReceiptService>();
            services.AddScoped<IPettyCashService, PettyCashService>();
            //services.AddScoped<IBudgetService, BudgetService>();
            services.AddScoped<IInvoiceClientService, InvoiceClientService>();
            services.AddScoped<IAccountReceiveableService, AccountReceiveableService>();
            services.AddScoped<IReconcileService, ReconcileService>();
            services.AddScoped<IRequestForQuotationService, RequestForQuotationService>();
            services.AddScoped<IPurchaseRequestService, PurchaseRequestService>();
            services.AddScoped<IPaymentRequestService, PaymentRequestService>();
            services.AddScoped<IBankPaymentService, BankPaymentService>();
            services.AddScoped<IInvoiceClientMultiPceService, InvoiceClientMultiPceService>();
            services.AddScoped<IBankReceivedService, BankReceivedService>();
            services.AddScoped<IChartOfAccountService, ChartOfAccountService>();
            services.AddScoped<IFinancePeriodService, FinancePeriodService>();
            services.AddScoped<IFinancialYearService, FinancialYearService>();
            services.AddScoped<IJobClosingService, JobClosingService>();
            services.AddScoped<IOrderReceiptService, OrderReceiptService>();
            services.AddScoped<IBastService, BastService>();
            services.AddScoped<IMapCoaTransaksiService, MapCoaTransaksiService>();
            services.AddScoped<ITImeSheetReportServices, TImeSheetReportServices>();
            services.AddTransient<FinancePeriodService>();
            services.AddScoped<IInvoiceDeliveryNotesService, InvoiceDeliveryNotesService>();
            services.AddScoped<IGoodsReturnServices, InventoryGoodsReturnServices>();
            services.AddScoped<IJobCardService, JobCardService>();
            services.AddScoped<IRevaluationListService, RevaluationListService>();
            services.AddScoped<ISalaryPaymentRequestService, SalaryPaymentRequestService>();
            services.AddScoped<IFixedAssetOutMovementService, FixedAssetOutMovementService>();
            #region AR
            //services.AddScoped<IInvoiceReceivableService, InvoiceReceivableService>();
            #endregion

            #region Report
            services.AddScoped<IRptStatementAPService, RptStatementAPService>();
            services.AddScoped<IRptStatementARService, RptStatementARService>();
            services.AddScoped<IRptProfitAndLossService, RptProfitAndLossService>();
            services.AddScoped<IRptBillingAndRevenueService, RptBillingAndRevenueService>();
            services.AddScoped<IAccountPayableAgingService, AccountPayableAgingService>();
            services.AddScoped<IAccountReceivableAgingService, AccountReceivableAgingService>();
            services.AddScoped<IRptTrialBalanceService, RptTrialBalanceService>();
            services.AddScoped<IRptBalanceSheetService, RptBalanceSheetService>();
            services.AddScoped<IRptBudgetService, RptBudgetService>();
            services.AddScoped<IRptBudgetBalanceService, RptBudgetBalanceService>();
            services.AddScoped<IRptPreFinancingService, RptPreFinancingService>();
            services.AddScoped<IRptInterCompanyReconciliationService, RptInterCompanyReconciliationService>();
            services.AddScoped<IRptExpensesAffiliationService, RptExpensesAffiliationService>();
            services.AddScoped<IRptCashFlowService, RptCashFlowService>();
            #endregion
            #endregion

            #region Pm
            services.AddScoped<IPceService, PceService>();
            services.AddScoped<IPcaService, PcaService>();
            services.AddScoped<IPMTeamService, PMTeamService>();
            services.AddScoped<IRateCardService, RateCardService>();
            services.AddScoped<IMotherPCAService, MotherPCAService>();
            services.AddScoped<IPceReportServices, PceReportServices>();
            services.AddScoped<IMasterJobStatusService, MasterJobStatusService>();
            services.AddScoped<IMasterNonJobService, MasterNonJobService>();
            services.AddScoped<IMasterDayTimesheetUserLockedService, MasterDayTimesheetUserLockedService>();
            services.AddScoped<IMasterCampaignTypeService, MasterCampaignTypeService>();
            services.AddScoped<IMasterPmBusinessTypeService, MasterPmBusinessTypeService>();
            services.AddScoped<IMasterJobTypeService, MasterJobTypeService>();
            services.AddScoped<IDamApprovalService, DamApprovalService>();
            services.AddScoped<IMyDamService, MyDamService>();
            services.AddScoped<IShowTaskServices, ShowTaskServices>();

            #endregion

            #region Accounting
            services.AddScoped<IGeneralLedgerDetailService, GeneralLedgerDetailService>();
            services.AddScoped<IJournalEntryService, JournalEntryService>();
            services.AddScoped<IJournalAdjustmentService, JournalAdjustmentService>();
            services.AddScoped<IAmortizationService, AmortizationService>();
            services.AddScoped<ISalaryAllocationService, SalaryAllocationService>();

            #endregion
        }
    }
}
