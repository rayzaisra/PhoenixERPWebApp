using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class BankReceivedDetailJournal : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Bank Received Id")]
        public string BankReceivedId { get; set; }

        [DisplayName("Account ID-Name")]
        public string CoaId { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Condition")]
        public string Condition { get; set; }

        [DisplayName("Debit")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Debit { get; set; }

        [DisplayName("Credit")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Credit { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Affiliation")]
        public string AffiliationId { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TransactionDate { get; set; }

        ////Not Mapped 

        public string AccountName { get; set; }
    }
}