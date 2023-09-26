using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.WebExtension.RestApi
{
    public static class ApiUrl
    {
        private static string Url
        {
            get
            {
                var url = DataConfiguration.Configuration.AppConfiguration.ApiServerUrl;
                if (url.EndsWith("/"))
                    return url.Substring(0, url.Length - 1);

                return url;
            }
        }

        #region Global
        public static string StatusLogUrl => $"{Url}/statuslog";

        //Global Hiris 
        public static string dropdownUrl => $"{Url}/dropdown";
        public static string DownloadFileUrl => $"{Url}/downloadFile";
        public static string EmployeeInfoUrl => $"{Url}/employeeInfo";
        public static string HolidayUrl => $"{Url}/holiday";

        //Global Finance 
        public static string financedropdownUrl => $"{Url}/financedropdown";
        public static string ChartOfAccountUrl => $"{Url}/chartofaccount";

        #endregion

        #region HRIS
        #region Master Data
        public static string MasterChecklistCategory => $"{Url}/MasterChecklistCategory";
        //Payroll 
        public static string SalaryMainComponentUrl => $"{Url}/salarymaincomponent";
        public static string SalaryStandardUrl => $"{Url}/salarystandard";
        public static string LoanCategoryUrl => $"{Url}/loancategory";
        public static string PtkpSettingUrl => $"{Url}/ptkpsetting";
        public static string MasterCutoffUrl => $"{Url}/mastercutoff";
        public static string BpjsComponentUrl => $"{Url}/bpjscomponent";
        public static string AllowanceComponentUrl => $"{Url}/allowancecomponent";
        public static string DeductionComponentUrl => $"{Url}/deductioncomponent";
        public static string InsuranceCompanyUrl => $"{Url}/insurancecompany";
        public static string EmployeePayrollGroupUrl => $"{Url}/employeepayrollgroup";

        //Termination
        public static string TmTerminationTypeUrl => $"{Url}/tmterminationtype";
        public static string FaultCategoryUrl => $"{Url}/faultcategory";
        public static string CategoryExitInterviewUrl => $"{Url}/categoryexitinterview";
        public static string ExitInterviewItemUrl => $"{Url}/exitinterviewitem";

        //Leave
        public static string LeaveRequestTypeUrl => $"{Url}/leaverequesttype";

        //legal
        public static string LegalCategoryUrl => $"{Url}/legalcategory";
        public static string LegalDocumentMasterCounterpartUrl => $"{Url}/legaldocumentmastercounterpart";

        //PDR
        public static string PdrCategoryUrl => $"{Url}/pdrcategory";
        public static string PdrParameterUrl => $"{Url}/pdrparameter";
        public static string PdrGradeUrl => $"{Url}/pdrgrade";
        public static string PdrSalaryIncreaseUrl => $"{Url}/pdrsalaryincrease";


        //PDR Apprailsal
        public static string BscCorporateObjectiveUrl => $"{Url}/bsccorporateobjective";
        public static string PdrObjectiveSettingUrl => $"{Url}/pdrobjectivesetting";
        public static string PdrObjectiveTargetUrl => $"{Url}/pdrobjectivetarget";

        public static string MasterTrainingRequestUrl => $"{Url}/mastertrainingrequest";
        public static string InstitutionUrl => $"{Url}/institution";
        #endregion

        #region Transaction
        //Termination
        public static string ContractUrl => $"{Url}/contract";
        public static string DeathUrl => $"{Url}/death";
        public static string ResignationUrl => $"{Url}/resignation";
        public static string RetiredUrl => $"{Url}/retired";
        public static string SanctionUrl => $"{Url}/sanction";
        public static string HcProcessTerminationUrl => $"{Url}/hcprocesstermination";
        public static string ExitChecklistnUrl => $"{Url}/exitchecklist";
        public static string ExitInterviewnUrl => $"{Url}/exitinterview";

        //Employee Self Service
        public static string MasterDivisionSelfserviceUrl => $"{Url}/masterdivisionselfservice";
        public static string MasterMappingRequestUrl => $"{Url}/mastermappingrequest";
        public static string ServiceRequestUrl => $"{Url}/servicerequest";
        public static string FullfillmentUrl => $"{Url}/fullfillment";
        public static string CommentUrl => $"{Url}/comment";

        //Attendance
        public static string MasterWorkingTimeUrl => $"{Url}/masterworkingtime";
        public static string AttendanceUrl => $"{Url}/attendance";
        public static string EmployeeLocationUrl => $"{Url}/employeelocation";
        public static string MasterLocationUrl => $"{Url}/masterlocation";
        public static string EmployeeWorkingTimeUrl => $"{Url}/employeeworkingtime";

        //Leave
        public static string LeaveBalanceUrl => $"{Url}/leavebalance";
        public static string LeaveUrl => $"{Url}/leave";
        public static string MassLeaveUrl => $"{Url}/massleave";
        public static string ManageLeaveBalanceUrl => $"{Url}/manageleavebalance";

        //legal
        public static string LegalDocumentRequestUrl => $"{Url}/legaldocumentrequest";
        public static string LegalDocumentRequestFullfilmentUrl => $"{Url}/legaldocumentrequestfullfilment";
        public static string LegalDocumentUrl => $"{Url}/legaldocument";
        public static string LegalSharingRequestUrl => $"{Url}/legalsharingrequest";

        //Overtime
        public static string OvertimeUrl => $"{Url}/overtime";

        //Payroll
        public static string EmployeeLoanUrl => $"{Url}/employeeloan";
        public static string EmployeePayrollProfileUrl => $"{Url}/employeepayrollprofile";
        public static string EmployeeHealthInsuranceUrl => $"{Url}/employeehealthinsurance";
        public static string PayrollUrl => $"{Url}/payroll";
        public static string DeclarationPayrollUrl => $"{Url}/declarationpayroll";

        //Promotion
        public static string PromotionUrl => $"{Url}/promotion";

        //Rotation
        public static string RotationUrl => $"{Url}/rotation";

        //Demotion
        public static string DemotionUrl => $"{Url}/demotion";

        //PDR
        public static string PdrPerformanceUrl => $"{Url}/pdrperformance";

        //PDR Apprailsal
        public static string PdrApprailsalUrl => $"{Url}/pdrapprailsal";

        //PDR M1 Final Score
        public static string PdrM1FinalScoreUrl => $"{Url}/pdrm1finalscore";

        //Training
        public static string Training => $"{Url}/training";

        public static string TrainingRegistrationPaymentReqUrl => $"{Url}/TrainingRegistrationPaymentReq";
        public static string TrainingShareSessionUrl => $"{Url}/TrainingShareSession";


        //Reqruitment
        public static string EmployeeBasic => $"{Url}/EmployeeBasic";
        public static string Family => $"{Url}/Family";
        public static string EmployeeEmergencyContact => $"{Url}/employeeemergencycontact";
        public static string Education => $"{Url}/education";
        public static string EmployeeExperience => $"{Url}/employeeexperience";
        public static string BusinessUnitJobLevel => $"{Url}/businessunitjoblevel";
        public static string EmployeeTraining => $"{Url}/employeetraining";
        public static string EmployeeChecklist => $"{Url}/employeechecklist";
        #endregion

        #region Report
        public static string PayrollCompareableUrl => $"{Url}/payrollcompareable";
        public static string SummaryPayrollUrl => $"{Url}/summarypayroll";
        #endregion
        #endregion

        #region UM
        public static string TransactionLogUrl => $"{Url}/general/transactionlog";
        public static string MenuPrivilegeUrl => $"{Url}/general/menu";
        public static string MenuAuthorizeUrl => $"{Url}/general/menu/auth";
        public static string EmailTemplateUrl => $"{Url}/emailtemplate";
        #endregion

        #region Finance
        #region Master Data
        public static string CurrencyUrl => $"{Url}/currency";
        public static string OrganizationUrl => $"{Url}/organization";
        public static string AffiliationUrl => $"{Url}/affiliation";
        public static string CashFlowUrl => $"{Url}/cashFlow";
        public static string BusinessUnitUrl => $"{Url}/businessunit";
        public static string ChartofaccountReportUrl => $"{Url}/chartofaccountreport";
        public static string MainServiceCategoryUrl => $"{Url}/mainservicecategory";
        public static string ShareServicesUrl => $"{Url}/shareservices";
        public static string BankUrl => $"{Url}/bank";
        public static string GetFromUrl => $"{Url}/getfrom";
        public static string ItemTypeUrl => $"{Url}/itemtype";
        public static string CategoryFixedAssetUrl => $"{Url}/categoryfixedasset";
        //public static string PaymentMethodUrl => $"{Url}/paymentmethod";
        public static string PaymentTypeUrl => $"{Url}/paymenttype";
        public static string CountryUrl => $"{Url}/country";
        public static string ProvinceUrl => $"{Url}/province";
        public static string CityUrl => $"{Url}/city";
        public static string MasterClientUrl => $"{Url}/company/client";
        public static string MasterVendorUrl => $"{Url}/company/vendor";
        public static string MasterClientBrandUrl => $"{Url}/companybrand/client";
        public static string MasterVendorBrandUrl => $"{Url}/companybrand/vendor";
        public static string TypeOfExpenseUrl => $"{Url}/typeofexpense";
        public static string MasterBudgetOpexUrl => $"{Url}/masterbudgetopex";
        public static string MasterBudgetCapexUrl => $"{Url}/masterbudgetcapex";
        public static string CostSharingAllocationUrl => $"{Url}/costsharingallocation";
        public static string LegalEntityUrl => $"{Url}/legalentity";
        public static string MasterOutletUrl => $"{Url}/masteroutlet";
        public static string MasterPettyCashUrl => $"{Url}/masterpettycash";
        public static string MapCoaTransaksiUrl => $"{Url}/mapcoatransaksi";
        public static string MasterAllocationRatioUrl => $"{Url}/masterallocationratio";
        public static string MasterMonthlyBudgetSpreadUrl => $"{Url}/mastermonthlybudgetspread";
        public static string BudgetOpexUrl => $"{Url}/budgetopex";
        public static string BudgetCapexUrl => $"{Url}/budgetcapex";
        public static string FinReportControlUrl => $"{Url}/finreportcontrol";
        public static string MasterOpeningBalanceUrl => $"{Url}/masteropeningbalance";
        #endregion
        //public static string TemporaryAccountPayable => $"{Url}/temporaryaccountpayable";
        public static string InvoiceReceived => $"{Url}/InvoiceReceived";
        //public static string AccountPayableUrl => $"{Url}/accountpayable";
        public static string CashAdvanceUrl => $"{Url}/cashadvance";
        public static string CashAdvanceSattleUrl => $"{Url}/cashadvancesattlement";
        public static string CashAdvanceSattleReverseUrl => $"{Url}/casjournalreverse";
        public static string PaymentRequestUrl => $"{Url}/paymentrequest";
        public static string InvoiceReceivable => $"{Url}/InvoiceReceivable";
        public static string PettyCashSettlement => $"{Url}/pettycashsettlement";
        public static string GeneralAdjustment => $"{Url}/generaladjustment";
        public static string F19DiageoPCE => $"{Url}/f19diageopce";
        public static string AccountPayableAging => $"{Url}/accountpayableaging";
        public static string AccountReceivableAging => $"{Url}/accountreceivableaging";
        public static string MappingJurnalUrl => $"{Url}/mappingjurnal";
        public static string PurchaseOrderUrl => $"{Url}/PurchaseOrder";
        public static string PettyCashUrl => $"{Url}/pettycash";
        public static string InvoiceClientUrl => $"{Url}/invoiceclient";
        public static string InvoiceClientMultipleUrl => $"{Url}/invoiceclientmultipce";
        public static string ARClientUrl => $"{Url}/accountreceivable";
        public static string ReconcileUrl => $"{Url}/Reconcile";
        public static string RequestForQuotationUrl => $"{Url}/RequestForQuotation";
        public static string VendorQuotation => $"{Url}/VendorQuotation";
        public static string PurchaseRequestUrl => $"{Url}/purchaserequest";
        public static string BankPaymentUrl => $"{Url}/bankpayment";
        public static string BankReceivedUrl => $"{Url}/bankreceived";
        public static string InventoryGoodsReceipt => $"{Url}/inventorygoodsreceipt";
        public static string GoodReturn => $"{Url}/goodsreturn";
        public static string GoodRequest => $"{Url}/GoodRequest";
        public static string ServiceReceipt => $"{Url}/servicereceipt";
        public static string FinancePeriodUrl => $"{Url}/financeperiod";
        public static string FinancialYearUrl => $"{Url}/financialyear";
        public static string MasterLocationProcurementUrl => $"{Url}/masterlocationprocurement";
        public static string MasterConditionProcurementUrl => $"{Url}/masterconditionprocurement";
        public static string MasterDepreciationUrl => $"{Url}/masterdepreciation";
        public static string FixedAssetUrl => $"{Url}/fixedasset";
        public static string FixedAssetMovementUrl => $"{Url}/fixedassetmovement";
        public static string FixedAssetDepreciationtUrl => $"{Url}/fixedassetdepreciation";
        public static string Inventory => $"{Url}/inventory";
        public static string JobClosingUrl => $"{Url}/jobclosing";
        public static string OrderReceipt => $"{Url}/orderreceipt";
        public static string BAST => $"{Url}/bast";
        public static string TimeSheetByJobUrl => $"{Url}/timesheetgridreport";
        public static string InvoiceDeliveryNotes => $"{Url}/invoicedeliverynotes";
        public static string jobCardUrl => $"{Url}/jobcard";
        public static string RevaluationListUrl => $"{Url}/revaluationlist";
        public static string SalaryPaymentRequestUrl => $"{Url}/salarypaymentrequest";
        public static string FixedAssetOutMovement => $"{Url}/FixedAssetOutMovement";
        public static string EFakturUrl => $"{Url}/efaktur";

        #region Report
        public static string RptStatementAP => $"{Url}/RptStatementAP";
        public static string RptStatementAR => $"{Url}/RptStatementAR";
        public static string RptProfitAndLossUrl => $"{Url}/rptprofitandloss";
        public static string RptBillingAndRevenue => $"{Url}/rptbillingandrevenue";
        public static string RptTrialBalanceUrl => $"{Url}/rpttrialbalance";
        public static string RptBalanceSheet => $"{Url}/rptbalancesheet";
        public static string RptBudget => $"{Url}/rptbudget";
        public static string RptBudgetBalance => $"{Url}/rptbudgetbalance";
        public static string RptPreFinancing => $"{Url}/rptprefinancing";
        public static string RptInterCompanyReconciliation => $"{Url}/rptintercompanyreconciliation";
        public static string RptExpensesAffiliation => $"{Url}/rptexpensesaffiliation";
        public static string RptCashFlowUrl => $"{Url}/rptcashflow";
        #endregion

        #endregion

        #region Media
        public static string UploadMasterTemplateUrl => $"{Url}/uploadmastertemplate";
        public static string MediaPlanTV => $"{Url}/mediaplantv";
        public static string MediaPlanTVApproval => $"{Url}/mediaplantvapproval";
        public static string MediaPlanStatus => $"{Url}/mediaplanstatus";
        public static string MediaOrderStatus => $"{Url}/mediaorderstatus";
        public static string ProgramPosition => $"{Url}/programposition";
        public static string ProgramType => $"{Url}/programtype";
        public static string MediaTypeUrl => $"{Url}/mastermediatype";
        public static string MediaPlanRequestType => $"{Url}/mediaplanrequesttype";
        public static string ProgramCategoryTypeUrl => $"{Url}/programcategorytype";
        public static string MediaOrderTVurl => $"{Url}/MediaOrderTV";
        public static string DownloadExcelUrl => $"{Url}/ExcelExport";

        public static string ReadExcelMediaPlanUrl => $"{Url}/MediaPlanTV/ReadExcel";
        public static string BugetUrl => $"{Url}/finance/Budget";
        public static string MasterFinancialYearURL => $"{Url}/finance/Budget/GetMasterFinancialYear";
        public static string MediaPlanEstimatesURL => $"{Url}/mediaplanestimates";
        public static string MediaOrderURL => $"{Url}/mediaorder";
        #endregion

        #region Approval
        public static string ManageTransApproval => $"{Url}/ManageTransApproval";
        public static string ManageUserUrl => $"{Url}/ManageUser";
        #endregion

        #region Pm
        public static string PceUrl => $"{Url}/pm/pce";
        public static string PcaUrl => $"{Url}/pm/pca";
        public static string TimeSheetUrl => $"{Url}/ProjectManagement/TimesheetUser";
        public static string JobDetail => $"{Url}/pm/JobDetail";
        public static string PMTeamUrl => $"{Url}/pm/pmteam";
        public static string RateCardUrl => $"{Url}/pm/ratecard";
        public static string MotherPCAUrl => $"{Url}/pm/motherpca";
        public static string MasterJobStatus => $"{Url}/pm/MasterJobStatus";
        public static string MasterNonJob => $"{Url}/pm/MasterNonJob";
        public static string MasterDayTimesheetUserLocked => $"{Url}/pm/MasterDayTimesheetUserLocked";
        public static string MasterCampaignType => $"{Url}/pm/MasterCampaignType";
        public static string MasterPmBusinessType => $"{Url}/pm/MasterPmBusinessType";
        public static string MasterJobType => $"{Url}/pm/MasterJobType";
        public static string DamApproval => $"{Url}/pm/DamApproval";
        public static string MyDam => $"{Url}/pm/mydam";
        public static string ReportTask => $"{Url}/pm/ReportTask";

        #endregion

        #region Accounting
        public static string GeneralLedgerDetailUrl => $"{Url}/generalledgerdetail";
        public static string JournalEntryUrl => $"{Url}/journalentry";
        public static string JournalAdjustmentUrl => $"{Url}/journaladjustment";
        public static string AmortizationUrl => $"{Url}/amortization";
        public static string SalaryAllocationUrl => $"{Url}/salaryallocation";

        #endregion

        #region UM
        public static string ManageUserAccessUrl => $"{Url}/ManageUserAccess";
        #endregion

        #region "Hris old"
        public static string Groupurl => $"{Url}/Group";
        public static string SubgroupUrl => $"{Url}/Subgroup";
        public static string DivisionUrl => $"{Url}/Division";
        public static string DepartementUrl => $"{Url}/Departement";
        public static string TrainingUrl => $"{Url}/Training";
        public static string TrainingRequisitionUrl => $"{Url}/TrainingRequisition";
        public static string TrainingRequisitionDetailUrl => $"{Url}/TrainingRequisitionDetail";
        public static string TrainingCategoryUrl => $"{Url}/TrainingCategory";
        public static string TrainingSubjectUrl => $"{Url}/TrainingSubject";
        public static string TrainingTypeUrl => $"{Url}/TrainingType";
        public static string TrainingBondingUrl => $"{Url}/TrainingBonding";
        #endregion
    }
}