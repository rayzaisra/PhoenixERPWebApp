using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models
{
    public static class ApprovalLink
    {
        public const string Rotation = "/hrisnew/rotation/addedit";
        public const string Promotion = "/hrisnew/promotion/addedit";
        public const string Demotion = "/hrisnew/demotion/addedit";
        //public const string LegalDocument = "/hrisnew/legaldocumentRequest/addedit";
        public const string LegalDocumentFullfilment = "/hrisnew/legaldocumentrequestfullfilment/addedit";
        public const string LegalSharingRequest = "/hrisnew/legalsharingrequest/addedit";
        public const string Sanction = "/hrisnew/sanction/addedit";
        public const string Retired = "/hrisnew/retired/addedit";
        public const string Resignation = "/hrisnew/employeeresignation/addedit";
        public const string MassLeave = "/hrisnew/massleave/addedit";
        public const string Leave = "/hrisnew/leave/addedit";
        public const string Overtime = "/hrisnew/overtime/addedit";
        public const string PdrPerformance = "/hrisnew/pdrperformance/addedit";
        public const string PdrApprailsal = "/hrisnew/pdrapprailsal/addedit";
        public const string Payroll = "/hrisnew/payroll/addedit";
        public const string EmployeeLoan = "/hrisnew/EmployeeLoan/AddEdit";
        public const string EmployeeLoanCoop = "/hrisnew/EmployeeLoanCoop/AddEdit";
        public const string ExitInterview = "/hrisnew/exitinterview/addedit";

        public const string PCA = "/pm/pca/addedit";
        public const string PCE = "/pm/pce/addedit";
        public const string CashAdvance = "/finance/cashadvance/detail";
        public const string CashAdvanceSettlement = "/finance/cashadvancesattlement/settle";
        public const string InvoiceClient = "/finance/InvoiceClient/Detail";
        public const string InvoiceClientMultiplePce = "/finance/nvoiceClientMultiPce/Detail";
        public const string TrainingRequisition = "/Hrisnew/TrainingRequisition/Detail";
        public const string MPP = "/hris/TrainingRequisition/Detail";



        #region Finance
        public const string PurchaseRequest = "/finance/PurchaseRequest/Detail";
        public const string RFQ = "/finance/requestforquotation/detail";
        public const string PO = "/Finance/PurchaseOrder/detail";
        public const string PaymentRequest = "/finance/PaymentRequest/Detail";
        public const string FinancialYear = "/finance/FinancialYear/Detail";
        public const string FinancePeriod = "/finance/FinancePeriod/Detail";
        public const string JobClosing = "/Finance/JobClosing/Detail";
        public const string PettyCash = "/Finance/PettyCash/Detail";
        public const string GoodRequest = "/Finance/GoodRequest/Detail";
        public const string FixedAssetDepreciation = "/Finance/FixedAssetDepreciation/Detail";
        public const string BankReceived = "/finance/BankReceived/Detail";
        public const string MasterBudgetCapex = "/finance/budgetcapex/addedit";
        public const string MasterBudgetOpex = "/finance/budgetopex/addedit";
        public const string JournalAdjustment = "/finance/journaladjustment/detail";
        public const string SalaryAllocation = "/finance/SalaryAllocation/detail";
        public const string Revaluation = "/finance/RevaluationList/addedit";
        public const string SalaryPaymentRequest = "/finance/SalaryPaymentRequest/addedit";
        public const string InventoryGoodRequest = "/finance/GoodReturn/Detail";
        public const string FixedAssetMovement = "/finance/fixedassetmovement/detail";
        public const string FixedAssetOutMovement = "/finance/fixedassetoutmovement/detail";
        #endregion

        #region Media
        public const string MediaPlanEstimates = "/media/mediaplanestimates/addedit";
        #endregion

        public const string TimeSheet = "/pm/timesheet/Index";
        public const string DamApproval = "/pm/DamApproval/Detail";
        public const string MyDam = "/pm/mydam/Detail";
    }
}
