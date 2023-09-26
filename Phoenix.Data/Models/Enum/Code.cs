using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models
{
    public static class Code
    {
        #region HRIS
        public const string Resignation = "REQRSG";
        public const string Contract = "REQEMP";
        public const string Death = "REQDH";
        public const string Sanction = "REQSC";
        public const string Retired = "RTD";

        public const string ServiceRequest = "SRV";
        public const string Leave = "REQL";
        public const string MassLeave = "MASL";

        public const string Promotion = "REQP";
        public const string Rotation = "REQR";
        public const string Demotion = "REQD";
        public const string LegalDocument = "LGD";
        public const string LegalSharing = "LGS";
        public const string Overtime = "OVT";
        public const string PDRPerformance = "PDRP";
        public const string PDRApprailsal = "PDRA";
        public const string Payroll = "PYR";
        public const string TrainingRegPayment = "TRGP";


        //Finance
        public const string MediaPlanTV = "OPT/MEDIA";
        public const string InvoiceReceived = "INVR";
        public const string InvoiceClientCode = "INCL";
        public const string InvoiceDeliveryNotes = "ND";
        //public const string TemporaryAccountPayable = "TAP";
        #endregion

        #region PM
        public const string PCA = "PCA";
        public const string PCE = "PCE";
        #endregion

        #region "galih"
        public const string Budget = "BGT";
        public const string TAP = "TAP";
        public const string AP = "PS";
        public const string RFQ = "RFQ";
        public const string PO = "PO";
        public const string PR = "REQ";
        public const string PaymentRequest = "CV";
        public const string BankReceived = "CR";
        public const string TrainingRequest = "TRREQ";
        public const string TrainingBonding = "TRBOND";
        #endregion

        #region finance
        public const string MasterClient = "MC";
        public const string MasterVendor = "MV";
        public const string MasterClientBrand = "MCB";
        public const string MasterVendorBrand = "MVB";
        public const string CashAdvance = "CA";
        public const string CashAdvanceSattlement = "CAST";
        public const string InventoryIn = "INV";
        public const string GoodsReceipt = "GR";
        public const string ServiceReceipt = "SR";
        public const string FinancialYear = "FY";
        public const string FinancePeriod = "FP";
        public const string MasterLocationProcurement = "MLP";
        public const string MasterConditionProcurement = "MDP";
        public const string MasterDepreciation = "MD";
        public const string MasterPettyCash = "MSPC";
        public const string PettyCash = "PC";
        public const string JobClosing = "JC";
        public const string FixedAssetMovement = "FAM";
        public const string FixedAsset = "FX";
        public const string FixedAssetDepreciation = "FXD";
        public const string MasterAllocationRatio = "CAR";
        public const string BudgetCodeCapex = "BCC";
        public const string BudgetCodeOpex = "BCO";
        public const string BudgetMasterCapex = "BMC";
        public const string BudgetMasterCapexDetail = "BMCD";
        public const string BudgetMasterOpex = "BMO";
        public const string BudgetMasterOpexDetail = "BMOD";
        public const string MonthlyBudgetSpread = "MBS";
        public const string MasterOpeningBalance = "O/BAL";

        #endregion

        #region Acconting
        public const string JournalEntry = "JE";
        public const string JournalAdjustment = "JA";
        public const string JournalVoucher = "JV";
        public const string Amortization = "AZ";
        public const string SalaryAllocation = "SA";
        public const string Revaluation = "REV";
        public const string SalaryPaymentRequest = "PS";
        public const string JournalWriteOffAp = "JAP";
        public const string JournalWriteOffAr = "JAR";
        public const string CASettlementJournalReverse = "CSR";        
        #endregion

        #region Media
        public const string MediaPlanHeaderID = "MP";
        public const string MediaPlanDetailID = "MPLE";
        public const string MediaOrderHeader = "MO";
        #endregion
    }
}
