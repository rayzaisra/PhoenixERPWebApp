using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance
{
    public class PettyCash : FinanceEntityBase
    {
        [DisplayName("Petty Cash ID")]
        public string Id { get; set; }

        [DisplayName("Bank Account")]
        public string RefferenceNo { get; set; }

        [DisplayName("Settlement Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? SettlementDate { get; set; }

        [DisplayName("Holder Id")]
        public string HolderId { get; set; }

        [DisplayName("Division Id")]
        public string DivisionId { get; set; }

        [DisplayName("Exchange Rate")]
        public float? ExchangeRate { get; set; }

        [DisplayName("Remarks")]
        public string Remarks { get; set; }

        [DisplayName("Status")]
        public int Status { get; set; }

        [DisplayName("Opening Balance")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? MasterPettyCashAmount { get; set; }

        [DisplayName("Total")]
        public decimal? TotalCreditAmount { get; set; }

        [DisplayName("Ending Balance")]
        public decimal? EndingBalance { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        [DisplayName("COA")]
        public string CoaId { get; set; }

        [DisplayName("Evidence")]
        public string FilemasterId { get; set; }

        [DisplayName("Account Number")]
        public string BankAccountNumber { get; set; }

        //Not Mapped
        //For List Petty Cash
        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }

        //[DisplayName("Bank Name")]
        //public string BankName { get; set; } 

        [DisplayName("Status")]
        public string StatusName { get; set; }

        [DisplayName("Holder Name")]
        public string HolderName { get; set; }

        [DisplayName("Holder Name2")]
        public string HolderName2 { get; set; }

        //=========== Start For Detail Forms ================
        [DisplayName("Evidence")]
        public string FilemasterName { get; set; }

        [DisplayName("Division")]
        public string DivisionName { get; set; }

        public List<PettyCashDetail> Detail { get; set; }

        //For Approval
        public string LogDescription { get; set; }

        //=========== End For Detail Forms ================ 
    }
}
