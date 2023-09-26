using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models.Enum
{
    public class SourceJournal
    {
        public static string AccountPayable = "AP";
        public static string AccountReceivable = "AR";
        public static string BankReceived = "BR";
        public static string PaymentRequest = "PR";
        //public static string CashAdvanceSettlement = "CAS"; --di cash advance settlement sudah tidak pakai journal lagi.
        public static string JobClosing = "JC";
        public static string PettyCash = "PC";
        public static string PettyCashSettlement = "PCS";
        public static string Amortization = "AMORTIZATION";
        public static string JournalAdjustment = "JA";
        public static string JournalEntry = "JE";
        public static string SalaryAllocation = "SA";
        public static string SalaryPaymentRequest = "SPR";
        public static string MasterOpeningBalance = "MOB";
        public static string JournalWriteOffAp = "WriteOffAp";
        public static string JournalWriteOffAr = "WriteOffAr";
        public static string JournalFixedAssetDepreciation = "FAD";
        public static string CASettlementJournalReverse = "CASREVERSE";
    }
}
