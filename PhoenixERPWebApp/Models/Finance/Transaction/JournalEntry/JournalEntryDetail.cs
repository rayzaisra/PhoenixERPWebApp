using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class JournalEntryDetail : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Journal Entry Id")]
        public string JournalEntryId { get; set; }

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
        
        [DisplayName("Job ID-Name")]
        public string JobId { get; set; }

        [DisplayName("PCE ID")]
        public string PceId { get; set; }

        [DisplayName("PCA ID")]
        public string PcaId { get; set; }

        [DisplayName("Task List Name")]
        public string PcaTaskId { get; set; }

        ////Not Mapped 

        [DisplayName("Job Name")]
        public string JobName { get; set; }

        public string PcaTask { get; set; }

        public string AccountName { get; set; }

        public string BusinessUnitName { get; set; }

        //[DisplayName("Account ID")]
        //public string CodeRec { get; set; }

        [DisplayName("Afiliation")]
        public string AfiliationName { get; set; }
    }
}