using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class MasterPettyCash : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Currency Id")]
        public string CurrencyId { get; set; }

        [DisplayName("Chart Account ID")]
        public string ChartAccountId { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        public bool IsSettle { get; set; }

        [DisplayName("Bank Account Name")]
        public string BankAccountName { get; set; }

        [DisplayName("Bank Account Number")]
        public string BankAccountNumber { get; set; }

        [DisplayName("Chart Account Name")]
        public string ChartAccountName { get; set; }

        [DisplayName("Chart Account Number")]
        public string ChartAccountNumber { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }
        
        public decimal? AmountInsused { get; set; }

        [DisplayName("Balance")]
        public decimal? Balance { get; set; }

        [DisplayName("Holder Name 1")]
        public string HolderUserId { get; set; }

        [DisplayName("Holder Name 2")]
        public string DelegateUserId { get; set; }

        //Not Mapped
        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }

        [DisplayName("Currency")]
        public string CurrencyCode { get; set; }

        [DisplayName("COA Level 5 ID")]
        public string CoaCodeRecName { get { return ChartAccountNumber + " - " + ChartAccountName; } }
    }
}