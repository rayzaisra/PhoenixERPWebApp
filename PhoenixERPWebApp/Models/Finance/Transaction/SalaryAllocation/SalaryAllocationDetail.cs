using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class SalaryAllocationDetail : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Journal Entry Id")]
        public string JournalEntryId { get; set; }

        [DisplayName("Account Name (Level 5 COA)")]
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

        //NotMapped
        [DisplayName("Account Name (Level 5 COA)")]
        public string AccountName { get; set; }

        [DisplayName("Account ID")]
        public string CodeRec { get; set; }
    }
}