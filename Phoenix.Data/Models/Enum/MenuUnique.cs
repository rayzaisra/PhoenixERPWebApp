using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models
{
    public static class MenuUnique
    {
        public const string Rotation = "ROTATIONREQ";
        public const string Promotion = "PROMOTIONREQ";
        public const string Demotion = "DEMOTIONREQ";
        //public const string LegalDocument = "DOCUMENTLEGALREQ";
        public const string LegalDocumentFullfilment = "DOCUMENTLEGALFULLFILMENTREQ";
        public const string LegalSharingRequest = "DOCLEGALSHARINGREQ";
        public const string Sanction = "SNCREQ";
        public const string Retired = "RTDREQ";
        public const string Resignation = "TEREMPRESREQ";
        public const string MassLeave = "MASSLEAVEREQ";
        public const string Leave = "LEAVEREQ";
        public const string Overtime = "OVERTIMEREQ";
        public const string PdrPerformance = "PDRPERFORMANCEREQ";
        public const string PdrApprailsal = "PDRAPPRAILSALREQ";
        public const string PCA = "PCAREQ";
        public const string PCE = "PCEREQ";
        public const string PMPCE = "PMPCEREQ";
        public const string CashAdvance = "ProcCashAdvance";
        public const string CashAdvanceSettlement = "ProcCashAdvanceSattlement";
        public const string Payroll = "PAYROLLREQ";
        public const string TrainingRequisition = "TRNREQ";
        public const string EmployeeLoan = "PAYROLLEMPLOAN";
        public const string EmployeeLoanCoop = "PAYROLLEMPLOANCOOP";
        public const string RECMPP = "RECMPP";
        public const string EmployeePayroll = "PAYROLLEMPPROFILE";
        public const string ExitInterView = "TEREXITINTER";  


        #region Finance
        public const string PurchaseRequest = "ProcPurchaseRequest";
        public const string RFQ = "ProcRFQ";
        public const string PO = "ProcPurchaseOrder";
        public const string PaymentRequest = "ProcPaymentReq";
        public const string FinancialYear = "FinancialYearReq";
        public const string FinancePeriod = "FinancePeriodReq";
        public const string JobClosing = "FNJOBCLOSING";
        public const string GoodRequest = "INVMOVEGOODREQUEST";
        public const string PettyCash = "ProcPettyCash";
        public const string InvoiceClient = "FININVCLNT";
        public const string InvoiceClientMultiplePce = "FININVCLNTMTLP";
        public const string FixedAssetDepreciation = "FADEPRECIATION";
        public const string BankReceived = "ProcBankReceived";
        public const string MasterBudgetCapex = "MSTFNBUDGETCAPEX";
        public const string MasterBudgetOpex = "MSTFNBUDGETOPEX";
        public const string JournalAdjustment = "FNJOURNALADJUSTMENT";
        public const string SalaryAllocation = "FNSALARYALLOCATION";
        public const string Revaluation = "FNREVALUATION";
        public const string SalaryPaymentRequest = "FNSALARYPAYMENTREQUEST";
        public const string InventoryGoodReturn = "GRT";
        public const string FixedAssetMovement = "FAMOV";
        public const string FixedAssetOutMovement = "FAOUTMOV";
        #endregion

        #region Media
        public const string MediaPlan = "MEDPLANESTMREQ";
        public const string MediaOrder = "MEDIAORDER";
        #endregion

        public const string TimeSheet = "TMSHEETUSR";
        public const string DamFileShared = "DAMSHR";
        public const string MyDam = "MMDAM";
    }
}
